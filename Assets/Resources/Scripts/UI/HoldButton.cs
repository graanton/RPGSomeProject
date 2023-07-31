using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action PressedEvent;
    public event Action UnpressedEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        PressedEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UnpressedEvent?.Invoke();
    }
}
