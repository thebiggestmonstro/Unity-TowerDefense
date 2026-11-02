using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile_Harpon : MonoBehaviour
{
    [SerializeField] 
    private Transform connectionPoint;

    private Player_TowerHarpon harponTower;
    private float speed;
    private Enemy_Base currentEnemy;
    private bool bIsAttached;

    public void SetupProjectile(Player_TowerHarpon inHarponTwoer, float inSpeed, Enemy_Base inEnmey)
    {
        harponTower = inHarponTwoer;
        speed = inSpeed;
        currentEnemy = inEnmey;
    }

    public Transform GetConnectionPoint()
    {
        if (connectionPoint == null)
        {
            return transform;
        }

        return connectionPoint;
    }

    private void Update()
    {
        if (currentEnemy == null || bIsAttached)
        {
            return;
        }

        MoveToEnmey();

        if (Vector3.Distance(transform.position, currentEnemy.transform.position) < 0.5f)
        {
            AttachToEnenmy();
        }
    }

    private void MoveToEnmey()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentEnemy.transform.position, speed * Time.deltaTime);
        transform.forward = currentEnemy.transform.position - transform.position;
    }

    private void AttachToEnenmy()
    {
        bIsAttached = true;
        transform.parent = currentEnemy.transform;
        harponTower.ActivateAttack();
    }
}
