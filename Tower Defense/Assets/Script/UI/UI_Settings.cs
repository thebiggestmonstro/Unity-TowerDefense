using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [Header("Camera Controller")]
    [SerializeField]
    private CameraController camController;

    [Header("Audio Mixer")]
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private float mixerMultiplier = 25.0f;

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

    [Space]
    [Header("Sound Setting")]
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private string sfxVolume;
    [SerializeField]
    private TextMeshProUGUI sfxVolumeText;

    [Space]
    [Header("BGM Setting")]
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private string bgmVolume;
    [SerializeField]
    private TextMeshProUGUI bgmVolumeText;

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

    public void ChangeSfxSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxVolume, newValue);
        sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void ChangeBgmSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmVolume, newValue);
        bgmVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSensParams, keyboarsSensSlider.value);
        PlayerPrefs.SetFloat(mouseSensParams, mouseSensSlider.value);
        PlayerPrefs.SetFloat(sfxVolume, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmVolume, bgmSlider.value);
    }

    private void OnEnable()
    {
        keyboarsSensSlider.value = PlayerPrefs.GetFloat(keyboardSensParams, .5f);
        mouseSensSlider.value = PlayerPrefs.GetFloat(mouseSensParams, 5f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolume, .5f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmVolume, .5f);
    }
}
