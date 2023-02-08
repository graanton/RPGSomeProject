using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomPlacer : MonoBehaviour
{
    [SerializeField] private Room _startRoom;
    [SerializeField] private Generator2D _generator;

    private Vector2Int _optimaizedStep => Vector2Int.one * 2;

    private void Awake()
    {
        _generator.GarantedRandomPlaceRoom(_startRoom, _optimaizedStep);
    }
}
