using System.Collections.Generic;

namespace Resources.Scripts.Common
{
    public struct Connect
    {
        public readonly IEnumerable<Hallway> Hallways;
        public readonly Room Room1;
        public readonly Room Room2;
        
        public Connect(IEnumerable<Hallway> hallways, Room room1, Room room2)
        {
            Hallways = hallways;
            Room1 = room1;
            Room2 = room2;
        }
    }
}