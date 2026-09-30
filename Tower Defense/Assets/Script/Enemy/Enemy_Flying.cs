using UnityEngine;
using UnityEngine.AI;

public class Enemy_Flying : Enemy_Base
{
    [SerializeField]
    private float flightHeight = 3.0f;

    protected override void Awake()
    {
        base.Awake();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.baseOffset = flightHeight;
        agent.Warp(transform.position);
    }
}
