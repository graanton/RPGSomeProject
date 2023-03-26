using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkObject))]
public class NetworkJoystickInput : NetworkBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Movement _movement;

    public void SetTarget(NetworkObject target)
    {
        if (target.IsOwner && target.TryGetComponent(out _movement))
        {
            Debug.Log("Sucessful set");
        }
    }

    private void Update()
    {
        if (_movement != null && _joystick.Direction.magnitude > 0)
        {
            _movement.Move(AxisConverter.XYToXZ(_joystick.Direction));
        }
    }
}
