using UnityEngine;

public class MovementChanger : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    [SerializeField] private PhysicsMovement _defaultMovement;

    private void Start()
    {
        _movement.SetMovement(_defaultMovement);
    }
}
