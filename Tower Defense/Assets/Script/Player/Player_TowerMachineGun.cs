using UnityEngine;

public class Player_TowerMachineGun : Player_TowerBase
{
    [Header("MachineGun Setting")]
    [SerializeField]
    private Vector3 rotationOffset;
    [SerializeField]
    private Transform[] gunPointSet;
    [SerializeField] 
    private Pool_MachineGun projectilePool;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private VisulEffect_MachineGun machineGunVisualParts;

    private int gunPointIndex;

    protected override void Attack()
    {
        gunPoint = gunPointSet[gunPointIndex];
        Vector3 directionToEnemy = GetDirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity, enemyLayerMask))
        {
            IDamageable target = hitInfo.transform.GetComponentInParent<IDamageable>();

            if (target != null && projectilePool != null)
            {
                projectilePool.Fire(target,hitInfo.point,damage,projectileSpeed,gunPoint.position,gunPoint.rotation);
                machineGunVisualParts.PlayRecoliVFX(gunPoint);
                base.Attack();
                gunPointIndex = (gunPointIndex + 1) % gunPointSet.Length;
            }
        }
    }

    protected override void RotateTowerHeadToEnemy()
    {
        if (currentEnemy == null)
        {
            return;
        }

        Vector3 directionToEnemy = (currentEnemy.GetCenterPoint() - rotationOffset) - towerHead.position;
        Quaternion lookRot = Quaternion.LookRotation(directionToEnemy);

        Vector3 finalRotation = Quaternion.Lerp(towerHead.rotation, lookRot, rotationSpeed * Time.deltaTime).eulerAngles;
        towerHead.rotation = Quaternion.Euler(finalRotation);
    }
}
