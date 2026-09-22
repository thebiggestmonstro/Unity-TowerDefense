using System.Threading;
using UnityEngine;

public class SwarmParts : MonoBehaviour
{
    [Header("Visual Variants")]
    [SerializeField]
    private GameObject[] variants;

    [Space]
    [Header("Bounce Settings")]
    [SerializeField]
    private AnimationCurve bounceCurve;
    [SerializeField]
    private float bounceSpeed;
    [SerializeField]
    private float minHeight;
    [SerializeField]
    private float maxHeight;
    private float bounceTimer;

    private bool canBounce = true;

    private void Start()
    {
        ChooseVisualVariants();
    }

    private void Update()
    {
        if (!canBounce)
        {
            return;
        }

        DoBounce();
    }

    private void ChooseVisualVariants()
    {
        foreach (var option in variants)
        { 
            option.SetActive(false);
        }

        int randomIndex = Random.Range(0, variants.Length);
        GameObject newVisual = variants[randomIndex];

        newVisual.SetActive(true);
    }

    private void DoBounce()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;

        float bounceValue = bounceCurve.Evaluate(bounceTimer % 1);
        float bounceHeight = Mathf.Lerp(minHeight, maxHeight, bounceValue);

        transform.localPosition = new Vector3(transform.localPosition.x, bounceHeight, transform.localPosition.z);
    }

    public void StopBounce()
    {
        canBounce = false;
    }
}
