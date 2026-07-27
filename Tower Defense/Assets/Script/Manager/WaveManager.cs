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
    private Queue<GameObject> enemiesToCreate;

    [Space]
    [Header("Spawners")]
    [SerializeField]
    private List<Enemy_Portal> activePortals;

    [Space]
    [Header("Wave Time Setting")]
    [SerializeField]
    private float timeBetweenWaves = 5.0f;
    [SerializeField]
    private float waveTimer;

    [Space]
    [Header("Grid Setting")]
    [SerializeField]
    private GridBuilder currentGrid;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float checkInterval = 0.5f;
    private bool isForcedSkip = false;

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
            StartCoroutine(CoWaveLoop());
        }
        else
        {
            Debug.LogError("Wave Data is not registered in EnemyManager");
        }
    }

    private IEnumerator CoWaveLoop()
    {
        while (currentWaveIndex < allWaves.Count)
        {
            InitWave(currentWaveIndex);

            UIManager.GetUI<UI_InGame>("UI_InGame").EnableWaveTimerText(false);

            while (HasEnemiesLeft() || !AllEnemiesDefeated())
            {
                yield return new WaitForSeconds(checkInterval);
            }

            currentWaveIndex++;
            CheckForNewLevelLayout();
            if (currentWaveIndex >= allWaves.Count)
            {
                break;
            }

            waveTimer = timeBetweenWaves;
            isForcedSkip = false;
            UIManager.GetUI<UI_InGame>("UI_InGame").EnableWaveTimerText(true);

            while (waveTimer > 0 && !isForcedSkip)
            {
                waveTimer -= Time.deltaTime;
                UIManager.GetUI<UI_InGame>("UI_InGame").UpdateWaveTimerText(waveTimer);
                yield return null; 
            }

            isForcedSkip = false;
            UIManager.GetUI<UI_InGame>("UI_InGame").EnableWaveTimerText(false);
        }

        UIManager.GetUI<UI_InGame>("UI_InGame").EnableWaveTimerText(false);
        Debug.Log("Clear!!!");
    }

    private void InitWave(int waveIndex)
    {
        List<GameObject> waveList = CreateNewEnemyWave(allWaves[waveIndex]);
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

    public bool HasEnemiesLeft()
    {
        return enemiesToCreate.Count > 0;
    }

    public List<GameObject> GetActiveEnemies() => activeEnemies;

    public void RegisterActiveEnemy(GameObject enemyToRegister)
    {
        if (!activeEnemies.Contains(enemyToRegister))
        {
            activeEnemies.Add(enemyToRegister);    
        }
    }

    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        { 
            activeEnemies.Remove(enemyToRemove);
        }
    }

    private bool AllEnemiesDefeated()
    {
        return GetActiveEnemies().Count <= 0;
    }

    private void CheckForNewLevelLayout()
    {
        if (currentWaveIndex >= allWaves.Count)
        {
            return;
        }

        WaveData nextWave = allWaves[currentWaveIndex];

        if (nextWave)
        {
            UpdateLevelGrid(nextWave.currentWaveGrid);
            UpdateLevelPortals(nextWave.newPortals);
            currentGrid.GetNavMesh()?.BuildNavMesh();
            BuildManager.Instance.MakeBuildTileAvaliablityFalse(currentGrid);
        }
    }

    private void UpdateLevelGrid(GridBuilder nextGridPrefab)
    {
        if (nextGridPrefab == null)
        {
            return;
        }

        if (currentGrid != null)
        {
            Destroy(currentGrid.gameObject);
        }

        GridBuilder newGridInstance = Instantiate(nextGridPrefab, Vector3.zero, Quaternion.identity);
        currentGrid = newGridInstance;
        BuildManager.Instance.currentGrid = currentGrid;
        BuildManager.Instance.MakeBuildTileAvaliablityFalse(currentGrid);
    }
    
    private void UpdateLevelPortals(Enemy_Portal[] portalPrefabs)
    {
        if (portalPrefabs == null || portalPrefabs.Length <= 0)
        {
            return;
        }

        foreach (Enemy_Portal portalPrefab in portalPrefabs)
        {
            if (portalPrefab == null)
            {
                continue;
            }

            Enemy_Portal createdPortal = Instantiate(
                portalPrefab,
                portalPrefab.transform.position,
                portalPrefab.transform.rotation
            );

            RegisterPortal(createdPortal);
        }
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
}