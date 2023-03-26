using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody), typeof(Rigidbody))]
public class PhysicsMovement : Movement
{
    [SerializeField] private MovementEvent _moveEvent;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    public override MovementEvent MoveEvent => _moveEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 direction)
    {
        Vector3 offset = direction * _speed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + offset);
        _moveEvent?.Invoke(offset);
    }
}
