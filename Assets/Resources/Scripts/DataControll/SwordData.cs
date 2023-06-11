using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Sword")]
public class SwordData : ScriptableObject
{
    [SerializeField, Min(1)] private int _damage = 50;

    public int Damage => _damage;
}
