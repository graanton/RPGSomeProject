using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Door : Tile3dBase
{
    [SerializeField] private Room _room;
    [SerializeField] private IncomingAndOutgoingWatcher _targetWaiter;

    public UnityEvent openEvent = new();
    public UnityEvent closeEvent = new();

    private void Awake()
    {
        _room.neighboreAddEvent.AddListener(OnNeighboreAdded);
        _targetWaiter.enterEvent.AddListener(OnPlayerEnter);
        Close();
        BoundsInit();
    }

    private void OnPlayerEnter(PlayerHealth player)
    {
        Close();
    }

    private void OnNeighboreAdded(Room neighbore)
    {
        bool isMyNeighbore = neighbore.OnTheBuffer(localBounds)
             &&
            Vector2Int.Distance(localBounds.position,
            neighbore.localBounds.position) == 1;

        if (isMyNeighbore)
        {
            OpenClientRpc();
        }
    }

    public void Close()
    {
        closeEvent?.Invoke();
    }

    [ClientRpc]
    private void OpenClientRpc()
    {
        openEvent?.Invoke();
    }

    public void Open()
    {
        OpenClientRpc();
    }
}
