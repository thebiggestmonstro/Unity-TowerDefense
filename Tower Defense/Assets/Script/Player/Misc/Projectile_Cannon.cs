using UnityEngine;

public class Projectile_Cannon : MonoBehaviour
{
    [SerializeField]
    private float damageRadius;
    [SerializeField]
    private LayerMask enemyLayerMask;
    [SerializeField]
    private GameObject explosionVFX;

    private float damage;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetupPorjectile(Vector3 newVelocity, float inDamage)
    { 
        rb.linearVelocity = newVelocity;
        damage = inDamage;
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

    private void OnTriggerEnter(Collider other)
    {
        GiveDamageEnemiesAround();
        explosionVFX.SetActive(true);
        explosionVFX.transform.parent = null;
        Destroy(gameObject);
    }
}
