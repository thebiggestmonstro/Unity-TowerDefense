using System.Collections;
using UnityEngine;

public class TankParts : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float currentShieldAmount;

    [Space]
    [Header("Impact Details")]
    [SerializeField]
    private Material shieldMateial;
    [SerializeField]
    private float defaultShieldGlowValue = 1.0f;
    [SerializeField]
    private float impactShieldGlowValue = 3.0f;
    [SerializeField]
    private float impactScaleMultiplier = 0.97f;
    [SerializeField]
    private float impactSpeed = .1f;
    [SerializeField]
    private float impactResetDuration = 0.5f;

    private string shieldFreshnelParams = "_FresnelPower";
    private Coroutine currentCo;
    private float defaultScale;

    private void Start()
    {
        defaultScale = transform.localScale.x;
    }

    public void SetupShield(float shieldAmount)
    { 
        currentShieldAmount = shieldAmount;
    }

    public void TakeDamage(int damage)
    {
        currentShieldAmount -= damage;
        ActivateShieldImpact();

        if (currentShieldAmount <= 0)
        {
            ClearAllCoroutine();
            gameObject.SetActive(false);
        }
    }

    public void ClearAllCoroutine()
    {
        StopAllCoroutines();
        currentCo = null;
    }

    private void ActivateShieldImpact()
    {
        if (currentCo != null)
        { 
            StopCoroutine(currentCo);
        }

        currentCo = StartCoroutine(CoShieldImpact());
    }

    private IEnumerator CoShieldImpact()
    {
        yield return StartCoroutine(CoChangeShield(impactShieldGlowValue, defaultScale * impactScaleMultiplier,impactSpeed));

        StartCoroutine(CoChangeShield(defaultShieldGlowValue, defaultScale, impactResetDuration));
    }

    private IEnumerator CoChangeShield(float tagetGlowValue, float targetScale, float duration)
    {
        float time = 0;
        float startGlowValue = shieldMateial.GetFloat(shieldFreshnelParams);
        Vector3 initialScale = transform.localScale;
        Vector3 newTargetScale = new Vector3(targetScale, targetScale, targetScale);

        while (time < duration)
        {
            float newGlowValue = Mathf.Lerp(startGlowValue, tagetGlowValue, time / duration);
            shieldMateial.SetFloat(shieldFreshnelParams, newGlowValue);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = newTargetScale;
        shieldMateial.SetFloat(shieldFreshnelParams, tagetGlowValue);
    }
}
