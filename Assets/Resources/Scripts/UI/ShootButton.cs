using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent PressedEvent;
    public UnityEvent UnpressedEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        PressedEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PressedEvent?.Invoke();
    }
}
