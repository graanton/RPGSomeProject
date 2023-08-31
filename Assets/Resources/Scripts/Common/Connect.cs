using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts.Common
{
    public struct Connect
    {
        public readonly IDictionary<Hallway, Vector2Int> HallwaysPositions;
        public readonly Room Room1;
        public readonly Room Room2;
        
        public Connect(IDictionary<Hallway, Vector2Int> hallways, Room room1, Room room2)
        {
            HallwaysPositions = hallways;
            Room1 = room1;
            Room2 = room2;
        }
    }
}