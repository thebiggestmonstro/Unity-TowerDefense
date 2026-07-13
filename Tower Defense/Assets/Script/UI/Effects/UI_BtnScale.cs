using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BtnScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private float showcaseScale = 1.1f;
    [SerializeField]
    private float scaledDuration = 0.25f;
    [SerializeField]
    private UI_TextBlink btnTextBlinkEffect;

    private UI_Animator uiAnimator;
    private RectTransform rectTransform;

    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiAnimator.ChangeScale(rectTransform, showcaseScale, scaledDuration);

        if (btnTextBlinkEffect)
        {
            btnTextBlinkEffect.EnableBlink(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiAnimator.ChangeScale(rectTransform, 1, scaledDuration);

        if (btnTextBlinkEffect)
        {
            btnTextBlinkEffect.EnableBlink(true);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.localScale = Vector3.one;

        if (btnTextBlinkEffect)
        {
            btnTextBlinkEffect.EnableBlink(true);
        }
    }
}
