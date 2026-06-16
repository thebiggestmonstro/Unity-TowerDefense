using UnityEngine;

public class Enemy_Portal : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField]
    private float spawnCooldown = 2.0f;

    private float spawnTimer;
    private bool isSpawning = false;

    private void OnEnable()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterPortal(this);
        }
    }

    private void Start()
    {
        if (WaveManager.Instance != null && !WaveManager.Instance.HasPortal(this))
        {
            WaveManager.Instance.RegisterPortal(this);
        }
    }

    private void OnDisable()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.UnregisterPortal(this);
        }
    }

    private void Update()
    {
        if (!isSpawning)
        {
            return;
        }

        if (!WaveManager.Instance.HasEnemiesLeft())
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
        GameObject enemy = WaveManager.Instance.RequestSpawnEnemy();

        if (enemy != null)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
        else
        {
            isSpawning = false;
        }
    }
}
