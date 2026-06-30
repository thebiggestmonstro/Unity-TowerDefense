using UnityEditor;
using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] uiElements;

    private UI_Settings uiSettings;
    private UI_MainMenu uiMainMenu;

    private void Awake()
    {
        uiMainMenu = GetComponentInChildren<UI_MainMenu>(true);
        uiSettings = GetComponentInChildren<UI_Settings>(true);

        SwitchUI(uiSettings.gameObject);
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
}
