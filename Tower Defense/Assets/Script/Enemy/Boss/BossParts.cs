using UnityEngine;

public class BossParts : MonoBehaviour
{
    [Header("Details")]
    public float legMoveSpeed = 3f;

    [Space]
    [Header("Animations")]
    [SerializeField]
    private Transform bodyTransform;
    [SerializeField]
    private float bodyAnimationSpeed = 1.0f;
    [SerializeField]
    private float maxBodyHeight = 0.05f;

    [Space]
    [Header("Smoke VFX")]
    [SerializeField]
    private ParticleSystem[] smokeParticles;
    [SerializeField]
    private float smokeCooldown;

    private LegParts[] legs;
    private Vector3 startPosition;
    private float elapsedTime;
    private float smokeTimer;

    private void Start()
    {
        legs = GetComponentsInChildren<LegParts>();
        startPosition = bodyTransform.localPosition;
    }

    private void Update()
    {
        AnimateBody();

        foreach (var leg in legs)
        {
            leg.UpdateLeg();
        }

        ActivateSmokeVFX();
    }

    private void AnimateBody()
    {
        elapsedTime += Time.deltaTime * bodyAnimationSpeed;
        float sinValue = (Mathf.Sin(elapsedTime) + 1) / 2;
        float newY = Mathf.Lerp(0, maxBodyHeight, sinValue);

        bodyTransform.localPosition = startPosition + new Vector3(0, newY, 0);
    }

    private void ActivateSmokeVFX()
    {
        smokeTimer -= Time.deltaTime;

        if (smokeTimer < 0)
        {
            smokeTimer = smokeCooldown;

            foreach (var smokeVFX in smokeParticles)
            {
                smokeVFX.Play();
            }
        }
    }
}
