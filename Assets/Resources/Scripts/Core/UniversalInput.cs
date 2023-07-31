using System.Collections;
using UnityEngine;

public class UniversalInput : IInput
{
    public Vector2 GetDirection()
    {
        return new Vector2(Input.GetAxisRaw(Consts.HorizontalAxis), Input.GetAxisRaw(Consts.VerticalAxis)).normalized;
    }
}
