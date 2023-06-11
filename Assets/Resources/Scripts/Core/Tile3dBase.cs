using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile3dBase : NetworkBehaviour, IBoundsCorrectble
{
    [SerializeField] private Vector2Int _boundsPosition;
    [SerializeField] private Tilemap _tilemap;

    public RectInt LocalBounds => new RectInt(_boundsPosition, Vector2Int.one);

    private void Awake()
    {
        BoundsInit();
    }

    public virtual void BoundsInit()
    {
        _boundsPosition = (Vector2Int)_tilemap.LocalToCell(transform.position);
    }

    public virtual void CorrectPosition()
    {
        transform.position = AxisConverter.XYToXZInt(LocalBounds.position);
    }

    public virtual void MoveBoundsPosition(Vector2Int newPosition)
    {
        _boundsPosition = newPosition;
    }

    public virtual void TilesRectToLocalZeroPosition()
    {
        foreach (Transform tile in _tilemap.transform)
        {
            tile.position = Vector3.zero;
        }
    }
}
