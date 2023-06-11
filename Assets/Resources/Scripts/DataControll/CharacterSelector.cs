using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharacterStand _standPrefab;
    [SerializeField] private CharacterDatabase _charactersData;
    [SerializeField] private Transform _root;
    [SerializeField] private Vector3 _spacing = Vector3.left * 2;
    [SerializeField] private float _changeSpeed = 3;
    [SerializeField] private Vector3 _pushForwardDirection = Vector3.forward;

    private HashSet<CharacterStand> _instantiatedStands = new();
    private int _selectedStandIndex = 0;

    public CharacterData SelectedCharacter => _charactersData.Characters[_selectedStandIndex];
    public int SelectedCharacterIndex => _selectedStandIndex;

    private void Start()
    {
        for(int i = 0; i < _charactersData.Characters.Length; i++)
        {
            CharacterStand stand = Instantiate(_standPrefab,
                _root.position + _spacing * i, _root.rotation, _root);

            stand.SetCharacter(_charactersData.Characters[i]);

            _instantiatedStands.Add(stand);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _instantiatedStands.Count; i++)
        {
            CharacterStand currentStand = _instantiatedStands.ElementAt(i);
            Vector3 currentStandDefaultPosition = _root.position + Quaternion.Euler(0, _root.rotation.eulerAngles.y, 0) * (_spacing * (i - _selectedStandIndex));
            Vector3 offset = Vector3.zero;

            if (_selectedStandIndex == i)
            {
                offset = _pushForwardDirection;
            }

            currentStand.transform.localPosition = Vector3.Slerp(currentStand.transform.localPosition,
                    currentStandDefaultPosition + offset,
                    _changeSpeed * Time.deltaTime);
        }
    }

    public void ChangeNext()
    {
        _selectedStandIndex++;
        if (_selectedStandIndex >= _charactersData.Characters.Length)
        {
            _selectedStandIndex = _charactersData.Characters.Length - 1;
        }
    }

    public void ChangeBefore()
    {
        _selectedStandIndex--;
        if (_selectedStandIndex < 0 )
        {
            _selectedStandIndex = 0;
        }
    }
}
