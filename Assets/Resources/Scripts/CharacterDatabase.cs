using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    [SerializeField] private CharacterData[] _characters;

    public CharacterData[] Characters => _characters;
}
