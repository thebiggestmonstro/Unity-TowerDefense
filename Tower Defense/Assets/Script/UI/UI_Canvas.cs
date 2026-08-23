using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private UI_Pause uiPause;
    private UI_Animator uiAnimator;

    public UI_BuildBtns uiBuildBtns { get; private set; }

    private void Awake()
    {
        UIManager.RegisterUI<UI_Canvas>(gameObject.name, this);

        uiMainMenu = GetComponentInChildren<UI_MainMenu>(true);
        uiSettings = GetComponentInChildren<UI_Settings>(true);
        uiInGame = GetComponentInChildren<UI_InGame>(true);
        uiPause = GetComponentInChildren<UI_Pause>(true);
        uiBuildBtns = GetComponentInChildren<UI_BuildBtns>(true);
        uiAnimator = GetComponent<UI_Animator>();

        ActivateUIFade(true);

        // Not every scene has every UI type (e.g. the main menu scene has no
        // in-game/pause UI, stage scenes have no main menu/settings UI), so
        // only switch to what this scene actually contains.
        if (uiInGame != null)
        {
            SwitchUI(uiInGame.gameObject);
        }
        else if (uiMainMenu != null)
        {
            SwitchUI(uiMainMenu.gameObject);
        }
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

    public void ToggleGamePause()
    {
        if (uiPause == null || uiInGame == null)
        {
            return;
        }

        SwitchUI(uiPause.gameObject.activeSelf ? uiInGame.gameObject : uiPause.gameObject);
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

    public UI_Animator GetUIAnimator() => uiAnimator;
}
