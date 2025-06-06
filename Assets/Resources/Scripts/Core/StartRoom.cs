using System;

public class StartRoom : LockedRoomBase
{
    private bool _isLocked = true;

    public override event Action OpenEvent;
    public override event Action LockEvent;

    public override bool IsLocked() => _isLocked;

    public override void Open()
    {
        if (_isLocked)
        {
            _isLocked = false;
            OpenEvent?.Invoke();
        }
        
    }
    
    public override void Lock()
    {
    }

    public override void Initialize()
    {
        Open();
    }
}
