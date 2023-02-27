using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private GameObject _characterPrefab, _unlockCharacterRender, _lockCharacterRender;
    [SerializeField] private bool _isOpened;

    public GameObject CharacterPrefab => _characterPrefab;
    public GameObject UnlockCharacterRender => _unlockCharacterRender;
    public GameObject LockCharacterRender => _unlockCharacterRender;
    public bool IsOpened => _isOpened;
}
