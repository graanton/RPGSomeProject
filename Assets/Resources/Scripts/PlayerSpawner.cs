using IJunior.TypedScenes;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : NetworkBehaviour, ISceneLoadHandler<int>
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _characterIndex;
    [SerializeField] private CharacterDatabase _characterDatabase;

    public NetworkSpawnEvent playerSpawnEvent = new();

    private void SpawnPlayer(ulong playerId, int characterIndex)
    {
        GameObject spawnedPlayer = Instantiate(
            _characterDatabase.Characters[characterIndex].CharacterPrefab,
            _spawnPoint.position, Quaternion.identity);

        NetworkObject networkOfPlayer = spawnedPlayer.GetComponent<NetworkObject>();

        networkOfPlayer.SpawnAsPlayerObject(playerId, true);

        ClientsSpawnInvokeClientRpc(networkOfPlayer.NetworkObjectId);
    }

    [ClientRpc]
    private void ClientsSpawnInvokeClientRpc(ulong objectId)
    {
        playerSpawnEvent?.Invoke(GetNetworkObject(objectId));
    }

    public void OnSceneLoaded(int characterIndex)
    {
        _characterIndex = characterIndex;
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, _characterIndex);
        else
        {
            SpawnPlayer(OwnerClientId, _characterIndex);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong ownerId, int characterIndex)
    {
        SpawnPlayer(ownerId, characterIndex);
    }
}

[Serializable]
public class NetworkSpawnEvent : UnityEvent<NetworkObject> { }