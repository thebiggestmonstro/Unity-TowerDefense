using System.Collections.Generic;
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
    private UI_BuildBtnHover[] buildButtonsEffects;
    private UI_BuildBtn[] buildButtons;
    private bool isBuildMenuActive;
    private List<UI_BuildBtn> unlockedButtons;
    private UI_BuildBtn lastSelectedButton;

    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        rectTransform = GetComponent<RectTransform>();

        buildBtnsBasePosition = rectTransform.anchoredPosition;
        buildButtonsEffects = GetComponentsInChildren<UI_BuildBtnHover>();
        buildButtons = GetComponentsInChildren<UI_BuildBtn>();
    }

    public void ShowBuildButtons(bool showButtons)
    {
        isBuildMenuActive = showButtons;
        float yOffset = isBuildMenuActive ? yPosOffset : -yPosOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0;

        uiAnimator.ChangePosition(transform, buildBtnsBasePosition, new Vector3(0, yOffset), openAnimationDuration);
        Invoke(nameof(ToggleButtonsMovement), methodDelay);
    }

    private void ToggleButtonsMovement()
    {
        foreach (var button in buildButtonsEffects)
        {
            button.ToggleCanMove(isBuildMenuActive);
        }
    }

    public UI_BuildBtn[] GetBuildButtons() => buildButtons;

    public void UpdateUnlockedButtons()
    {
        unlockedButtons = new List<UI_BuildBtn>();

        foreach (var button in buildButtons)
        {
            if (button.bButtonUnlocked)
            {
                unlockedButtons.Add(button);
            }
        }
    }

    public List<UI_BuildBtn> GetUnlockedButtons() => unlockedButtons;
    public UI_BuildBtn GetLastSelectedButton() => lastSelectedButton;
    public void SetLastSelected(UI_BuildBtn newLastSelected) => lastSelectedButton = newLastSelected;
}
