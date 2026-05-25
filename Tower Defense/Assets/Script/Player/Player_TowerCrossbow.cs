using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_TowerCrossbow : Player_TowerBase
{
    void LateUpdate()
    {
        RotateTowerHeadToEnemy();
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
