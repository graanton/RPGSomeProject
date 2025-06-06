using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class SpawnedPlayersRegister : MonoBehaviour
{
    [SerializeField] private PlayerInstaller _spawner;

    public PlayerRegisterEvent registerEvent = new();
    public IReadOnlyCollection<NetworkObject> Players => _players;

    private List<NetworkObject> _players = new();

    private void Awake()
    {
        throw new NotImplementedException();
    }

    public void RegisterPlayer(NetworkObject player)
    {
        _players.Add(player);
        registerEvent?.Invoke(player);
    }
}

[Serializable]
public class PlayerRegisterEvent: UnityEvent<NetworkObject> { }