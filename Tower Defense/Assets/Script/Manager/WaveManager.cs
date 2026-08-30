using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Data Assets")]
    [SerializeField]
    private List<WaveData> allWaves;
    private int currentWaveIndex = 0;

    [Space]
    [Header("Wave Time Setting")]
    [SerializeField]
    private float timeBetweenWaves = 5.0f;
    private float waveTimer;

    [Space]
    [Header("Collaborators")]
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private LevelTransitionHandler levelTransitionHandler;

    private float checkInterval = 0.5f;
    private bool isForcedSkip = false;
    private bool bIsWaveManagerActive = false;

    private void OnEnable()
    {
        GameServices.Register(this);
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
    }

    private IEnumerator CoWaveLoop()
    {
        while (currentWaveIndex < allWaves.Count)
        {
            enemySpawner.BeginWave(allWaves[currentWaveIndex]);
            GameEvents.RaiseWaveTimerVisibilityChanged(false);

            while (enemySpawner.HasEnemiesLeft() || !enemySpawner.AllEnemiesDefeated())
            {
                yield return new WaitForSeconds(checkInterval);
            }

            currentWaveIndex++;
            AdvanceLevelLayout();
            if (currentWaveIndex >= allWaves.Count)
            {
                break;
            }

            waveTimer = timeBetweenWaves;
            isForcedSkip = false;
            GameEvents.RaiseWaveTimerVisibilityChanged(true);

            while (waveTimer > 0 && !isForcedSkip)
            {
                waveTimer -= Time.deltaTime;
                GameEvents.RaiseWaveTimerUpdated(waveTimer);
                yield return null;
            }

            isForcedSkip = false;
            GameEvents.RaiseWaveTimerVisibilityChanged(false);
        }

        GameEvents.RaiseWaveTimerVisibilityChanged(false);
        Debug.Log("Clear!!!");
    }

    private void AdvanceLevelLayout()
    {
        if (currentWaveIndex >= allWaves.Count)
        {
            return;
        }

        levelTransitionHandler.TransitionToWave(allWaves[currentWaveIndex]);
    }

    public void ForceStartNextWave()
    {
        if (waveTimer > 0)
        {
            isForcedSkip = true;
        }
    }

    public WaveData GetCurrentWaveData()
    {
        return allWaves[currentWaveIndex];
    }

    public WaveData GetNextWaveData()
    {
        int nextIndex = currentWaveIndex + 1;
        if (nextIndex >= allWaves.Count)
        {
            return null;
        }

        return allWaves[nextIndex];
    }

    [ContextMenu("Active Wave Manager")]
    public void ActivateWaveManager()
    {
        if (bIsWaveManagerActive)
        {
            return;
        }

        if (allWaves != null && allWaves.Count > 0)
        {
            bIsWaveManagerActive = true;
            StartCoroutine(CoWaveLoop());
            Debug.Log("Wave Manager Activated!");
        }
        else
        {
            Debug.LogError("Wave Data is not registered in EnemyManager");
        }
    }
}