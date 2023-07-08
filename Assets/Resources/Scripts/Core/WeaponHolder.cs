using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField, Min(0)] private int _capacity = 2;
    [SerializeField] private Transform _activeWeaponTransform;
    [SerializeField] private Transform _holsterWeaponTransform;

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
        if (_activeWeapon != null)
        {
            DisableActiveWeapon();
        }
        var weapon = GetWeapon(slot);
        weapon.transform.parent = _activeWeaponTransform;
        EnableActiveWeapon();
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

    private void DisableActiveWeapon()
    {
        ActiveWeaponIsNullAsWarning();
        _activeWeapon.transform.parent = _holsterWeaponTransform;
        _activeWeapon.gameObject.SetActive(false);
    }

    private void EnableActiveWeapon()
    {
        ActiveWeaponIsNullAsWarning();
        _activeWeapon.transform.parent = _holsterWeaponTransform;
        _activeWeapon.gameObject.SetActive(true);
    }

    private bool ActiveWeaponIsNullAsWarning()
    {
        bool isNull = _activeWeapon == null;
        if (isNull)
        {
            Debug.LogWarning("Current weapon is null");
        }
        return isNull;
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
