using System;
using System.Collections;
using System.Collections.Generic;
using Tests;
using TMPro;
using UnityEngine;

public class PolygonesCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private int _polygoneCount;

    public void AddCountFromGameObject(GameObject spawned)
    {
        _polygoneCount += spawned.GetComponent<MeshFilter>().mesh.triangles.Length / 3;
        print(_polygoneCount);
        _textMeshPro.text = $"{_polygoneCount}";
    }
}
