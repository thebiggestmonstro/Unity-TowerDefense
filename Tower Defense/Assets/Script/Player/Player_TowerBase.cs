using System;
using UnityEditor.Rendering;
using UnityEngine;

public class Player_TowerBase : MonoBehaviour
{
    [Header("Unit Setting")]
    [SerializeField]
    protected float rotationSpeed = 5f;
    [SerializeField]
    protected float attackRange = 2.5f;
    [SerializeField]
    protected LayerMask enemyLayerMask;
    [SerializeField]
    protected float attackCooldown = 1;
    [SerializeField]
    protected Transform towerHead;

    protected float lastAttackTime;
    protected Transform currentEnemy = null;
    private readonly Collider[] enemiesToAttack = new Collider[10];

    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemy();
            return;
        }

        if (CanAttack())
        {
            Attack();
        }

        if (currentEnemy != null)
        {
            float enemyDistance = (currentEnemy.position - transform.position).sqrMagnitude;

            if (enemyDistance > attackRange * attackRange)
            {
                currentEnemy = null;
            }
        }
    }

    protected virtual Transform FindRandomEnemy()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, enemiesToAttack, enemyLayerMask);

        if (count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, count);
        return enemiesToAttack[randomIndex].transform;
    }

    protected bool CanAttack()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            return true;
        }

        return false;
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack performed at " + Time.time);
    }

    protected virtual void RotateTowerHeadToEnemy()
    {
        if (currentEnemy == null)
        {
            return;
        }

        Vector3 direction = currentEnemy.position - towerHead.position;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
