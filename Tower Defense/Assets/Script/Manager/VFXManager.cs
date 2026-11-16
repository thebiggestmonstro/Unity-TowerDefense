using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXManager : MonoBehaviour
{
    private readonly Dictionary<GameObject, ObjectPool<PooledVFX>> pools = new();
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> manualPools = new();

    private void OnEnable() => GameServices.Register(this);
    private void OnDisable() => GameServices.Unregister(this);

    // Auto Control
    public void Play(GameObject prefab, Vector3 position, Quaternion rotation, float scale = 1f)
    {
        if (prefab == null)
        {
            return;
        }

        if (!pools.TryGetValue(prefab, out ObjectPool<PooledVFX> pool))
        {
            pool = CreatePool(prefab);
            pools[prefab] = pool;
        }

        pool.Get().Play(position, rotation, scale);
    }

    private ObjectPool<PooledVFX> CreatePool(GameObject prefab)
    {
        ObjectPool<PooledVFX> pool = null;

        pool = new ObjectPool<PooledVFX>(
            createFunc: () =>
            {
                GameObject instance = Instantiate(prefab, transform);
                instance.SetActive(false);
                PooledVFX vfx = instance.AddComponent<PooledVFX>();
                vfx.Initialize(v => pool.Release(v));
                return vfx;
            },
            actionOnRelease: vfx => vfx.gameObject.SetActive(false),
            actionOnDestroy: vfx => Destroy(vfx.gameObject),
            collectionCheck: false,
            defaultCapacity: 8,
            maxSize: 32
        );

        return pool;
    }

    // Manual control
    public GameObject Get(GameObject prefab)
    {
        if (prefab == null)
        {
            return null;
        }

        return GetManualPool(prefab).Get();
    }

    public void Release(GameObject prefab, GameObject instance)
    {
        if (prefab == null || instance == null)
        {
            return;
        }

        if (manualPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
        {
            pool.Release(instance);
        }
    }

    public void Prewarm(GameObject prefab, int count)
    {
        if (prefab == null || count <= 0)
        {
            return;
        }

        ObjectPool<GameObject> pool = GetManualPool(prefab);
        int missing = count - pool.CountAll;

        if (missing <= 0)
        {
            return;
        }

        GameObject[] warmed = new GameObject[missing];

        for (int i = 0; i < missing; i++)
        {
            warmed[i] = pool.Get();
        }

        for (int i = 0; i < missing; i++)
        {
            pool.Release(warmed[i]);
        }
    }

    private ObjectPool<GameObject> GetManualPool(GameObject prefab)
    {
        if (!manualPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
        {
            pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject instance = Instantiate(prefab, transform);
                    instance.SetActive(false);
                    return instance;
                },
                actionOnGet: instance => instance.SetActive(true),
                actionOnRelease: instance => instance.SetActive(false),
                actionOnDestroy: instance => Destroy(instance),
                collectionCheck: false,
                defaultCapacity: 100,
                maxSize: 300
            );

            manualPools[prefab] = pool;
        }

        return pool;
    }
}
