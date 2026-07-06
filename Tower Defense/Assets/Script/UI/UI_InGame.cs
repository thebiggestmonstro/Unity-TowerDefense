using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Txt_healhPoints;
    [SerializeField]
    private TextMeshProUGUI Txt_currency;
    [SerializeField]
    private TextMeshProUGUI Txt_waveTime;

    private void Awake()
    {
        UIManager.RegisterUI(gameObject.name, this);
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
        Txt_waveTime.transform.parent.gameObject.SetActive(enable);
    }

    public void ForceNextWave()
    {
        WaveManager.Instance.ForceStartNextWave();
    }
}
