using UnityEngine;
using UnityEngine.InputSystem;

public class UI_BuildBtns : MonoBehaviour
{
    [SerializeField]
    private float yPosOffset;
    [SerializeField]
    private float openAnimationDuration = 0.1f;

    private UI_Animator uiAnimator;
    private RectTransform rectTransform;
    private Vector3 buildBtnsBasePosition;
    private UI_BuildBtnHover[] buildButtons;
    private bool isBuildMenuActive;

    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        buildBtnsBasePosition = rectTransform.anchoredPosition;
        buildButtons = GetComponentsInChildren<UI_BuildBtnHover>();
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
        isBuildMenuActive = !isBuildMenuActive;
        float yOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0;

        uiAnimator.ChangePosition(transform, buildBtnsBasePosition, new Vector3(0, yOffset), openAnimationDuration);
        Invoke(nameof(ToggleButtonsMovement), methodDelay);
    }

    private void ToggleButtonsMovement()
    {
        foreach (var button in buildButtons)
        {
            button.ToggleCanMove(isBuildMenuActive);
        }
    }
}
