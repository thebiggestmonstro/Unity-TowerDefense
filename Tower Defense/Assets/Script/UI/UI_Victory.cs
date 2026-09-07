using UnityEngine;

public class UI_Victory : MonoBehaviour
{
    public void MainMenuButtonClicked()
    {
        GameEvents.RaiseReturnMainStage();
    }
}
