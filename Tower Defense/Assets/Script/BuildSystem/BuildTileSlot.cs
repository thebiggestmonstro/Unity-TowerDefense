using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildTileSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private TileAnimator tileAnimator;
    private Vector3 defaultPosition;
    private bool bCanMove = true;
    private Coroutine currentMoveUpCoroutine;

    private void Awake()
    {
        defaultPosition = transform.position;
    }

    public void InitTileAnimator(TileAnimator tileAnim)
    { 
        tileAnimator = tileAnim;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        BuildManager.Instance.SetSelectedBuildTile(this);
        MoveTileUp();
        bCanMove = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            return;
        }

        if (!bCanMove)
        {
            return;
        }

        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!bCanMove)
        {
            return;
        }

        if (currentMoveUpCoroutine != null)
        {
            Invoke(nameof(MoveTileDown), TileAnimator.Instance.GetMovementDurtaion());
        }
        else
        {
            MoveTileDown();
        }
    }

    public void UnSelectTile()
    {
        MoveTileDown();
        bCanMove = true;
    }

    private void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, tileAnimator.GetBuildTileOffset(), 0);
        tileAnimator.MoveTile(transform, targetPosition);
        currentMoveUpCoroutine = tileAnimator.activeMoveTileCoroutines[transform];
    }

    private void MoveTileDown()
    {
        tileAnimator.MoveTile(transform, defaultPosition);
    }
}
