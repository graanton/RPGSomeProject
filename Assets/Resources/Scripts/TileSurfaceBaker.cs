using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

public class TileSurfaceBaker : NetworkBehaviour
{
    [SerializeField] private NavMeshSurface _tileSurface;

    public override void OnNetworkSpawn()
    {
        BuildSurfaceClientRpc();
    }

    [ClientRpc]
    private void BuildSurfaceClientRpc()
    {
        _tileSurface.BuildNavMesh();
    }
}
