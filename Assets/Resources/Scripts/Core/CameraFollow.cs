using System;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private PlayerSpawner _spawner; 
    [SerializeField] private Transform _target;

    private void Awake()
    {
        _spawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void OnSpawn(NetworkObject player)
    {
        if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            _target = player.transform;
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
        }
    }
}
