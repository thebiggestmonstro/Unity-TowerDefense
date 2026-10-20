using UnityEngine;
using UnityEngine.Pool;

public class Pool_MachineGun : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] 
    private Projectile_MachineGun projectilePrefab;
    [SerializeField] 
    private int initialSize = 50;
    [SerializeField] 
    private int maxSize = 200;

    private ObjectPool<Projectile_MachineGun> projectilePool;

    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile_MachineGun>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile,
            actionOnDestroy: OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: initialSize,
            maxSize: maxSize
        );

        for (int i = 0; i < initialSize; i++)
        {
            Projectile_MachineGun projectile = projectilePool.Get();
            projectilePool.Release(projectile);
        }
    }

    private Projectile_MachineGun CreateProjectile()
    {
        Projectile_MachineGun projectile = Instantiate(projectilePrefab);

        projectile.SetPool(this);
        projectile.gameObject.SetActive(false);

        return projectile;
    }

    private void OnGetProjectile(Projectile_MachineGun projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile_MachineGun projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile_MachineGun projectile)
    {
        Destroy(projectile.gameObject);
    }

    public void Fire(IDamageable target, Vector3 targetPosition, float damage, float speed, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Projectile_MachineGun projectile = projectilePool.Get();

        projectile.transform.SetPositionAndRotation(
            spawnPosition,
            spawnRotation
        );

        projectile.SetupProjectile(
            target,
            targetPosition,
            damage,
            speed
        );
    }

    public void ReturnToPool(Projectile_MachineGun projectile)
    {
        projectilePool.Release(projectile);
    }
}
