using System;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    public NetworkSpawnEvent playerSpawnEvent = new();

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public override void OnNetworkSpawn()
    {
        OnClientConnected(OwnerClientId);
    }

    private void OnClientConnected(ulong id)
    {
        if (IsHost)
        {
            SpawnPlayer(id);
        }
    }

    private void SpawnPlayer(ulong playerId)
    {
        NetworkObject spawnedPlayer = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
        spawnedPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerId, true);

        ClientsSpawnInvokeClientRpc(spawnedPlayer.NetworkObjectId);
    }

    [ClientRpc]
    private void ClientsSpawnInvokeClientRpc(ulong objectId)
    {
        playerSpawnEvent?.Invoke(GetNetworkObject(objectId));
    }
}

[Serializable]
public class NetworkSpawnEvent : UnityEvent<NetworkObject> { }