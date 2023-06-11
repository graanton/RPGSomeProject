using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerSpawner _targetSpawner;

    private IAttack _attack;

    private void Awake()
    {
        _targetSpawner.PlayerSpawnEvent.AddListener(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(NetworkObject player)
    {
        _attack = player.GetComponent<IAttack>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _attack.StartAttacking();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _attack.StopAttacking();
    }
}
