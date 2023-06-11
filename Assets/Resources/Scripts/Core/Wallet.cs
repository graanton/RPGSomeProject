using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    [SerializeField, Min(0)] private int _ammount;

    public IntEvent AddEvent = new();
    public IntEvent GrabEvent = new();

    public int Ammount => _ammount;

    public void GrabAll(Wallet self)
    {
        int grabValue = _ammount;
        self.Add(_ammount);
        _ammount = 0;
        GrabEvent?.Invoke(grabValue);
    }

    public void Add(int ammount)
    {
        _ammount += ammount;
        AddEvent?.Invoke(_ammount);
    }
}

public class IntEvent: UnityEvent<int> { }