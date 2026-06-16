using NUnit.Framework;
using System.Collections.Generic;
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
    private List<Transform> myWaypoints;
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

    private int nextWaypointIndex = 0;
    private int currentWaypointIndex = 0;
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
        if (myWaypoints != null && myWaypoints.Count > 0 && totalDistance == 0)
        {
            InitializeEnemy();
        }
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

        if (ShouldChangeWaypoint())
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
        if (myWaypoints == null || myWaypoints.Count == 0)
        {
            return;
        }

        if (nextWaypointIndex >= myWaypoints.Count)
        {
            isPathEnded = true;
            agent.ResetPath();
            OnDestinationReached?.Invoke();
            return;
        }

        Vector3 targetPoint = myWaypoints[nextWaypointIndex].position;

        if (nextWaypointIndex > 0)
        {
            float distance = Vector3.Distance(myWaypoints[nextWaypointIndex].position, myWaypoints[nextWaypointIndex - 1].position);
            totalDistance -= distance;
        }

        agent.SetDestination(targetPoint);
        nextWaypointIndex++;
        currentWaypointIndex = nextWaypointIndex - 1;
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
        for (int i = 0; i < myWaypoints.Count - 1; i++)
        {
            float distanceBetweenWaypoints = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);
            totalDistance += distanceBetweenWaypoints;
        }
    }

    public float GetDistanceToEndPoint() => totalDistance + agent.remainingDistance;

    public Vector3 GetCenterPoint() => centerPoint.position;

    public EnemyType GetEnemyType() => enemyType;


    public void SetupEnemy(List<Waypoint> newWaypoints)
    {
        myWaypoints = new List<Transform>();

        foreach (var waypoint in newWaypoints)
        {
            if (waypoint != null)
            {
                myWaypoints.Add(waypoint.transform);
            }
        }

        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        nextWaypointIndex = 0;
        isPathEnded = false;
        totalDistance = 0f;

        CollectTotalDistanceToEndPoint();
        SetNextDestination();
    }

    private bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return false;
        }

        if (!agent.pathPending && agent.remainingDistance <= arrivalDistance)
        {
            return true;
        }

        Vector3 currentWaypoint = myWaypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWaypointIndex].position;

        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceBetweenPoints > distanceToNextWaypoint;
    }
}
