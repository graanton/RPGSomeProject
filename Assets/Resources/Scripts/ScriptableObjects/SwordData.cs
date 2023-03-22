using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Sword")]
public class SwordData : ScriptableObject
{
    [SerializeField, Min(1)] private int _damage = 50;
    [SerializeField, Min(0)] private float _length, _width;

    public int Damage => _damage;
    public float Length => _length;
    public float Width => _width;
    
}
