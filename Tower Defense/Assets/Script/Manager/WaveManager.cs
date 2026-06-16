using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Data Assets")]
    [SerializeField]
    private List<WaveData> allWaves;
    private int currentWaveIndex = 0;
    private Queue<GameObject> enemiesToCreate;

    [Space]
    [Header("Spawners")]
    [SerializeField]
    private List<Enemy_Portal> activePortals;

    public static WaveManager Instance { get; private set; }
    

    private void Awake()
    {
        enemiesToCreate = new Queue<GameObject>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (allWaves != null && allWaves.Count > 0)
        {
            StartWave(0);
        }
        else
        {
            Debug.LogError("Wave Data is not registered in EnemyManager");
        }
    }

    public void RegisterPortal(Enemy_Portal portal)
    {
        if (!activePortals.Contains(portal))
        {
            activePortals.Add(portal);
        }
    }

    public void UnregisterPortal(Enemy_Portal portal)
    {
        if (activePortals.Contains(portal))
        {
            activePortals.Remove(portal);
        }
    }

    public bool HasPortal(Enemy_Portal portal)
    { 
        return activePortals.Contains(portal);
    }

    public void StartWave(int waveIndex)
    {
        if (waveIndex >= allWaves.Count)
        {
            Debug.Log("Clear!!!");
            return;
        }

        currentWaveIndex = waveIndex;
        List<GameObject> waveList = CreateNewEnemyWave(allWaves[currentWaveIndex]);
        ShuffleList(waveList);
        enemiesToCreate = new Queue<GameObject>(waveList);

        foreach (var portal in activePortals)
        {
            if (portal != null)
            {
                portal.StartSpawning();
            }
        }
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    private List<GameObject> CreateNewEnemyWave(WaveData waveData)
    {
        List<GameObject> newEnemyList = new List<GameObject>();

        foreach (var info in waveData.waveInfo)
        {
            if (info.enemyPrefab == null)
            {
                continue;
            }

            for (int i = 0; i < info.spawnCount; i++)
            {
                newEnemyList.Add(info.enemyPrefab);
            }
        }

        return newEnemyList;
    }

    public GameObject RequestSpawnEnemy()
    {
        if (enemiesToCreate.Count == 0)
        {
            return null; 
        }

        return enemiesToCreate.Dequeue();
    }

    [ContextMenu("Setup Next Wave")]
    public void NextWave()
    {
        StartWave(currentWaveIndex + 1);
    }

    public bool HasEnemiesLeft()
    {
        return enemiesToCreate.Count > 0;
    }
}
