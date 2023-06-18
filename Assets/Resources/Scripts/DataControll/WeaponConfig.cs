using UnityEngine;

public class WeaponConfig: ScriptableObject
{
  [SerializeField] private Weapon _prefab;

  public Weapon WeaponPrefab => _prefab;
}
