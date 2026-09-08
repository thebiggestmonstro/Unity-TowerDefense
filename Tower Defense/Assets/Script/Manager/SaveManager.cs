using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameServices.Register(this);
    }

    private void OnDisable()
    {
        GameServices.Unregister(this);
    }

    public void UnlockLevel(string levelSceneName)
    {
        if (string.IsNullOrEmpty(levelSceneName))
        {
            return;
        }

        PlayerPrefs.SetInt(levelSceneName + "Unlocked", 1);
    }

    public bool IsLevelUnlocked(string levelSceneName) => PlayerPrefs.GetInt(levelSceneName + "Unlocked", 0) == 1;
}
