using System.Collections;
using UnityEngine;

public class Enemy_Boss : Enemy_Base
{
    [Header("EMP Attack")]
    [SerializeField]
    private GameObject empPrefab;
    [SerializeField]
    private LayerMask towerLayerMask;
    [SerializeField]
    private float towerCheckingRadius = 4.5f;
    [SerializeField]
    private float empSpawnCooldown = 8.0f;
    [SerializeField]
    private float empEffectDuration = 6.0f;
    [SerializeField]

    protected override bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return false;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.3f)
        {
            return true;
        }

        return false;
    }

    protected override void Die()
    {
        StopAllCoroutines();

        base.Die();
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();

        base.DestroyEnemy();
    }

    private void Start()
    {
        StartCoroutine(CoSpawnEMP());
    }

    private IEnumerator CoSpawnEMP()
    {
        WaitForSeconds wait = new WaitForSeconds(empSpawnCooldown);

        while (true)
        {
            SpawnEMP();
            yield return wait;
        }
    }

    private void SpawnEMP()
    {
        Transform target = FindRandomTower();

        if (target == null)
        {
            return;
        }

        Vector3 spawnPos = transform.position + new Vector3(0, 0.15f, 0);
        Vector3 dest = new Vector3(target.position.x, spawnPos.y, target.position.z);

        GameObject newEMP = Instantiate(empPrefab, spawnPos, Quaternion.identity);
        newEMP.GetComponent<EMPParts>().SetupEMP(empEffectDuration, dest);
    }

    private Transform FindRandomTower()
    {
        Collider[] towers = Physics.OverlapSphere(transform.position, towerCheckingRadius, towerLayerMask);

        if (towers.Length > 0)
        {
            return towers[Random.Range(0, towers.Length)].transform;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, towerCheckingRadius);
    }
}
