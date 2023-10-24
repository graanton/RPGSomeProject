using Unity.AI.Navigation;
using UnityEngine;

public class TileSurfaceBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _tileSurface;

    public void Bake()
    {
        _tileSurface.BuildNavMesh();
    }
}
