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

    private readonly Collider[] overlapBuffer = new Collider[32];

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

        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, overlapBuffer, enemyLayerMask);
        for (int i = 0; i < count; i++)
        {
            Enemy_Base target = overlapBuffer[i].GetComponentInParent<Enemy_Base>();
            
            if (target != null && !target.IsStealthed)
            {
                target.SetSpeed(slowMultiplier, slowDuration);
            }
        }
    }

    protected override bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCooldown && IsAnyEnemyInRange();
    }

    private bool IsAnyEnemyInRange() => Physics.OverlapSphereNonAlloc(transform.position, attackRange, overlapBuffer, enemyLayerMask) > 0;
}
