using System;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    private IMoveble _movement;
    private IInput _input;

    public event Action<Vector3> MoveEvent;

    public void SetMovement(IMoveble movement)
    {
        _movement = movement;
    }
    
    public void SetInput(IInput input)
    {
        _input = input;
    }

    private void Update()
    {
        if (_movement != null && _input != null)
        {
            Vector3 direction = AxisConverter.XYToXZ(_input.GetDirection() * Time.deltaTime);
            if (direction != Vector3.zero)
            {
                _movement.Move(direction);
                MoveEvent?.Invoke(direction);
            }
        }
    }
}

public interface IMoveble
{
    void Move(Vector3 direction);
}

public interface IInput
{
    Vector2 GetDirection();
}
