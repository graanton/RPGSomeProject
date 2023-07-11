using System;
using Unity.Netcode;
using UnityEngine;

public class InputChanger : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private Joystick _joystickInput;
    [SerializeField] private PlayerSpawner _playerSpawner;

    private void Awake()
    {
        _playerSpawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void OnSpawn(NetworkObject player)
    {
        _movement = player.GetComponent<Movement>();
        ChangeInput();
    }

    private void ChangeInput()
    {
        IInput defaultInput;

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:
                defaultInput = new KeyboardInput();
                break;
            case DeviceType.Handheld:
                defaultInput = _joystickInput;
                break;
            default:
                Debug.LogError("Unknoun device");
                return;
        }

        _movement.SetInput(defaultInput);
    }
}
