using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AttackVisualizer : NetworkBehaviour
{
    [SerializeField] private SwordAttack _attacker;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Projectile _visualize;

    private void Awake()
    {
        _attacker.AttackEvent.AddListener(OnAttack);
    }

    private void OnAttack()
    {
        if (IsServer)
        {
            var visualize = Instantiate(_visualize,
            _attackPoint.position, _attackPoint.rotation);
            visualize.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}
