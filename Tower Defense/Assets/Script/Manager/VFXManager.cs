using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFXManager : MonoBehaviour
{
    private readonly Dictionary<GameObject, ObjectPool<PooledVFX>> pools = new Dictionary<GameObject, ObjectPool<PooledVFX>>();

    private void OnEnable() => GameServices.Register(this);
    private void OnDisable() => GameServices.Unregister(this);

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
}
