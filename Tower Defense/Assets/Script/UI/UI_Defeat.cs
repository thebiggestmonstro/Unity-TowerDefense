using UnityEngine;

public class UI_Defeat : MonoBehaviour
{
    public void RestartButtonClicked()
    {
        GameEvents.RaiseSceneRestarted();
    }

    public void MainMenuButtonClicked()
    {
        GameEvents.RaiseReturnMainStage();
    }
}
