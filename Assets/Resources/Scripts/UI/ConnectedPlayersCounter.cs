using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ConnectedPlayersCounter : MonoBehaviour
{
    [SerializeField] private SpawnedPlayersRegister _register;

    public CountEvent CountEvent = new CountEvent();

    private int _count;

    private void Awake()
    {
        _register.registerEvent.AddListener(OnRegister);
    }

    private void OnRegister(NetworkObject player)
    {
        _count++;
        CountEvent?.Invoke(_count);
    }
}

[Serializable]
public class CountEvent: UnityEvent<int> { }