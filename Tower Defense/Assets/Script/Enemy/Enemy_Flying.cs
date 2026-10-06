using UnityEngine;
using UnityEngine.AI;

public class Enemy_Flying : Enemy_Base
{
    [SerializeField]
    private float flightHeight = 3.0f;

    protected override void Awake()
    {
        base.Awake();

        agent.baseOffset = flightHeight;
        if (agent.enabled)
        {
            agent.Warp(transform.position);
        }
    }
}
