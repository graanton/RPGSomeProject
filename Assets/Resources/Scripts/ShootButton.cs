using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private PlayerSpawner _targetSpawner;

    private LinearShooting _shooting;

    private void Awake()
    {
        _targetSpawner.PlayerSpawnEvent.AddListener(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(NetworkObject player)
    {
        _shooting = player.GetComponent<LinearShooting>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _shooting.Shoot();
    }
}
