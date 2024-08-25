using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class PointerEvent : UnityEvent<Vector2> { }

[System.Serializable]
public class DragEvent : UnityEvent<Vector2, Vector2, Vector2> { }

public class UISwipeHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // delegate
    public delegate void PointerAction(Vector2 position);
    public delegate void DragAction(Vector2 startPosition, Vector2 previousPosition, Vector2 currentPosition);

    [Header("Events")]
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onDrag;

    [Header("Events with param")]
    public PointerEvent onPointerDownWithParam;
    public PointerEvent onPointerUpWithParam;
    public DragEvent onDragWithParam;

    public PointerAction onPointerDownAction;
    public PointerAction onPointerUpAction;
    public DragAction onDragAction;

    private Vector2 _startPosition;
    private Vector2 _previousPosition;
    private NonInteractableZones _nonInteractableZones;

    private void Awake()
    {
        _nonInteractableZones = FindObjectOfType<NonInteractableZones>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_nonInteractableZones != null && _nonInteractableZones.IsPointerOverNonInteractableZone(eventData.position))
        {
            return;
        }

        _startPosition = eventData.position;
        _previousPosition = eventData.position;
        onPointerDown?.Invoke();
        onPointerDownWithParam?.Invoke(eventData.position);
        onPointerDownAction?.Invoke(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_nonInteractableZones != null && _nonInteractableZones.IsPointerOverNonInteractableZone(eventData.position))
        {
            return;
        }

        onPointerUp?.Invoke();
        onPointerUpWithParam?.Invoke(eventData.position);
        onPointerUpAction?.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_nonInteractableZones != null && _nonInteractableZones.IsPointerOverNonInteractableZone(eventData.position))
        {
            return;
        }

        Vector2 currentPosition = eventData.position;
        onDrag?.Invoke();
        onDragWithParam?.Invoke(_startPosition, _previousPosition, currentPosition);
        onDragAction?.Invoke(_startPosition, _previousPosition, currentPosition);
        _previousPosition = currentPosition;
    }

    public void SetOnPointerDownAction(PointerAction action)
    {
        onPointerDownAction = action;
    }

    public void SetOnPointerUpAction(PointerAction action)
    {
        onPointerUpAction = action;
    }

    public void SetOnDragAction(DragAction action)
    {
        onDragAction = action;
    }

    public void SetOnPointerDownAction(UnityAction action)
    {
        onPointerDown.AddListener(action);
    }

    public void SetOnPointerUpAction(UnityAction action)
    {
        onPointerUp.AddListener(action);
    }

    public void SetOnDragAction(UnityAction action)
    {
        onDrag.AddListener(action);
    }
}