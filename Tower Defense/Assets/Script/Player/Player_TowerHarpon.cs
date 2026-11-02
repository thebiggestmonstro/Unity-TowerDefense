using System.Collections;
using UnityEngine;

public class Player_TowerHarpon : Player_TowerBase
{
    [Header("Harpon Details")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileDefaultPosition;
    [SerializeField]
    private float projectileLaunchSpeed = 15.0f;

    [Space]
    [Header("Damage Settings")]
    [SerializeField]
    private float initialDamage = 5.0f;
    [SerializeField]
    private float damagePerSecond = 5.0f;
    [SerializeField]
    private float attackDuration = 4.0f;
    [Range(0f, 1f)]
    [SerializeField]
    private float slowEffect = 0.7f;

    private Enemy_Base attackTarget;
    private Projectile_Harpon currentProjectile;
    private bool bIsAttacking;
    private Coroutine currentAttackCoroutine;
    private VisualEffect_Harpon harponVisualEffect;

    protected override void Awake()
    {
        base.Awake();
        harponVisualEffect = GetComponent<VisualEffect_Harpon>();
    }

    protected override void Start()
    {
        base.Start();

        CreateNewProjectile();
    }

    protected override void Update()
    {
        base.Update();

        if (bIsAttacking && attackTarget == null)
        {
            ResetAttack();
        }
    }

    protected override bool CanAttack()
    {
        return base.CanAttack() && !bIsAttacking;
    }

    protected override void Attack()
    {
        base.Attack();

        if (Physics.Raycast(gunPoint.position, gunPoint.forward, out RaycastHit hitInfo, Mathf.Infinity, enemyLayerMask))
        {
            Enemy_Base target = hitInfo.collider.GetComponentInParent<Enemy_Base>();

            if (target == null)
            {
                return;
            }

            attackTarget = target;
            bIsAttacking = true;
            currentProjectile.SetupProjectile(this, projectileLaunchSpeed, attackTarget);
            harponVisualEffect.EnableChainVisuals(true, currentProjectile.GetConnectionPoint());
        }
    }

    public void ActivateAttack()
    {
        if (attackTarget == null)
        {
            return;
        }

        attackTarget.SetSpeed(slowEffect, attackDuration);
        harponVisualEffect.CreateVFX(attackTarget.transform);
        attackTarget.TakeDamage(Mathf.RoundToInt(initialDamage));

        currentAttackCoroutine = StartCoroutine(CoGiveDamagePerTick());
    }

    private IEnumerator CoGiveDamagePerTick()
    {
        float time = 0;
        float tickInterval = 1.0f;
        int damagePerTick = Mathf.RoundToInt(damagePerSecond * tickInterval);

        while (time < attackDuration)
        {
            yield return new WaitForSeconds(tickInterval);
            time += tickInterval;

            if (attackTarget == null)
            {
                break;
            }

            attackTarget.TakeDamage(damagePerTick);
        }

        currentAttackCoroutine = null;
        ResetAttack();
    }

    private void ResetAttack()
    {
        if (currentAttackCoroutine != null)
        {
            StopCoroutine(currentAttackCoroutine);
            currentAttackCoroutine = null;
        }

        if (currentProjectile != null)
        {
            Destroy(currentProjectile.gameObject);
        }

        attackTarget = null;
        bIsAttacking = false;
        lastAttackTime = Time.time;
        harponVisualEffect.EnableChainVisuals(false);

        CreateNewProjectile();
    }

    private void CreateNewProjectile()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileDefaultPosition.position, projectileDefaultPosition.rotation, towerHead);
        currentProjectile = newProjectile.GetComponent<Projectile_Harpon>();
    }
}
