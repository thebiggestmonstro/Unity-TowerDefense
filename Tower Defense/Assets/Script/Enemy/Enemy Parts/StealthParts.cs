using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthParts : MonoBehaviour
{
    [Header("Stealth Settings")]
    [SerializeField]
    private LayerMask enemyLayerMask;
    [SerializeField]
    private Material stealthMaterial;
    [SerializeField]
    private float checkInterval = 0.2f;

    private float stealthRadius;
    private readonly Collider[] hits = new Collider[30];
    private readonly HashSet<Enemy_Base> stealthedEnemies = new HashSet<Enemy_Base>();
    private readonly HashSet<Enemy_Base> foundEnemies = new HashSet<Enemy_Base>();

    public void SetupStealth(float radius)
    {
        stealthRadius = radius;
    }

    private void OnEnable()
    {
        StartCoroutine(CoRefreshStealth());
    }

    private void OnDisable()
    {
        foreach (Enemy_Base enemy in stealthedEnemies)
        {
            if (enemy != null)
            {
                enemy.RemoveStealth();
            }
        }

        stealthedEnemies.Clear();
        ClearAllCoroutine();
    }

    public void ClearAllCoroutine()
    {
        StopAllCoroutines();
    }

    private IEnumerator CoRefreshStealth()
    {
        WaitForSeconds wait = new WaitForSeconds(checkInterval);

        while (true)
        {
            yield return wait;
            RefreshStealthTargets();
        }
    }

    private void RefreshStealthTargets()
    {
        foundEnemies.Clear();

        int count = Physics.OverlapSphereNonAlloc(transform.position, stealthRadius, hits, enemyLayerMask);
        int loopCount = Mathf.Min(count, hits.Length);

        for (int i = 0; i < loopCount; i++)
        {
            if (hits[i].TryGetComponent(out Enemy_Base enemy))
            {
                foundEnemies.Add(enemy);
            }
        }

        foreach (Enemy_Base enemy in foundEnemies)
        {
            if (stealthedEnemies.Add(enemy))
            {
                enemy.AddStealth(stealthMaterial);
            }
        }

        stealthedEnemies.RemoveWhere(enemy =>
        {
            if (foundEnemies.Contains(enemy))
            {
                return false;
            }

            if (enemy != null)
            {
                enemy.RemoveStealth();
            }

            return true;
        });
    }
}
