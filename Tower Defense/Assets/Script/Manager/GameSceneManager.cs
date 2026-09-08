using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private string nextSceneOnClear;
    [SerializeField]
    private string mainScene;

    private void OnEnable()
    {
        GameServices.Register(this);
        GameEvents.OnStageCleared += HandleStageCleared;
        GameEvents.OnReturnMainScene += HandleReturnMainScene;
        GameEvents.OnSceneSelected += HandleSceneSelected;
        GameEvents.OnSceneRestarted += HandleRestartCurrentScene;
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
        GameEvents.OnStageCleared -= HandleStageCleared;
        GameEvents.OnReturnMainScene -= HandleReturnMainScene;
        GameEvents.OnSceneSelected -= HandleSceneSelected;
        GameEvents.OnSceneRestarted -= HandleRestartCurrentScene;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void HandleStageCleared()
    {
        if (string.IsNullOrEmpty(nextSceneOnClear))
        {
            UIManager.GetUI<UI_InGame>("UI_InGame").EnableVictoryUI(true);
            return;
        }

        GameServices.Get<SaveManager>()?.UnlockLevel(nextSceneOnClear);
        LoadScene(nextSceneOnClear);
    }

    public void HandleReturnMainScene()
    {
        if (string.IsNullOrEmpty(mainScene))
        {
            Debug.LogWarning($"{nameof(GameSceneManager)}: main scene was not set.");
            return;
        }

        LoadScene(mainScene);
    }

    public void HandleSceneSelected(string selectedSceneName)
    {
        if (string.IsNullOrEmpty(selectedSceneName))
        {
            Debug.LogWarning($"{nameof(GameSceneManager)}: selected scene was not set.");
            return;
        }

        LoadScene(selectedSceneName);
    }


    public void HandleRestartCurrentScene()
    {
        if (string.IsNullOrEmpty(SceneManager.GetActiveScene().name))
        {
            Debug.LogWarning($"{nameof(GameSceneManager)}: current scene was not set.");
            return;
        }

        LoadScene(SceneManager.GetActiveScene().name);
    }
}
