using UnityEngine;

public abstract class Interactble : MonoBehaviour
{
    [SerializeField] private Sprite _interactSprite;

    public Sprite InteractSprite => _interactSprite;

    public abstract void Interact();
    public abstract bool CanInteract();
}
