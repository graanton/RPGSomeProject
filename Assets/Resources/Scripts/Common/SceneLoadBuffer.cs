using UnityEngine;
using IJunior.TypedScenes;

public class SceneLoadBuffer : MonoBehaviour, ISceneLoadHandler<bool>
{
    private bool _value;

    public bool Value => _value;

    public void OnSceneLoaded(bool argument)
    {
        _value = argument;
    }
}
