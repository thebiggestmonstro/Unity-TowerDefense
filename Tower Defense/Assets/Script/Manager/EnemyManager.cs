using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Transform enemySpawner;
    [SerializeField]
    private float spawnCooldown;
    private float spawnTimer;

    public static EnemyManager Instance { get; private set; }

    [Header("Enemy Prefab")]
    [SerializeField]
    private GameObject basicEnemy;

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

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(basicEnemy, enemySpawner.position, Quaternion.identity);
    }
}
