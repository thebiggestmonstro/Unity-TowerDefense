using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player_TowerCrossbow))]
public class VisualEffect_CrossbowTower : MonoBehaviour
{
    [Header("Settings for Lazer VFX")]
    [SerializeField]
    private LineRenderer visualEffect;
    [SerializeField]
    private float visualEffectDuration = 0.1f;

    [Space]
    [Header("Setting for Glow VFX")]
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private float maxMaterialIntensity = 150;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    private float currentMaterialIntensity;
    private Material material;
    private Player_TowerCrossbow crossbowTower;

    private void Awake()
    {
        crossbowTower = GetComponent<Player_TowerCrossbow>();
        material = meshRenderer.material;
        StartCoroutine(CoChangeEmissionOfMaterial(1));
    }

    private void Update()
    {
        UpdateEmissionColor();
    }

    public void EnableVisualEffect(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(CoDisableVisualEffect(startPoint, endPoint));
    }

    private IEnumerator CoDisableVisualEffect(Vector3 startPoint, Vector3 endPoint)
    {
        crossbowTower.EnableRotation(false);
        visualEffect.enabled = true;

        visualEffect.SetPosition(0, startPoint);
        visualEffect.SetPosition(1, endPoint);

        yield return new WaitForSeconds(visualEffectDuration);

        visualEffect.enabled = false;
        crossbowTower.EnableRotation(true);
    }

    private IEnumerator CoChangeEmissionOfMaterial(float changeDuration)
    {
        float startChangeTime = Time.time;
        float startMaterialIntensity = 0;

        while (Time.time - startChangeTime < changeDuration)
        {
            float factor = (Time.time - startChangeTime) / changeDuration;
            currentMaterialIntensity = Mathf.Lerp(startMaterialIntensity, maxMaterialIntensity, factor);
            yield return null;
        }
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentMaterialIntensity / maxMaterialIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentMaterialIntensity);
        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadVFX(float duration)
    {
        StartCoroutine(CoChangeEmissionOfMaterial(duration / 2));
    }
}
