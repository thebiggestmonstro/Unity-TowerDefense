using System.Collections.Generic;
using UnityEngine;

public class Player_TowerFan : Player_TowerBase
{
    private readonly HashSet<Enemy_Base> enemiesToReveal = new HashSet<Enemy_Base>();

    public void AddEnemyToReveal(Enemy_Base inEnemy)
    {
        if (enemiesToReveal.Add(inEnemy))
        {
            inEnemy.AddReveal();
        }
    }

    public void RemoveEnemyToReveal(Enemy_Base inEnemy)
    {
        if (enemiesToReveal.Remove(inEnemy) && inEnemy != null)
        {
            inEnemy.RemoveReveal();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (Enemy_Base enemy in enemiesToReveal)
        {
            if (enemy != null)
            {
                enemy.RemoveReveal();
            }
        }

        enemiesToReveal.Clear();
    }
}
