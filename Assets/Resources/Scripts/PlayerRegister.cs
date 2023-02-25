using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRegister : MonoBehaviour
{
    public PlayerRegisterEvent registerEvent = new();
    public IReadOnlyCollection<ComponentsForGet> Players => _players;

    private List<ComponentsForGet> _players = new();

    public void RegisterPlayer(ComponentsForGet player)
    {
        _players.Add(player);
        registerEvent?.Invoke(player);
    }
}

[Serializable]
public class PlayerRegisterEvent: UnityEvent<ComponentsForGet> { }