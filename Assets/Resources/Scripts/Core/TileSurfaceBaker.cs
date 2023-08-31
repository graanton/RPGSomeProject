using Unity.AI.Navigation;
using UnityEngine;

public class TileSurfaceBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _tileSurface;

    private void Start()
    {
        _tileSurface.BuildNavMesh();
    }
}
