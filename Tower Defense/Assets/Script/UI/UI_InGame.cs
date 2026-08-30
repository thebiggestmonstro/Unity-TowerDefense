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

    private void Awake()
    {
        UIManager.RegisterUI(gameObject.name, this);
        uiAnimator = GetComponentInParent<UI_Animator>();
        uiCanvas = GetComponentInParent<UI_Canvas>();
        waveTimerBasePosition = Txt_waveTime.transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnEnable()
    {
        GameEvents.OnHealthChanged += UpdateHealthPointsText;
        GameEvents.OnDamageTaken += ShakeHealthPointUI;
        GameEvents.OnCurrencyChanged += UpdateCurrencyText;
        GameEvents.OnCurrencyShortage += ShakeCurrencyUI;
        GameEvents.OnWaveTimerVisibilityChanged += EnableWaveTimerText;
        GameEvents.OnWaveTimerUpdated += UpdateWaveTimerText;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= UpdateHealthPointsText;
        GameEvents.OnDamageTaken -= ShakeHealthPointUI;
        GameEvents.OnCurrencyChanged -= UpdateCurrencyText;
        GameEvents.OnCurrencyShortage -= ShakeCurrencyUI;
        GameEvents.OnWaveTimerVisibilityChanged -= EnableWaveTimerText;
        GameEvents.OnWaveTimerUpdated -= UpdateWaveTimerText;
    }

    private void OnDestroy()
    {
        UIManager.UnregisterUI<UI_InGame>(gameObject.name);
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
        GameServices.Get<WaveManager>().ForceStartNextWave();
    }

    public void ShakeCurrencyUI()
    {
        uiCanvas.GetUIAnimator().ShakeUI(Txt_currency.transform.parent);
    }

    public void ShakeHealthPointUI()
    {
        uiCanvas.GetUIAnimator().ShakeUI(Txt_healhPoints.transform.parent);
    }
}