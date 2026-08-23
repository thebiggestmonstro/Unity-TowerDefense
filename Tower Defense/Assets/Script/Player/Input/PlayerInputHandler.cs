using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private BuildManager buildManager;
    [SerializeField]
    private UI_BuildBtns uiBuildBtns;
    [SerializeField]
    private UI_Canvas uiCanvas;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions.FindActionMap("Camera")?.Enable();
        playerInput.actions.FindActionMap("Build")?.Enable();
        playerInput.actions.FindActionMap("UI")?.Enable();
    }

    private void OnDisable()
    {
        playerInput.actions.FindActionMap("Camera")?.Disable();
        playerInput.actions.FindActionMap("Build")?.Disable();
        playerInput.actions.FindActionMap("UI")?.Disable();
    }

    #region Camera action map

    public void OnMove(InputAction.CallbackContext context)
    {
        cameraController.SetMoveInput(context.action.ReadValue<Vector2>());
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            cameraController.SetRotating(true);
        }
        else if (context.canceled)
        {
            cameraController.SetRotating(false);
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            cameraController.SetScrollValue(context.ReadValue<float>());
        }
        else if (context.canceled)
        {
            cameraController.SetScrollValue(0f);
        }
    }

    public void OnHoldClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            cameraController.BeginMiddleClickDrag();
        }

        if (context.performed)
        {
            cameraController.SetMiddleClickPressing(true);
        }

        if (context.canceled)
        {
            cameraController.SetMiddleClickPressing(false);
        }
    }

    #endregion

    #region Build action map

    public void OnCancelBuild(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        buildManager.CancleBuildUnit();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        buildManager.TryDeselectOnWorldClick();
    }

    public void OnHotkey1(InputAction.CallbackContext context) => HandleHotkey(context, 0);
    public void OnHotkey2(InputAction.CallbackContext context) => HandleHotkey(context, 1);
    public void OnHotkey3(InputAction.CallbackContext context) => HandleHotkey(context, 2);
    public void OnHotkey4(InputAction.CallbackContext context) => HandleHotkey(context, 3);
    public void OnHotkey5(InputAction.CallbackContext context) => HandleHotkey(context, 4);
    public void OnHotkey6(InputAction.CallbackContext context) => HandleHotkey(context, 5);
    public void OnHotkey7(InputAction.CallbackContext context) => HandleHotkey(context, 6);

    private void HandleHotkey(InputAction.CallbackContext context, int buttonIndex)
    {
        if (!context.performed)
        {
            return;
        }

        uiBuildBtns.SelectHotkeyButton(buttonIndex);
    }

    public void OnConfirmBuild(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        uiBuildBtns.ConfirmBuild();
    }

    #endregion

    #region UI action map

    public void OnTogglePause(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        uiCanvas.ToggleGamePause();
    }

    #endregion
}
