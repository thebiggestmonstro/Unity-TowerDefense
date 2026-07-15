using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] pauseUiElements;

    private UI_Canvas uiCanvas;
    private UI_InGame uiInGame;

    private void Awake()
    {
        UIManager.RegisterUI(gameObject.name, this);
        uiCanvas = GetComponentInParent<UI_Canvas>();
    }

    private void Start()
    {
        uiInGame = UIManager.GetUI<UI_InGame>("UI_InGame");
    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            uiCanvas.SwitchUI(uiInGame.gameObject);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void SwitchPauseUIElements(GameObject elementToEnable)
    {
        foreach (GameObject obj in pauseUiElements)
        {
            obj.SetActive(false);
        }

        elementToEnable.SetActive(true);
    }
}