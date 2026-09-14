using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEffect : MonoBehaviour
{
    [Header("Menu View")]
    [SerializeField]
    private Vector3 inMenuPosition;
    [SerializeField]
    private Vector3 inMenuRotation;

    [Space]
    [Header("Game View")]
    [SerializeField]
    private Vector3 inGamePosition;
    [SerializeField]
    private Vector3 inGameRotation;

    [Space]
    [Header("Level Selection View")]
    [SerializeField]
    private Vector3 levelSelectionPosition;
    [SerializeField]
    private Vector3 levelSelectionRotation;

    [Space]
    [Header("Screenshake Settings")]
    [Range(0.01f, .5f)]
    [SerializeField] private float shakeMagnutide;
    [Range(0.1f, 3f)]
    [SerializeField] private float shakeDuration;

    [Space]
    [Header("Castle Focus Details")]
    [SerializeField] private float focusOnCastleDuration = 0.5f;
    [SerializeField] private float hightOffset = 3;
    [SerializeField] private float distanceToCastle = 7;

    [Header("Transition details")]
    [SerializeField] private float transitionDuration = 3;

    private CameraController camController;

    private void Awake()
    {
        camController = GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        GameEvents.OnLevelStarted += HandleLevelStarted;
        GameEvents.OnDamageTaken += HandleDamaged;
    }

    private void OnDisable()
    {
        GameEvents.OnLevelStarted -= HandleLevelStarted;
        GameEvents.OnDamageTaken -= HandleDamaged;
    }

    private void Start()
    {
        SwitchToMenuView();
    }

    public void SwitchToMenuView()
    {
        StopAllCoroutines();
        StartCoroutine(CoChangePositionAndRotation(inMenuPosition, Quaternion.Euler(inMenuRotation)));
        StartCoroutine(CoEnableCameraControllsAfter(transitionDuration + .1f));
    }

    public void SwitchToGameView()
    {
        StopAllCoroutines();
        StartCoroutine(CoChangePositionAndRotation(inGamePosition, Quaternion.Euler(inGameRotation)));
        StartCoroutine(CoEnableCameraControllsAfter(transitionDuration + .1f));
    }

    public void SwitchToLevelSelectionView()
    {
        StopAllCoroutines();
        StartCoroutine(CoChangePositionAndRotation(levelSelectionPosition, Quaternion.Euler(levelSelectionRotation)));
        StartCoroutine(CoEnableCameraControllsAfter(transitionDuration + .1f));
    }

    private void SwitchCameraFocusOnCastle()
    {
        Transform castle = GameServices.Get<UnitManager>().GetUnitByName<Player_Castle>("Player_Castle").transform;

        if (castle == null)
        {
            Debug.Log("There is no castle to focus on!");
            return;
        }

        Vector3 directionToCastle = (castle.position - transform.position).normalized;
        Vector3 targetPosition = castle.position - (directionToCastle * distanceToCastle);
        targetPosition.y = castle.position.y + hightOffset;

        Quaternion targetRotation = Quaternion.LookRotation(castle.position - targetPosition);

        StopAllCoroutines();
        StartCoroutine(CoChangePositionAndRotation(targetPosition, targetRotation, focusOnCastleDuration));
        StartCoroutine(CoEnableCameraControllsAfter(focusOnCastleDuration + .1f));
    }

    public void Screenshake(float newDuration, float newMagnitude)
    {
        StartCoroutine(CoScreenshakeFX(newDuration, newMagnitude));
    }

    private IEnumerator CoChangePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration = 3, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        camController.EnableCameraConrolls(false);

        float time = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        camController.SyncTargetPosition(targetPosition);
        camController.SyncCameraRotation(targetRotation);
    }

    private IEnumerator CoScreenshakeFX(float duration, float magnitude)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            camController.SetShakeOffset(new Vector3(x, y, 0));

            elapsed += Time.deltaTime;
            yield return null;
        }

        camController.SetShakeOffset(Vector3.zero);
    }

    private IEnumerator CoEnableCameraControllsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        camController.EnableCameraConrolls(true);
    }

    private void HandleLevelStarted() => SwitchToGameView();

    private void HandleDamaged() => SwitchCameraFocusOnCastle();
}