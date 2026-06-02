using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyType
{ 
    Basic,
    Fast,
    None
}

public class Enemy_Base : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float arrivalDistance = 0.5f;
    [SerializeField]
    private float turnSpeed = 5.0f;
    [SerializeField]
    private float totalDistance;
    [SerializeField]
    private Transform centerPoint;

    [Space]
    [Header("Events")]
    public UnityEvent OnDestinationReached;

    [Space]
    [Header("Enemy Status")]
    public int healthPoints = 10;
    [SerializeField]
    EnemyType enemyType = EnemyType.None;

    private int waypointIndex = 0;
    private NavMeshAgent agent;
    private bool isPathEnded = false; 

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    private void Start()
    {
        waypoints = WaypointManager.Instance.GetWaypoints();

        CollectTotalDistanceToEndPoint();

        SetNextDestination();
    }

    private void Update()
    {
        if (isPathEnded)
        {
            return;
        }

        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            FaceToTarget(agent.steeringTarget);
        }

        if (!agent.pathPending && agent.remainingDistance <= arrivalDistance)
        {
            SetNextDestination();
        }
    }

    private void OnDestroy()
    {
        OnDestinationReached.RemoveAllListeners();
    }

    private void FaceToTarget(Vector3 newTarget)
    {
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0;

        if (directionToTarget.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }

    void SetNextDestination()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (waypointIndex >= waypoints.Length)
        {
            isPathEnded = true;
            agent.ResetPath();
            OnDestinationReached?.Invoke();
            return;
        }

        Vector3 targetPoint = waypoints[waypointIndex].position;

        if (waypointIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[waypointIndex].position, waypoints[waypointIndex - 1].position);
            totalDistance -= distance;
        }

        agent.SetDestination(targetPoint);
        waypointIndex++;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void CollectTotalDistanceToEndPoint()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            float distanceBetweenWaypoints = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance += distanceBetweenWaypoints;
        }
    }

    public float GetDistanceToEndPoint() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;
}
