using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerRegister : MonoBehaviour
{
    public IReadOnlyCollection<NetworkObject> Players => _players;

    private List<NetworkObject> _players = new();

    public void RegisterPlayer(NetworkObject player)
    {
        _players.Add(player);
    }
}
