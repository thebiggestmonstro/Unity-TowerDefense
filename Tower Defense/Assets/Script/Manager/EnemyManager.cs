using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Wave Data Assets")]
    [SerializeField]
    private List<WaveData> allWaves;
    private int currentWaveIndex = 0;

    [Space]
    [SerializeField]
    private Transform enemySpawner;
    [SerializeField]
    private float spawnCooldown;
    private float spawnTimer;

    public static EnemyManager Instance { get; private set; }
    private List<GameObject> enemiesToCreate;

    private void Awake()
    {
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

    private void Update()
    {
        if (enemiesToCreate.Count <= 0)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    public void StartWave(int waveIndex)
    {
        if (waveIndex >= allWaves.Count)
        {
            Debug.Log("Clear!!!");
            return;
        }

        currentWaveIndex = waveIndex;
        enemiesToCreate = CreateNewEnemyWave(allWaves[currentWaveIndex]);
        spawnTimer = spawnCooldown;
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

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        if (randomEnemy)
        {
            GameObject newEnemy = Instantiate(randomEnemy, enemySpawner.position, Quaternion.identity);
        }
    }

    private GameObject GetRandomEnemy()
    {
        if (enemiesToCreate.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chosenEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.RemoveAt(randomIndex);
        return chosenEnemy;
    }

    public void NextWave()
    {
        StartWave(currentWaveIndex + 1);
    }
}
