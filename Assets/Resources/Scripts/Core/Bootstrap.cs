using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Generator2D _generator;

    private void Start()
    {
        _generator.Generate();
    }
}
