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

    [Space]
    [Header("Setting for Front String")]
    [SerializeField]
    private LineRenderer frontString_L;
    [SerializeField]
    private LineRenderer frontString_R;
    [SerializeField]
    private Transform frontStartPoint_L;
    [SerializeField]
    private Transform frontStartPoint_R;
    [SerializeField]
    private Transform frontEndPoint_L;
    [SerializeField]
    private Transform frontEndPoint_R;

    [Space]
    [Header("Setting for Back String")]
    [SerializeField]
    private LineRenderer backString_L;
    [SerializeField]
    private LineRenderer backString_R;
    [SerializeField]
    private Transform backStartPoint_L;
    [SerializeField]
    private Transform backStartPoint_R;
    [SerializeField]
    private Transform backEndPoint_L;
    [SerializeField]
    private Transform backEndPoint_R;

    [Space]
    [Header("Setting for Rotor Effects")]
    [SerializeField]
    private Transform rotor;
    [SerializeField]
    private Transform unloadedRotor;
    [SerializeField]
    private Transform loadedRotor;

    private float currentMaterialIntensity;
    private Material material;
    private Player_TowerCrossbow crossbowTower;
    private LineRenderer[] stringLineRenderers;

    private void Awake()
    {
        crossbowTower = GetComponent<Player_TowerCrossbow>();
        material = meshRenderer.material;

        stringLineRenderers = new LineRenderer[]
        {
            frontString_L,
            frontString_R,
            backString_L,
            backString_R
        };
        UpdateStringMaterials();

        StartCoroutine(CoChangeEmissionOfMaterial(1));
    }

    private void Update()
    {
        UpdateEmissionColor();

        UpdateStringsEffect(frontString_L, frontStartPoint_L, frontEndPoint_L);
        UpdateStringsEffect(frontString_R, frontStartPoint_R, frontEndPoint_R);

        UpdateStringsEffect(backString_L, backStartPoint_L, backEndPoint_L);
        UpdateStringsEffect(backString_R, backStartPoint_R, backEndPoint_R);
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

    private void UpdateStringsEffect(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public void PlayReloadVFX(float duration)
    {
        float halfDuration = duration / 2;

        StartCoroutine(CoChangeEmissionOfMaterial(halfDuration));
        StartCoroutine(CoLoadRotor(halfDuration));
    }

    private IEnumerator CoLoadRotor(float duration)
    { 
        float startLoadTime = Time.time;

        while (Time.time - startLoadTime < duration)
        { 
            float factor = (Time.time - startLoadTime) / duration;
            rotor.position = Vector3.Lerp(unloadedRotor.position, loadedRotor.position, factor);
            yield return null;
        }

        rotor.position = loadedRotor.position;
    }

    private void UpdateStringMaterials()
    {
        foreach (LineRenderer lr in stringLineRenderers)
        {
            lr.material = material;
        }
    }
}
