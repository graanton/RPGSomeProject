using UnityEngine;

public class PlayersTeleporterToRoom : MonoBehaviour
{
    [SerializeField] private PlayerRegister _playersRegister;

    private void Awake()
    {
        _playersRegister.registerEvent.AddListener(OnPlayerRigistered);
    }

    private void OnPlayerRigistered(ComponentsForGet components)
    {
        if (components.RequestComponent(out RoomMover roomMover))
        {
            
        }
    }
}
