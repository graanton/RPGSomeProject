using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMovement : MonoBehaviour, IMoveble
{
    [SerializeField] private float _speed = 3;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 offset = direction.normalized * _speed * Time.deltaTime;
        Vector3 newPosition = _rigidbody.position + offset;

        _rigidbody.MovePosition(newPosition);
    }
}
