using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] uiElements;
    [SerializeField]
    private Image fadeImageUI;

    private UI_Settings uiSettings;
    private UI_MainMenu uiMainMenu;
    private UI_InGame uiInGame;
    private UI_Animator uiAnimator;

    private void Awake()
    {
        uiMainMenu = GetComponentInChildren<UI_MainMenu>(true);
        uiSettings = GetComponentInChildren<UI_Settings>(true);
        uiInGame = GetComponentInChildren<UI_InGame>(true);
        uiAnimator = GetComponent<UI_Animator>();

        ActivateUIFade(true);

        SwitchUI(uiSettings.gameObject);
        SwitchUI(uiInGame.gameObject);
        SwitchUI(uiMainMenu.gameObject);
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements)
        {
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }

        uiToEnable.SetActive(true);
    }

    public void QuitGame()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void ActivateUIFade(bool fadeIn)
    {
        if (fadeIn)
        {
            uiAnimator.FadeImage(fadeImageUI, 0, 2);
        }
        else
        {
            uiAnimator.FadeImage(fadeImageUI, 1, 2);
        }
    }
}
