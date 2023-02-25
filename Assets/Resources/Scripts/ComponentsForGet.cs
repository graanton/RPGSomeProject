using Unity.Netcode;
using UnityEngine;

public class ComponentsForGet : NetworkBehaviour
{
    [SerializeField] private Component[] _components;

    public bool RequestComponent<T>(out T result) where T : Component
    {
        foreach (var component in _components)
        {
            if (component is T)
            {
                result = (T)component;
                return true;
            }
        }

        result = null;
        return false;
    }
}
