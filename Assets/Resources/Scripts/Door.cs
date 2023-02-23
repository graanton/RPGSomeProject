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
    }

    private void Start()
    {
        Open();
    }

    private void OnPlayerEnter(PlayerHealth player)
    {
        Close();
    }

    private void OnNeighboreAdded(Room neighbore)
    {
        RectInt globalBounds = new RectInt(localBounds.position + _room.localBounds.position,
            localBounds.size);

        bool isMyNeighbore = neighbore.OnTheBuffer(globalBounds)
             &&
            Vector2Int.Distance(_room.localBounds.position + localBounds.position,
            neighbore.localBounds.position) == 1;

        if (isMyNeighbore)
        {
            Open();
        }
    }

    public void Close()
    {
        closeEvent?.Invoke();
    }

    public void Open()
    {
        openEvent?.Invoke();
    }
}
