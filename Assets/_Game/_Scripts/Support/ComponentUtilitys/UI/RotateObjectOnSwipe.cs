namespace ComponentUtilitys.UI
{
    using UnityEngine;
using UnityEngine.EventSystems;

public class RotateObjectOnSwipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform topArea;
    public RectTransform midArea;
    public RectTransform downArea;
    public GameObject objectToRotate;

    private Vector2 _startPosition;

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(midArea, eventData.position))
        {
            Vector2 direction = eventData.position - _startPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            objectToRotate.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(midArea, eventData.position))
        {
            _startPosition = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optional: Reset rotation or perform other actions when drag ends
    }
}
}