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
    private bool isRightButtonPressed;

    [Space]
    [Header("Zoom Settings")]
    [SerializeField] 
    private float zoomSpeed = 20f;
    [SerializeField] 
    private float minZoom = 3f;
    [SerializeField] 
    private float maxZoom = 15f;
    private float scrollValue;
    private Vector3 targetZoomPosition; 
    private Vector3 zoomVelocity = Vector3.zero;
    private float smoothTime = 0.1f;

    [Space]
    [Header("Mouse Movement Settings")]
    [SerializeField] 
    private Vector3 levelCenterPoint;
    [SerializeField] 
    private float maxDistanceFromCenter;
    [SerializeField] 
    private float mouseMovementSpeed = 0.5f;
    private Vector3 mouseMovementVelocity = Vector3.zero;
    private Vector3 lastMousePosition;
    private bool isMiddleClickPressing = false;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        targetZoomPosition = transform.position;
    }

    private void LateUpdate()
    {
        HandleCameraZoom();
        HandleCameraRotation();
        HandleMouseMovement();
        HandleCameraMovement();
        UpdateFocusPointFromScreen();
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

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            scrollValue = context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            scrollValue = 0f;
        }
    }

    public void OnHoldClick(InputAction.CallbackContext context)
    {
        if (context.started && Mouse.current != null)
        {
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        if (context.performed)
        {
            isMiddleClickPressing = true;
        }

        if (context.canceled)
        {
            isMiddleClickPressing = false;
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
        targetZoomPosition += movement;
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
        transform.rotation = targetRotation;
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
    }

    void HandleCameraZoom()
    {
        if (Mathf.Abs(scrollValue) > 0.01f)
        {
            float scrollDirection = scrollValue > 0 ? 1f : -1f;

            Vector3 zoomDirection = transform.forward * scrollDirection * zoomSpeed;
            Vector3 nextTargetPosition = targetZoomPosition + zoomDirection * Time.deltaTime;

            if (targetZoomPosition.y < minZoom && scrollDirection > 0)
            {
                return;
            }
            if (targetZoomPosition.y > maxZoom && scrollDirection < 0)
            {
                return;
            }

            targetZoomPosition = nextTargetPosition;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetZoomPosition, ref zoomVelocity, smoothTime);
    }

    private void HandleMouseMovement()
    {
        if (isMiddleClickPressing && Mouse.current != null)
        {
            Vector3 positionDifference = (Vector3)Mouse.current.position.ReadValue() - lastMousePosition;
            Vector3 moveRight = transform.right * (-positionDifference.x) * mouseMovementSpeed * Time.deltaTime;
            Vector3 moveForward = transform.forward * (-positionDifference.y) * mouseMovementSpeed * Time.deltaTime;

            moveRight.y = 0;
            moveForward.y = 0;

            Vector3 movement = moveRight + moveForward;
            Vector3 targetPosition = transform.position + movement;

            if (Vector3.Distance(levelCenterPoint, targetPosition) > maxDistanceFromCenter)
            {
                targetPosition = levelCenterPoint + (targetPosition - levelCenterPoint).normalized * maxDistanceFromCenter;
                movement = targetPosition - transform.position; 
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref mouseMovementVelocity, smoothTime);
            targetZoomPosition += movement; 
            focusPoint.Translate(movement, Space.World); 

            lastMousePosition = Mouse.current.position.ReadValue();
        }
    }
    #endregion
}