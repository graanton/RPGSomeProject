using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHitble
{
    public int Health { get; }
    public int MaxHealth { get; }
    public UnityEvent DeadEvent { get; }

    public void Hit(int damage);
}
