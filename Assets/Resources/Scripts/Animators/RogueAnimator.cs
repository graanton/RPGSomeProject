using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;

    private const string _isRunningKey = "isRunning";
    
    private void Start()
    {
        _movement.MoveEvent += OnMove;
        _movement.StopedEvent += OnStop;
    }

    private void OnStop()
    {
        SetRunningState(false);
    }

    private void OnMove(Vector3 direction)
    {
        SetRunningState(true);
    }

    private void SetRunningState(bool state)
    {
        _animator.SetBool(_isRunningKey, state);
    }
}
