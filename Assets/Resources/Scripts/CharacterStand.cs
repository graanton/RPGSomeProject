using UnityEngine;

public class CharacterStand : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private Vector3 _instantiateOffest;

    public CharacterData characterData => _characterData;

    private void Start()
    {
        if (characterData.IsOpened)
        {

        }

        Instantiate(characterData.UnlockCharacterRender,
            transform.position + _instantiateOffest, transform.rotation, transform);
    }


}
