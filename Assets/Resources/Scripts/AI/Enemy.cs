using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class Enemy : NetworkBehaviour, IDamageable
{
    [SerializeField] private UnityEvent _deadEvent = new();

    public PlayerEvent haveTargetEvent = new();

    public int Health => throw new System.NotImplementedException();
    public int MaxHealth => throw new System.NotImplementedException();
    public UnityEvent DeadEvent => _deadEvent;

    public void SetTargetDetecter(IncomingAndOutgoingWatcher detecter)
    {
        detecter.enterEvent.AddListener(TargetInvoke);
    }

    public void TakeDamage(int damage)
    {
        
    }

    private void TargetInvoke(PlayerHealth player) => haveTargetEvent?.Invoke(player);
}
