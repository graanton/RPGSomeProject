using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineModeStarter : MonoBehaviour
{
    public void Load()
    {
        SampleScene.Load(false);
    }
}
