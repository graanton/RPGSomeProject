using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private LinearShooting _shooting;

    public void OnPointerDown(PointerEventData eventData)
    {
        _shooting.Shoot();
    }
}
