using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody), typeof(Rigidbody))]
public class PhysicsMovement : NetworkBehaviour, IGrountMovement
{
    [SerializeField] private MovementEvent _moveEvent;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    public MovementEvent MoveEvent => _moveEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 direction)
    {
        Vector3 offset = AxisConverter.XYToXZ(direction) * _speed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + offset);
    }
}
