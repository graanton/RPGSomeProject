using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class SworderAnimator : MonoBehaviour
{
    [SerializeField] private string _speedName = "Speed";
    [SerializeField] private Movement _movement;

    private Animator _animator;
    private bool _isMoved;

    private const float StopSpeedValue = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement.MoveEvent.AddListener(OnMove);
    }

    private void LateUpdate()
    {
        if (_isMoved)
        {
            _animator.SetFloat(_speedName, StopSpeedValue);
            _isMoved = false;
        }
    }

    private void OnMove(Vector3 direction)
    {
        _animator.SetFloat(_speedName, direction.magnitude);
        _isMoved = true;
    }
}
