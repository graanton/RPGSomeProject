using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField, Min(0)] private int _capacity = 2;
    [SerializeField] private Transform _defaultWeaponTransform;

    private Weapon _activeWeapon;
    private Weapon[] _weapons = new Weapon[0];

    public WeaponEvent ChangeEvent = new();
    public WeaponEvent SetEvent = new();

    public Weapon ActiveWeapon => _activeWeapon;
        
    public void SetWeapon(Weapon weapon, int slot)
    {
        if (CheckForExceptionSlotValue(slot))
        {
            return;
        }
        _weapons[slot] = weapon;
        SetEvent?.Invoke(weapon);
        if (_activeWeapon == null)
        {
            ChangeWeaponSlot(slot);
        }
    }

    public void ChangeWeaponSlot(int slot)
    {
        if (CheckForExceptionSlotValue(slot))
        {
            return;
        }
        Destroy(_activeWeapon.gameObject);
        _activeWeapon = Instantiate(_weapons[slot], transform);
        ChangeEvent?.Invoke(_activeWeapon);
    }

    public Weapon GetWeapon(int slot)
    {
        return _weapons[slot];
    }

    private void OnValidate()
    {
        if (_weapons.Length != _capacity)
        {
            Weapon[] newWeaponsSize = new Weapon[_capacity];
            for (int i = 0; i < _weapons.Length && i < _capacity; i++)
            {
                newWeaponsSize[i] = _weapons[i];
            }
            _weapons = newWeaponsSize;
        }
    }

    private bool CheckForExceptionSlotValue(int slot)
    {
        if (slot >= _capacity)
        {
            Debug.LogError("Slot index out of capacity value");
            return true;
        }
        if (slot < 0)
        {
            Debug.LogError("Slot index below zero");
            return true;
        }
        if (_weapons[slot] == null)
        {
            Debug.LogWarning("Slot is void");
            return true;
        }
        if (_weapons[slot] == _activeWeapon)
        {
            Debug.LogWarning("It is the same");
            return true;
        }
        return false;
    }
}

[Serializable]
public class WeaponEvent: UnityEvent<Weapon> { }
