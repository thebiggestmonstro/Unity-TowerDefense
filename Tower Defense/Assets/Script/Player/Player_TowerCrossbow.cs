using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_TowerCrossbow : Player_TowerBase
{
    [Header("Crossbow Tower Setting")]
    [SerializeField]
    private Transform gunPoint;
    [SerializeField]
    private int damageAmount;

    private VisualEffect_CrossbowTower visualEffect;

    protected override void Awake()
    {
        base.Awake();

        EnableRotation(true);
        
        visualEffect = GetComponent<VisualEffect_CrossbowTower>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = GetDirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            if (hitInfo.transform.TryGetComponent<IDamageable>(out var damagedTarget))
            {
                damagedTarget.TakeDamage(damageAmount);
                Enemy_Base enemyTarget = damagedTarget as Enemy_Base;

                visualEffect.EnableVisualEffect(gunPoint.position, hitInfo.point, enemyTarget);
                visualEffect.PlayReloadVFX(attackCooldown);
            }
        }
    }
}
