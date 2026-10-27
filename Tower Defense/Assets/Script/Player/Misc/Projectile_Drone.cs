using System;
using UnityEngine;

public class Projectile_Drone : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField]
    private LayerMask enemyLayerMask;
    [SerializeField]
    private float enemyCheckRadius = 10.0f;
    [SerializeField]
    private float targetUpdateInterval = 0.5f;
    [SerializeField]
    private float moveSpeed = 5.0f;

    [Space]
    [Header("Damage Setting")]
    [SerializeField]
    private float damage;
    [SerializeField]
    private float damageRadius = 0.8f;
    [SerializeField]
    private float explosionDistance = 0.5f;
    [SerializeField]
    private GameObject explosionVFX; 

    private Rigidbody rb;
    private Transform currentTarget;
    private bool bIsLaunched;
    private Action<GameObject> returnToPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!bIsLaunched)
        {
            return;
        }

        if (currentTarget == null)
        {
            UpdateTarget();

            if (currentTarget == null)
            {
                return;
            }
        }

        Vector3 nextPosition = Vector3.MoveTowards(rb.position, currentTarget.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPosition);

        if (Vector3.Distance(transform.position, currentTarget.position) < explosionDistance)
        {
            Explode();
            return;
        }
    }

    public void SetReturnAction(Action<GameObject> inReturnAction)
    {
        returnToPool = inReturnAction;
    }

    public void SetupDrone()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        bIsLaunched = true;

        InvokeRepeating(nameof(UpdateTarget), 0.1f, targetUpdateInterval);
    }

    public void ResetDrone()
    {
        CancelInvoke();
        bIsLaunched = false;
        currentTarget = null;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void ReturnToPool()
    {
        returnToPool?.Invoke(gameObject);
    }

    private void UpdateTarget()
    {
        currentTarget = FindClosestEnemy();

        if (currentTarget != null)
        {
            rb.useGravity = false;
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, enemyCheckRadius, enemyLayerMask);
        Transform advancedEnemy = null;
        float shortestDistance = float.MaxValue;

        foreach (Collider enemy in enemiesAround)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                advancedEnemy = enemy.transform;
                shortestDistance = distance;
            }
        }

        return advancedEnemy;
    }

    private void Explode()
    {
        GiveDamageEnemiesAround();

        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            vfx.SetActive(true);
            Destroy(vfx, 2f);
        }

        ReturnToPool();
    }

    private void GiveDamageEnemiesAround()
    {
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, damageRadius, enemyLayerMask);

        foreach (Collider enemy in enemiesAround)
        {
            IDamageable damageable = enemy.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(Mathf.RoundToInt(damage));
            }
        }
    }
}
