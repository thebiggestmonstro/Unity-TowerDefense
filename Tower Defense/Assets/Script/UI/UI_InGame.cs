using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Txt_healhPoints;
    [SerializeField]
    private TextMeshProUGUI Txt_currency;
    [SerializeField]
    private TextMeshProUGUI Txt_waveTime;
    [SerializeField]
    private float waveTimerTxtOffset;
    [SerializeField]
    UI_TextBlink waveTimerTextBlinkEffect;

    private UI_Animator uiAnimator;
    private bool isWaveTimerVisible = false;
    private Vector3 waveTimerBasePosition;

    private UI_Canvas uiCanvas;
    private UI_Pause uiPause;

    private void Awake()
    {
        UIManager.RegisterUI(gameObject.name, this);
        uiAnimator = GetComponentInParent<UI_Animator>();
        uiCanvas = GetComponentInParent<UI_Canvas>();
        waveTimerBasePosition = Txt_waveTime.transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Start()
    {
        uiPause = UIManager.GetUI<UI_Pause>("UI_Pause");
    }

    private void OnDestroy()
    {
        UIManager.UnregisterUI<UI_InGame>(gameObject.name);
    }

    private void Update()
    {
        if (Keyboard.current[Key.F10].wasPressedThisFrame)
        {
            uiCanvas.SwitchUI(uiPause.gameObject);
        }
    }

    public void UpdateHealthPointsText(int value, int maxValue)
    {
        int newValue = maxValue - value;
        Txt_healhPoints.text = "Threat : " + newValue + "/" + maxValue;
    }

    public void UpdateCurrencyText(int value)
    {
        Txt_currency.text = "Resources : " + value;
    }

    public void UpdateWaveTimerText(float value)
    {
        Txt_waveTime.text = "Next Wave : " + value.ToString("00");
    }

    public void EnableWaveTimerText(bool enable)
    {
        if (isWaveTimerVisible == enable)
        {
            return;
        }
        isWaveTimerVisible = enable;

        Transform waveTimerTextTransform = Txt_waveTime.transform.parent;
        Vector3 offset = enable ? new Vector3(0, waveTimerTxtOffset, 0) : Vector3.zero;
        uiAnimator.ChangePosition(waveTimerTextTransform, waveTimerBasePosition, offset);
        waveTimerTextBlinkEffect.EnableBlink(enable);
    }

    public void ForceNextWave()
    {
        WaveManager.Instance.ForceStartNextWave();
    }
}