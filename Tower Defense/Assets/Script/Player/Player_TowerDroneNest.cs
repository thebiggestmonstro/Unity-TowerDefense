using System.Collections;
using UnityEngine;

public class Player_TowerDroneNest : Player_TowerBase
{
    [Header("Drone Nest Settings")]
    [SerializeField]
    private Pool_DroneNest dronePool;
    [SerializeField]
    private GameObject[] currentActivatingDrones;
    [Range(0, 1)]
    [SerializeField]
    private float attackTimeMultiplier = 0.4f;

    [Space]
    [SerializeField]
    private Transform[] droneSpawnerTransforms;
    [SerializeField]
    private Transform[] attachPoints;
    [SerializeField]
    private Transform[] attachPointRefs;

    private float reloadTimeMultiplier;
    private int currentDroneIndex = 0;

    protected override void Awake()
    {
        InitializeDrones();

        reloadTimeMultiplier = 1 - attackTimeMultiplier;
    }

    protected override void Update()
    {
        base.Update();

        UpdateAttachPoint();
    }

    protected override bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCooldown;
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(CoAttack());
    }

    private IEnumerator CoAttack()
    {
        Transform currentDrone = droneSpawnerTransforms[currentDroneIndex];
        Transform currentAttachPoint = attachPoints[currentDroneIndex];
        float attackTime = (attackCooldown / 4) * attackTimeMultiplier;
        float reloadTime = (attackCooldown / 4) * reloadTimeMultiplier;

        yield return CoChangeScale(currentDrone, 1, attackTime);
        currentActivatingDrones[currentDroneIndex].GetComponent<Projectile_Drone>().SetupDrone();

        yield return CoChangeScale(currentDrone, 0.1f, reloadTime);
        currentActivatingDrones[currentDroneIndex] = dronePool.GetDrone(currentAttachPoint);

        currentDroneIndex = (currentDroneIndex + 1) % attachPoints.Length;
    }

    private IEnumerator CoChangeScale(Transform inTransform, float newScale, float inDuration = 0.25f)
    {
        float time = 0;

        Vector3 initialScale = inTransform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, newScale, initialScale.z);

        while (time < inDuration)
        {
            inTransform.localScale = Vector3.Lerp(initialScale, targetScale, time / inDuration);
            time += Time.deltaTime;
            yield return null;
        }
         
        inTransform.localScale = targetScale;
    }

    private void InitializeDrones()
    {
        currentActivatingDrones = new GameObject[attachPoints.Length];

        for (int i = 0; i < currentActivatingDrones.Length; i++)
        {
            currentActivatingDrones[i] = dronePool.GetDrone(attachPoints[i]);
        }
    }

    private void UpdateAttachPoint()
    {
        for (int i = 0; i < attachPoints.Length; i++)
        {
            attachPoints[i].position = attachPointRefs[i].position;
        }
    }
}
