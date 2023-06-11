using System;
using Unity.Netcode;
using UnityEngine;

public class JoystickAttack : MonoBehaviour
{
    [SerializeField] private PlayerSpawner _spawner;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private BreakRotatble _breakRotatble;

    private IAttack _attack;
    private bool _attackStoped = true;

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
                if (_breakRotatble.Rotate(_joystick.Direction))
                {
                    _attackStoped = false;
                    _attack.StartAttacking();
                }
            }
            else if (_attackStoped == false)
            {
                _attackStoped = true;
                _attack.StopAttacking();
            }
        }
    }

    private void OnSpawn(NetworkObject player)
    {
        _breakRotatble = player.GetComponent<BreakRotatble>();
        _attack = player.GetComponent<IAttack>();
    }
}
