using System.Collections;
using UnityEngine;

public class KeyboardInput : IInput
{
    public Vector2 GetDirection()
    {
        return new Vector2(Input.GetAxis(Consts.HorizontalAxis), Input.GetAxis(Consts.VerticalAxis)).normalized;
    }
}
