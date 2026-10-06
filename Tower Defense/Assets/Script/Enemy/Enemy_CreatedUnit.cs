using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_CreatedUnit : Enemy_Swarm
{
    private Vector3 savedDestination;
    private Enemy_Flying_Boss unitCreator;

    public void SetupEnemy(Vector3 destination, Enemy_Flying_Boss myCreator, List<Transform> waypoints, int nextIndex, float distance)
    {
        unitCreator = myCreator;
        savedDestination = destination;
        myWaypoints = waypoints;
        nextWaypointIndex = nextIndex;
        currentWaypointIndex = nextIndex - 1;
        totalDistance = distance;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            return;
        }

        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            return;
        }

        rb.useGravity = false;
        rb.isKinematic = true;

        transform.position = hit.position;
        agent.enabled = true;
        agent.SetDestination(savedDestination);
    }
}
