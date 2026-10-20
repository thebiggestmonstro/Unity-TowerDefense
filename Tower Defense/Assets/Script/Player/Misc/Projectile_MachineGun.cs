using UnityEngine;

public class Projectile_MachineGun : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] 
    private float maxDistance = 30f;
    [SerializeField] 
    private float lifeTime = 3f;
    [SerializeField]
    private GameObject onHitVFX;

    private IDamageable target;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    private float damage;
    private float speed;
    private float elapsedTime;

    private Pool_MachineGun ownerPool;

    public void SetPool(Pool_MachineGun pool)
    {
        ownerPool = pool;
    }

    public void SetupProjectile(IDamageable inTarget, Vector3 inTargetPosition, float inDamage,float inSpeed)
    {
        target = inTarget;
        targetPosition = inTargetPosition;
        damage = inDamage;
        speed = inSpeed;

        startPosition = transform.position;
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed * Time.deltaTime);

        // Hit Target
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            if (target != null && (target as UnityEngine.Object) != null)
            {
                target.TakeDamage(Mathf.RoundToInt(damage));
            }

            PlayHitVFX();
            ReturnToPool();
            return;
        }

        // Hit Failed
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance || elapsedTime >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        target = null;

        if (ownerPool != null)
        {
            ownerPool.ReturnToPool(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void PlayHitVFX()
    {
        if (onHitVFX == null)
        {
            return;
        }

        GameObject vfx = Instantiate(onHitVFX, targetPosition, Quaternion.identity);
        Destroy(vfx, 1f);
    }
}
