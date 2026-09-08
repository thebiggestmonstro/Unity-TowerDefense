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

    private CameraController camController;

    private void Awake()
    {
        camController = GetComponent<CameraController>();
    }

    private void Start()
    {
        SwitchToMenuView();
    }

    public void SwitchToMenuView()
    {
        StopAllCoroutines();
        StartCoroutine(ChangePositionAndRotation(inMenuPosition, Quaternion.Euler(inMenuRotation)));
    }

    public void SwitchToGameView()
    {
        StopAllCoroutines();
        StartCoroutine(ChangePositionAndRotation(inGamePosition, Quaternion.Euler(inGameRotation)));
    }

    public void SwitchToLevelSelectionView()
    {
        StopAllCoroutines();
        StartCoroutine(ChangePositionAndRotation(levelSelectionPosition, Quaternion.Euler(levelSelectionRotation)));
    }

    public void Screenshake(float newDuration, float newMagnitude)
    {
        StartCoroutine(ScreenshakeFX(newDuration, newMagnitude));
    }


    private IEnumerator ChangePositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float duration = 3, float delay = 0)
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
        camController.EnableCameraConrolls(true);
    }

    private IEnumerator ScreenshakeFX(float duration, float magnitude)
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
}
