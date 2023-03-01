using UnityEngine;

public class CharacterStand : MonoBehaviour
{
    [SerializeField] private Vector3 _instantiateOffest;

    public void SetCharacter(CharacterData characterData)
    {
        Transform viewCharacteRender;

        if (characterData.IsUnlocked)
        {
            viewCharacteRender = characterData.UnlockCharacterRender;
        }
        else
        {
            viewCharacteRender = characterData.LockCharacterRender;
        }

        Instantiate(viewCharacteRender.gameObject,
            transform.position + _instantiateOffest, transform.rotation, transform);
    }


}
