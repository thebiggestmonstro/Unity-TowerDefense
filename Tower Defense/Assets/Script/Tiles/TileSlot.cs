using UnityEngine;

public class TileSlot : MonoBehaviour
{
    public bool canBuild;

    public void ButtonClick()
    {
        canBuild = !canBuild;
        Debug.Log(gameObject.name + "`s Button worked");
    }
}
