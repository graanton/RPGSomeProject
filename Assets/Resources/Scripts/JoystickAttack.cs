using System;
using Unity.Netcode;
using UnityEngine;

public class JoystickAttack : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private BreakRotatble _breakRotatble;

    private void Awake()
    {
        _spawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void Update()
    {
        if (_breakRotatble != null)
        {
            if (_joystick.Direction.magnitude > 0)
            {
                _breakRotatble.Rotate(_joystick.Direction);
            }
        }
    }

    private void OnSpawn(NetworkObject player)
    {
        _breakRotatble = player.GetComponent<BreakRotatble>();
    }
}
