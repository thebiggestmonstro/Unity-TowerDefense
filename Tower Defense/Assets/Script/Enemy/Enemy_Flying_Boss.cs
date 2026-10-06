
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Flying_Boss : Enemy_Flying
{
    [Header("Boss Details")]
    [SerializeField]
    private GameObject unitToCreate;
    [SerializeField]
    private int createdUnitAmount = 150;
    [SerializeField]
    private float creationCooldown = 0.25f;

    private List<GameObject> createdEnemies = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(CoCreateUnits());
    }

    private IEnumerator CoCreateUnits()
    {
        yield return null;   

        WaitForSeconds wait = new WaitForSeconds(creationCooldown);

        while (createdUnitAmount > 0)
        {
            CreateNewEnemy();
            yield return wait;
        }
    }

    private void CreateNewEnemy()
    {
        createdUnitAmount--;

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * 0.5f;
        GameObject newUnit = Instantiate(unitToCreate, spawnPos, Quaternion.identity);
        Enemy_CreatedUnit createUnit = newUnit.GetComponent<Enemy_CreatedUnit>();
        createUnit.SetupEnemy(agent.destination, this, myWaypoints, nextWaypointIndex, totalDistance);

        foreach (Collider bossCol in GetComponentsInChildren<Collider>())
        {
            foreach (Collider unitCol in newUnit.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(unitCol, bossCol);
            }
        }

        createdEnemies.Add(newUnit);
    }

    public override void DestroyEnemy()
    {
        EliminateAllCreatedUnits();
        StopAllCoroutines();
        base.DestroyEnemy();
    }

    private void EliminateAllCreatedUnits()
    {
        foreach (GameObject unit in createdEnemies)
        {
            Destroy(unit);
        }
    }
}
