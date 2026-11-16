using TMPro;
using UnityEngine;

public class UI_LevelCompleted : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountTxt;

    private void OnEnable()
    {
        EnemySpawner spawner = GameServices.Get<EnemySpawner>();

        if (spawner == null)
        {
            return;
        }

        killCountTxt.text = "Total Defeated Enemies : " + GameServices.Get<EnemySpawner>().GetTotalDefeatedCount();
    }

    public void NextLevelButtonClicked()
    {
        GameEvents.RaiseNextLevelStarted();
    }

    public void MainMenuButtonClicked()
    {
        GameEvents.RaiseReturnMainStage();
    }
}
