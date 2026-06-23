using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] 
    private float moveSpeed = 10f;
    private Vector2 moveInput;

    [Space]
    [Header("Rotation Settings")]
    [SerializeField]
    private float rotateSpeed = 15f;
    [SerializeField]
    private Transform focusPoint;
    [SerializeField] 
    private float minPitch = 5f;
    [SerializeField] 
    private float maxPitch = 85f;
    [SerializeField]
    private float maxFocusPointDistance = 15f;

    private float yaw;
    private float pitch;
    private float currentDistance;
    private bool isRightButtonPressed;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        currentDistance = maxFocusPointDistance;
    }

    private void LateUpdate()
    {
        UpdateFocusPointFromScreen();
        HandleCameraRotation();
        HandleCameraMovement();
    }

#region Input Binding Function
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.action.ReadValue<Vector2>();
    }


    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            isRightButtonPressed = true;
        }
        else if (context.canceled)
        {
            isRightButtonPressed = false;
        }
    }
#endregion

#region Normal Function
    void HandleCameraMovement()
    {
        Vector3 localDir = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldDir = transform.TransformDirection(localDir);
        Vector3 finalDir = Vector3.ProjectOnPlane(worldDir, Vector3.up).normalized;

        Vector3 movement = finalDir * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        focusPoint.Translate(movement, Space.World);
    }

    void HandleCameraRotation()
    {
        if (isRightButtonPressed && Mouse.current != null)
        {
            float mouseX = Mouse.current.delta.x.ReadValue();
            float mouseY = Mouse.current.delta.y.ReadValue();

            yaw += mouseX * rotateSpeed * Time.deltaTime;
            pitch -= mouseY * rotateSpeed * Time.deltaTime; 
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = focusPoint.position - (targetRotation * Vector3.forward * currentDistance);

        transform.rotation = targetRotation;
        transform.position = targetPosition;
    }


    private void UpdateFocusPointFromScreen()
    {
        if (isRightButtonPressed)
        {
            return;
        }

        Vector3 targetPoint = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxFocusPointDistance)
            ? hit.point
            : transform.position + (transform.forward * maxFocusPointDistance);

        focusPoint.position = targetPoint;
        currentDistance = Vector3.Distance(transform.position, targetPoint);
    }
    #endregion
}