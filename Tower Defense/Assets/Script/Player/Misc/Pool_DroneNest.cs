using UnityEngine;
using UnityEngine.Pool;

public class Pool_DroneNest : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField]
    private GameObject dronePrefab;
    [SerializeField]
    private int defaultCapacity = 4;
    [SerializeField]
    private int maxSize = 8;

    private ObjectPool<GameObject> dronePool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (dronePool != null)
        {
            return;
        }

        dronePool = new ObjectPool<GameObject>(
            createFunc: CreateDrone,
            actionOnGet: drone => drone.SetActive(true),
            actionOnRelease: OnReleaseDrone,
            actionOnDestroy: drone => Destroy(drone),
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private GameObject CreateDrone()
    {
        GameObject drone = Instantiate(dronePrefab);
        drone.GetComponent<Projectile_Drone>().SetReturnAction(dronePool.Release);
        return drone;
    }

    private void OnReleaseDrone(GameObject drone)
    {
        drone.GetComponent<Projectile_Drone>().ResetDrone();
        drone.SetActive(false);
    }

    public GameObject GetDrone(Transform attachPoint)
    {
        InitializePool();

        GameObject drone = dronePool.Get();

        drone.transform.SetParent(attachPoint, false);
        drone.transform.localScale = dronePrefab.transform.localScale;
        drone.transform.SetPositionAndRotation(attachPoint.position, Quaternion.identity);

        return drone;
    }
}