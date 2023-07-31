using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Dude", menuName = "ScriptableObjects/Character data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private PlayerHealth _characterPrefab;
    [SerializeField] private Transform _unlockCharacterRender, _lockCharacterRender;
    [SerializeField] private bool _isUnlocked;

    public PlayerHealth CharacterPrefab => _characterPrefab;
    public Transform UnlockCharacterRender => _unlockCharacterRender;
    public Transform LockCharacterRender => _unlockCharacterRender;
    public bool IsUnlocked => _isUnlocked;
}
