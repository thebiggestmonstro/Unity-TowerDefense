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
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
        GameEvents.OnStageCleared -= HandleStageCleared;
        GameEvents.OnReturnMainScene -= HandleReturnMainScene;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void HandleStageCleared()
    {
        if (string.IsNullOrEmpty(nextSceneOnClear))
        {
            Debug.LogWarning($"{nameof(GameSceneManager)}: stage cleared but no Next Scene On Clear is set.");
            return;
        }

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
}
