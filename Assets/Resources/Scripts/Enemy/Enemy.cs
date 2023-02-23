using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class Enemy : NetworkBehaviour, IHitble
{
    [SerializeField] private UnityEvent _deadEvent = new();

    public PlayerEvent haveTargetEvent = new();

    public int Health => throw new System.NotImplementedException();
    public int MaxHealth => throw new System.NotImplementedException();
    public UnityEvent DeadEvent => _deadEvent;

    private IncomingAndOutgoingWatcher _playerDetecter;

    public override void OnNetworkSpawn()
    {
        _playerDetecter.enterEvent.AddListener(TargetInvoke);
    }

    public void SetTargetDetecter(IncomingAndOutgoingWatcher detecter)
    {
        _playerDetecter = detecter;
    }

    public void Hit(int damage)
    {
        
    }

    private void TargetInvoke(PlayerHealth player) => haveTargetEvent?.Invoke(player);
}
