using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class InputChanger : MonoBehaviour
{
    [SerializeField] private Joystick _joystickInput;

    private Movement _movement;

    [Inject]
    private void Construct(PlayerHealth player)
    {
        _movement = player.GetComponent<Movement>();
    }

    private void Start()
    {
        ChangeInput();
    }

    private void ChangeInput()
    {
        IInput defaultInput;

        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:
                defaultInput = new UniversalInput();
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