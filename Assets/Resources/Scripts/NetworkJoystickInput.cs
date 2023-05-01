using UnityEngine;
using Unity.Netcode;

public class NetworkJoystickInput : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private Movement _movement;

    private void Awake()
    {
        _spawner.PlayerSpawnEvent.AddListener(OnSpawn);
    }

    private void OnSpawn(NetworkObject player)
    {
        player.TryGetComponent(out _movement);
    }

    private void Update()
    {
        if (_movement != null && _joystick.Direction.magnitude > 0)
        {
            _movement.Move(AxisConverter.XYToXZ(_joystick.Direction));
        }
    }
}
