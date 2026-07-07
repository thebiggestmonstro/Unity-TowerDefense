using UnityEngine;
using UnityEngine.InputSystem;

public class UI_BuildBtns : MonoBehaviour
{
    [SerializeField]
    private float yPosOffset;

    public bool isActive;
    private UI_Animator uiAnimator;
    private RectTransform rectTransform;
    private Vector3 buildBtnsBasePosition;

    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        buildBtnsBasePosition = rectTransform.anchoredPosition; 
    }

    private void Update()
    {
        if (Keyboard.current[Key.B].wasPressedThisFrame)
        {
            ShowBuildButtons();
        }
    }

    void ShowBuildButtons()
    {
        isActive = !isActive;
        float yOffset = isActive ? yPosOffset : -yPosOffset;
        Vector3 offset = new Vector3(0, yOffset);
        uiAnimator.ChangePosition(transform, buildBtnsBasePosition, offset);
    }
}
