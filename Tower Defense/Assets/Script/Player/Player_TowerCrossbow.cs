using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_TowerCrossbow : Player_TowerBase
{
    [Header("Crossbow Tower Setting")]
    [SerializeField]
    private Transform gunPoint;

    private VisualEffect_CrossbowTower visualEffect;

    protected override void Awake()
    {
        base.Awake();

        visualEffect = GetComponent<VisualEffect_CrossbowTower>();
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = GetDirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy;

            Debug.Log(hitInfo.collider.gameObject.name + " was attacked!!!");

            visualEffect.EnableVisualEffect(gunPoint.position, hitInfo.point);
            visualEffect.PlayReloadVFX(attackCooldown);
        }
    }
}
