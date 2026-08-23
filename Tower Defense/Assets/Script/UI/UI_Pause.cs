using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pauseUiElements;

    private void Awake()
    {
        UIManager.RegisterUI(gameObject.name, this);
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