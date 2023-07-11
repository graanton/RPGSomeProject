using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaterFromMove : MonoBehaviour
{
    [SerializeField] private Movement _movement;

    private void Awake()
    {
        _movement.MoveEvent.AddListener(OnMove);
    }

    private void OnMove(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
