using System.Collections.Generic;
using UnityEngine;

public class Player_TowerHammer : Player_TowerBase
{
    [Header("Hammer Settings")]
    [SerializeField]
    private VisualEffect_Hammer hammerParts;
    [Range(0,1)]
    [SerializeField]
    private float slowMultiplier = 0.4f;
    [SerializeField]
    private float slowDuration = 2.0f;

    protected override void Update()
    {
        if (!bIsTowerActive)
        {
            return;
        }

        if (CanAttack())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();
        hammerParts.PlayerHammerAttackVFX();

        foreach (var enemy in GetValidTargets())
        {
            enemy.SetSpeed(slowMultiplier, slowDuration);
        }
    }

    protected override bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCooldown && IsAnyEnemyInRange();
    }

    private bool IsAnyEnemyInRange()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask);
        return enemies.Length > 0;
    }

    private List<Enemy_Base> GetValidTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayerMask);

        List<Enemy_Base> targets = new List<Enemy_Base>(colliders.Length);

        foreach (Collider col in colliders)
        {
            Enemy_Base target = col.GetComponentInParent<Enemy_Base>();

            if (target != null)
            {
                targets.Add(target);
            }
        }

        return targets;
    }
}
