using UnityEngine;

public class UI_CreditBtn : MonoBehaviour
{
    [SerializeField]
    private string openUrl;

    public void OpenURL() => Application.OpenURL(openUrl);
}
