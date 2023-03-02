using System;
using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using IJunior.TypedScenes;

public class PlayerSpawner : NetworkBehaviour, ISceneLoadHandler<CharacterData>
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CharacterData _playerData;

    public NetworkSpawnEvent playerSpawnEvent = new();

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
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
        var spawnedPlayer = Instantiate(_playerData.CharacterPrefab, _spawnPoint.position, Quaternion.identity);
        spawnedPlayer.SpawnAsPlayerObject(playerId, true);

        ClientsSpawnInvokeClientRpc(spawnedPlayer.NetworkObjectId);
    }

    [ClientRpc]
    private void ClientsSpawnInvokeClientRpc(ulong objectId)
    {
        playerSpawnEvent?.Invoke(GetNetworkObject(objectId));
    }

    public void OnSceneLoaded(CharacterData data)
    {
        _playerData = data;
    }
}

[Serializable]
public class NetworkSpawnEvent : UnityEvent<NetworkObject> { }