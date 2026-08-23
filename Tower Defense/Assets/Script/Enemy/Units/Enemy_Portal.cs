using System.Collections.Generic;
using UnityEngine;

public class Enemy_Portal : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField]
    private float spawnCooldown = 2.0f;
    [SerializeField]
    private List<Waypoint> waypointsList;

    private float spawnTimer;
    private bool isSpawning = false;

    private void Awake()
    {
        CollectWaypoints();
    }

    private void OnEnable()
    {
        if (GameServices.EnemySpawner != null)
        {
            GameServices.EnemySpawner.RegisterPortal(this);
        }
    }

    private void Start()
    {
        if (GameServices.EnemySpawner != null && !GameServices.EnemySpawner.HasPortal(this))
        {
            GameServices.EnemySpawner.RegisterPortal(this);
        }
    }

    private void OnDisable()
    {
        if (GameServices.EnemySpawner != null)
        {
            GameServices.EnemySpawner.UnregisterPortal(this);
        }
    }

    private void Update()
    {
        if (!isSpawning)
        {
            return;
        }

        if (!GameServices.EnemySpawner.HasEnemiesLeft())
        {
            isSpawning = false;
            return;
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnProcess();
            spawnTimer = spawnCooldown;
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
    }

    public void SpawnProcess()
    {
        GameObject enemy = GameServices.EnemySpawner.RequestSpawnEnemy();

        if (enemy != null)
        {
            GameObject spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            GameServices.EnemySpawner.RegisterActiveEnemy(spawnedEnemy);

            Enemy_Base enemyComponent = spawnedEnemy.GetComponent<Enemy_Base>();
            if (enemyComponent != null)
            {
                enemyComponent.SetupEnemy(waypointsList, this);
            }
        }
        else
        {
            isSpawning = false;
        }
    }

    private void CollectWaypoints()
    {
        waypointsList = new List<Waypoint>();

        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();

            if (waypoint != null)
            {
                waypointsList.Add(waypoint);
            }
        }
    }
}
