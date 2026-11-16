using UnityEngine;

public class Player_TowerCannon : Player_TowerBase
{
    [Header("Cannone Settings")]
    [SerializeField]
    private float timeToRotateTarget = 1.5f;
    [SerializeField]
    private GameObject cannonProjectile;
    [SerializeField]
    private float cannonDamage;
    [SerializeField]
    private ParticleSystem cannonAttackVFX;

    private readonly Collider[] targetBuffer = new Collider[32];
    private readonly Collider[] nearbyBuffer = new Collider[32];

    protected override void Attack()
    {
        base.Attack();

        Vector3 velocity = GetLaunchVelocity();
        cannonAttackVFX.Play();

        GameObject newProjectile = Instantiate(cannonProjectile, gunPoint.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile_Cannon>().SetupPorjectile(velocity, cannonDamage);
    }

    private int GetEnemiesAroundTarget(Transform targetTransform)
    {
        return Physics.OverlapSphereNonAlloc(targetTransform.position, 1, nearbyBuffer, enemyLayerMask);
    }

    protected override Enemy_Base FindEnemyWithinRange()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, attackRange, targetBuffer, enemyLayerMask);
        Enemy_Base advancedTarget = null;
        int maxNearbyEnemies = 0;

        for (int i = 0; i < count; i++)
        {
            Collider enemy = targetBuffer[i];
            Enemy_Base enemeyToAttack = enemy.GetComponentInParent<Enemy_Base>();

            if (enemeyToAttack == null || enemeyToAttack.IsStealthed)
            {
                continue;
            }

            int amountofEnemies = GetEnemiesAroundTarget(enemy.transform);

            if (amountofEnemies > maxNearbyEnemies)
            {
                maxNearbyEnemies = amountofEnemies;
                advancedTarget = enemeyToAttack;
            }
        }

        return advancedTarget;
    }

    protected override void HandleTowerRotation()
    {
        if (currentEnemy == null)
        {
            return;
        }

        RotateCannonBody();
        RotateToLaunchDirection();
    }

    private void RotateCannonBody()
    {
        if (towerBody == null)
        {
            return;
        }

        Vector3 directionToEnemy = GetDirectionToEnemy(towerBody);
        directionToEnemy.y = 0;

        Quaternion loockRot = Quaternion.LookRotation(directionToEnemy);
        towerBody.rotation = Quaternion.Slerp(towerBody.rotation, loockRot, rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetLaunchVelocity()
    {
        Vector3 direction = currentEnemy.GetCenterPoint() - gunPoint.position;
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        Vector3 veclocityYXZ = directionXZ / timeToRotateTarget;

        float yVelocity = (direction.y - (Physics.gravity.y * Mathf.Pow(timeToRotateTarget, 2)) / 2) / timeToRotateTarget;
        Vector3 launchVelocity = veclocityYXZ + (Vector3.up * yVelocity);

        return launchVelocity;
    }

    void RotateToLaunchDirection()
    {
        Vector3 attackDirection = GetLaunchVelocity();
        Quaternion lookRot = Quaternion.LookRotation(attackDirection);

        Quaternion localLook = Quaternion.Inverse(towerHead.parent.rotation) * lookRot;
        float targetPitch = localLook.eulerAngles.x;

        Vector3 current = towerHead.localEulerAngles;
        float newPitch = Mathf.LerpAngle(current.x, targetPitch, rotationSpeed * Time.deltaTime);

        towerHead.localRotation = Quaternion.Euler(newPitch, 0, 0);
    }
}
