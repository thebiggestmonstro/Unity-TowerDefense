using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    [SerializeField]
    private Transform wayPoint;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.SetDestination(wayPoint.position);
    }
}
