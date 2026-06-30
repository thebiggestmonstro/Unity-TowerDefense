using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [Header("Camera Controller")]
    [SerializeField]
    private CameraController camController;

    [Space]
    [Header("Keyboard Sensitivity")]
    [SerializeField]
    private Slider keyboarsSensSlider;
    [SerializeField]
    private TextMeshProUGUI keyboarsSensText;
    [SerializeField]
    private string keyboardSensParams = "";
    [SerializeField]
    private float minKeyboardSens = 30;
    [SerializeField]
    private float maxKeyboardSens = 120;

    [Space]
    [Header("Mouse Sensitivity")]
    [SerializeField]
    private Slider mouseSensSlider;
    [SerializeField]
    private TextMeshProUGUI mouseSensText;
    [SerializeField]
    private string mouseSensParams = "";
    [SerializeField]
    private float minMousedSens = 0.01f;
    [SerializeField]
    private float maxMouseSens = 1f;

    public void ChageKeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minKeyboardSens, maxKeyboardSens, value);
        camController.AdjustKeyboardSensitivity(newSensitivity);
        keyboarsSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void ChageMouseSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minMousedSens, maxMouseSens, value);
        camController.AdjustMouseSensitivity(newSensitivity);
        mouseSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSensParams, keyboarsSensSlider.value);
        PlayerPrefs.SetFloat(mouseSensParams, mouseSensSlider.value);
    }

    private void OnEnable()
    {
        keyboarsSensSlider.value = PlayerPrefs.GetFloat(keyboardSensParams, .5f);
        mouseSensSlider.value = PlayerPrefs.GetFloat(mouseSensParams, 5f);
    }
}
