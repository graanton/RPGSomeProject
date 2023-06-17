using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWeaponSetter : MonoBehaviour
{
    [SerializeField] private WeaponHolder _weaponHolder;
    [SerializeField] private Weapon[] _weaponsToSet;

    private void Start()
    {
        for (int i = 0; i < _weaponsToSet.Length; i++)
        {
            _weaponHolder.SetWeapon(_weaponsToSet[i], i);
        }
    }
}
