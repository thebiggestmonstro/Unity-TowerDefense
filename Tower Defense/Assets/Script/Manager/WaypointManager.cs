using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    public static WaypointManager Instance { get; private set; }

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

    public Transform[] GetWaypoints() => waypoints;
}
