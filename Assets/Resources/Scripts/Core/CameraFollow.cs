using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private PlayerInstaller _spawner; 

    private Transform _target;

    [Inject]
    private void Construct(PlayerHealth player)
    {
        _target = player.transform;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
}
