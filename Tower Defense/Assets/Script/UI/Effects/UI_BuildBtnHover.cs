using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuildBtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float adjustSpeed = 10.0f;
    [SerializeField]
    private float showcaseY;
    [SerializeField]
    private float defaultY;

    private float targetY;
    private bool canMove;

    private void Update()
    {
        if (Mathf.Abs(transform.position.y - targetY) > 0.01f && canMove)
        {
            float newPositionY = Mathf.Lerp(transform.position.y, targetY, adjustSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }

    private void SetTargetY(float newY) => targetY = newY;

    public void ToggleCanMove(bool bCanMove)
    {
        canMove = bCanMove;
        SetTargetY(defaultY);

        if (!bCanMove)
        {
            SetDefaultPosition();
        }
    }

    private void SetDefaultPosition()
    {
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
    }

    public void OnPointerEnter(PointerEventData eventData) => SetTargetY(showcaseY);

    public void OnPointerExit(PointerEventData eventData) => SetTargetY(defaultY);
}