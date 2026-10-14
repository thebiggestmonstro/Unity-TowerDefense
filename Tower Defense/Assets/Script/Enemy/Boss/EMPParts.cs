using UnityEngine;

public class EMPParts : MonoBehaviour
{
    [SerializeField]
    private float empRadius = 2.0f;
    [SerializeField]
    private float empEffectDuration = 2.0f;
    [SerializeField]
    private float moveSpeed = 0.8f;
    [SerializeField]
    private GameObject empVFX;

    private Vector3 destination;
    private float shrinkSpeed = 3.0f;
    private bool bShouldShrink;
    private bool bArrived;
    private bool bTimerStarted;

    private void Update()
    {
        MoveToTarget();

        if (bShouldShrink)
        {
            Shrink();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Player_TowerBase tower = other.GetComponent<Player_TowerBase>();

        if (tower != null)
        {
            tower.DeactivateTower(empEffectDuration, empVFX);
            StartLifeTimer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, empRadius);
    }

    public void SetupEMP(float duration, Vector3 newTarget)
    {
        empEffectDuration = duration;
        destination = newTarget;
    }

    private void MoveToTarget()
    {
        if (bArrived)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            bArrived = true;
            StartLifeTimer();
        }
    }

    private void Shrink()
    {
        float newScale = Mathf.Max(0f, transform.localScale.x - shrinkSpeed * Time.deltaTime);
        transform.localScale = Vector3.one * newScale;

        if (newScale <= 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void DeactivateEMP() => bShouldShrink = true;

    private void StartLifeTimer()
    {
        if (bTimerStarted)
        {
            return;
        }

        bTimerStarted = true;

        float shrinkTime = transform.localScale.x / shrinkSpeed;
        Invoke(nameof(DeactivateEMP), Mathf.Max(0f, empEffectDuration - shrinkTime));
    }
}
