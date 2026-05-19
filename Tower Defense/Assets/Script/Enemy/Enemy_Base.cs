using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy_Base : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField] 
    private float arrivalDistance = 0.5f;
    [SerializeField]
    private float turnSpeed = 5.0f;

    [Header("Events")]
    public UnityEvent OnDestinationReached; 

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

        agent.SetDestination(waypoints[waypointIndex].position);
        waypointIndex++;
    }
}
