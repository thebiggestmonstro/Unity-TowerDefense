using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<Enemy_Portal> activePortals;

    private Queue<GameObject> enemiesToCreate = new Queue<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void OnEnable()
    {
        GameServices.RegisterEnemySpawner(this);
    }

    private void OnDisable()
    {
        GameServices.UnregisterEnemySpawner(this);
    }

    public void BeginWave(WaveData waveData)
    {
        List<GameObject> waveList = CreateNewEnemyWave(waveData);
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

    public bool HasPortal(Enemy_Portal portal) => activePortals.Contains(portal);

    public List<Enemy_Portal> GetActivePortals() => activePortals;

    public GameObject RequestSpawnEnemy()
    {
        return enemiesToCreate.Count > 0 ? enemiesToCreate.Dequeue() : null;
    }

    public bool HasEnemiesLeft() => enemiesToCreate.Count > 0;

    public bool AllEnemiesDefeated() => activeEnemies.Count <= 0;

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

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
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
}
