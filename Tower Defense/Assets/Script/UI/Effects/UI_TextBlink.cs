using TMPro;
using UnityEngine;

public class UI_TextBlink : MonoBehaviour
{
    [SerializeField]
    private float alphaChangeSpeed;

    private float targetAlphaValue;
    private bool canBlink;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!canBlink)
        {
            return;
        }

        if (Mathf.Abs(textMeshPro.color.a - targetAlphaValue) > 0.1f)
        {
            float newAlpha = Mathf.Lerp(textMeshPro.color.a, targetAlphaValue, alphaChangeSpeed * Time.deltaTime);
            ChangeColorAlpha(newAlpha);
        }
        else
        {
            ChangeTargetAlpha();
        }
    }

    private void ChangeColorAlpha(float newAlpha)
    { 
        Color currentColor = textMeshPro.color;
        textMeshPro.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    private void ChangeTargetAlpha() => targetAlphaValue = (targetAlphaValue == 1) ? 0 : 1;

    public void EnableBlink(bool enable)
    {
        canBlink = enable;

        if (!canBlink)
        {
            ChangeColorAlpha(1);
        }
    }
}
