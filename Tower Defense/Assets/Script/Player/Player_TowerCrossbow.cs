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
    private readonly RaycastHit[] rayHits = new RaycastHit[8];

    protected override void Awake()
    {
        base.Awake();

        EnableRotation(true);
        
        visualEffect = GetComponent<VisualEffect_CrossbowTower>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = GetDirectionToEnemy(gunPoint);

        int count = Physics.RaycastNonAlloc(gunPoint.position, directionToEnemy, rayHits, Mathf.Infinity, enemyLayerMask);
        RaycastHit hitInfo = default;
        float nearestDistance = float.MaxValue;
        bool isHit = false;

        for (int i = 0; i < Mathf.Min(count, rayHits.Length); i++)
        {
            Enemy_Base owner = rayHits[i].collider.GetComponentInParent<Enemy_Base>();

            if (owner != null && owner.IsStealthed && owner != currentEnemy)
            {
                continue;
            }

            if (rayHits[i].distance < nearestDistance)
            {
                nearestDistance = rayHits[i].distance;
                hitInfo = rayHits[i];
                isHit = true;
            }
        }

        if (isHit)
        {
            towerHead.forward = directionToEnemy;

            IDamageable damagedTarget;

            if (hitInfo.collider.TryGetComponent<TankParts>(out var enemyShield))
            {
                damagedTarget = enemyShield.GetComponent<IDamageable>();
            }
            else
            {
                hitInfo.transform.TryGetComponent<IDamageable>(out damagedTarget);
            }

            if (damagedTarget != null)
            {
                damagedTarget.TakeDamage(damageAmount);
                Enemy_Base enemyTarget = damagedTarget as Enemy_Base;

                visualEffect.CreateOnHitVFX(hitInfo.point);
                visualEffect.EnableVisualEffect(gunPoint.position, hitInfo.point, enemyTarget);
                visualEffect.PlayReloadVFX(attackCooldown);
                GameServices.Get<AudioManager>()?.PlayAttackSFX(attackAudioClip, true);
            }
        }
    }
}
