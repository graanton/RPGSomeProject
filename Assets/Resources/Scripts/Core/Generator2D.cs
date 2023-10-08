using System.Collections.Generic;
using System.Linq;
using Graphs;
using Resources.Scripts.Common;
using UnityEngine;
using Random = System.Random;

//Origin https://github.com/vazgriz/DungeonGenerator/blob/master/Assets/Scripts2D/Generator2D.cs

public class Generator2D : MonoBehaviour
{
    private enum CellType
    {
        None,
        Room,
        Hallway
    }

    [SerializeField] private Vector2Int _size;
    [SerializeField] private int _maxRoomCount;
    [SerializeField] private int _minRoomCount;
    [SerializeField] private int _seed;
    [SerializeField] Pool<Room> _roomsPool;
    [SerializeField] private Hallway _hallwayPrefab;
    [SerializeField] private Hallway _hallwayBorderPrefab;
    [SerializeField] private Transform _root;
    [SerializeField] private List<Room> _precreatedRooms = new();
    [SerializeField] private List<Room> _garantedToPlaceRooms = new();
    [Space]
    [SerializeField] private bool _seedIsRandom;
    [SerializeField] private bool _runInStart;

    private Random _random;
    private Grid2D<CellType> _grid;
    private Dictionary<Room, Vector2Int> _roomsPositions = new();
    private Delaunay2D _delaunay;
    private HashSet<Prim.Edge> _selectedEdges;
    private List<Hallway> _hallways = new();
    
    private const int TRIES_COUNT = 10;
    private const int STEP_X = 2;
    private const int STEP_Y = 2;

    private void Start()
    {
        if (_runInStart)
        {
            Generate();
        }
    }

    private void OnValidate()
    {
        _random = new Random(_seed);
        _grid = new Grid2D<CellType>(_size);
        _roomsPositions = new Dictionary<Room, Vector2Int>();
        _hallways = new List<Hallway>();
    }

    public void Generate()
    {
        Clear();
        if (_seedIsRandom)
        {
            _seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            OnValidate();
        }
        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        CreateHallwaysBorder();
    }

    private int TotalUnrandomRooms() => _precreatedRooms.Count + _garantedToPlaceRooms.Count;
    
    private void PlaceRooms()
    {
        for (int i = 0; i < _maxRoomCount + TotalUnrandomRooms(); i++)
        {
            Room currentRoom;

            bool currentRoomIsPrecreated = i < _precreatedRooms.Count;
            bool currentRoomIsGarantedToPlace =
                !currentRoomIsPrecreated && i < _precreatedRooms.Count + _garantedToPlaceRooms.Count;

            if (currentRoomIsPrecreated)
            {
                currentRoom = _precreatedRooms[i];
            }
            else if (currentRoomIsGarantedToPlace)
            {
                currentRoom = _garantedToPlaceRooms[i - _precreatedRooms.Count];
            }
            else
            {
                currentRoom = _roomsPool.GetRandomWeightedObject().obj;
            }

            if (currentRoomIsPrecreated)
            {
                RegisterRoom(currentRoom, CalculateRoomRect(currentRoom).position);
            }
            else if (currentRoomIsGarantedToPlace)
            {
                GarantedRandomPlaceRoom(currentRoom);
            }
            else if (_roomsPositions.Count - TotalUnrandomRooms() - i >= _maxRoomCount - _minRoomCount)
            {
                GarantedRandomPlaceRoom(currentRoom);
            }
            else
            {
                Vector2Int location = new Vector2Int(_random.Next(0, _size.x), _random.Next(0, _size.y));
                if (CanPlaceRoom(new RectInt(location, currentRoom.Size)))
                {
                    PlaceRoom(currentRoom, location);
                }
            }
        }
    }

    private void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (Room room in _roomsPositions.Keys)
        {
            Vector2Int roomPostion = _roomsPositions[room];
            vertices.Add(new Vertex<Room>(roomPostion + ((Vector2)room.Size) / 2, room));
        }

        _delaunay = Delaunay2D.Triangulate(vertices);
    }

    private void CreateHallways()
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in _delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        _selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(_selectedEdges);

        foreach (var edge in remainingEdges)
        {
            _selectedEdges.Add(edge);
        }
    }

    private void CreateHallwaysBorder()
    {
        HashSet<Vector2Int> borderPoints = new();

        for (int x = 1; x < _grid.Size.x - 1; x++)
        {
            for (int y = 1; y < _grid.Size.y - 1; y++)
            {
                if (_grid[new Vector2Int(x, y)] == CellType.Hallway)
                {
                    foreach (Vector2Int point in 
                        new RectInt(x - 1, y - 1, 3, 3).allPositionsWithin)
                    {
                        borderPoints.Add(point);
                    }
                }
            }
        }

        foreach (var point in borderPoints)
        {
            if (_grid[point] == CellType.None)
            {
                PlaceHallway(_hallwayBorderPrefab, point);
            }
        }
    }

    private void PathfindHallways()
    {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(_size);

        foreach (var edge in _selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = GetRoomRect(startRoom).center;
            var endPosf = GetRoomRect(endRoom).center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
            {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);

                if (_grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (_grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5; //magic numbers
                }
                else if (_grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (_grid[current] == CellType.None)
                    {
                        _grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }
                
                Dictionary<Hallway, Vector2Int> hallwaysInConnect = new();
                foreach (var pos in path)
                {
                    if (_grid[pos] == CellType.Hallway)
                    {
                        Hallway placedHallway = PlaceHallway(_hallwayPrefab, pos);
                        hallwaysInConnect[placedHallway] = pos;
                    }
                }

                Connect connect = new Connect(hallwaysInConnect, startRoom, endRoom);
                startRoom.SetConnectionData(connect);
                endRoom.SetConnectionData(connect);
            }
        }
    }

    private Room PlaceRoom(Room room, Vector2Int position)
    {
        Vector3 placePosition = _root.position +
            _root.right * position.x + _root.forward * position.y;
        Room spawnedRoom = Instantiate(room, placePosition, _root.rotation, _root);
        
        RegisterRoom(spawnedRoom, position);

        return spawnedRoom;
    }

    private void RegisterRoom(Room room, Vector2Int position)
    {
        _roomsPositions[room] = position;

        foreach (var pos in GetRoomRect(room).allPositionsWithin)
        {
            _grid[pos] = CellType.Room;
        }
    }

    private Hallway PlaceHallway(Hallway hallway, Vector2Int position)
    {
        _grid[position] = CellType.Hallway;
        Vector3 placePosition = _root.position +
            _root.right * position.x + _root.forward * position.y;
        Hallway spawnedHallway = Instantiate(hallway, placePosition, _root.rotation, _root);
        
        _hallways.Add(spawnedHallway);
        
        return spawnedHallway;
    }

    public Room GarantedRandomPlaceRoom(Room room)
    {
        HashSet<Vector2Int> placePositions = new();
        Vector2Int findStep = new Vector2Int(STEP_X, STEP_Y);

        for (int tries = 0; tries < TRIES_COUNT; tries++)
        {
            Vector2Int randomPoint = new Vector2Int(_random.Next(0, _grid.Size.x), _random.Next(0, _grid.Size.y));
            Vector2Int roomSize = room.Size;
            if (CanPlaceRoom(new RectInt(randomPoint, roomSize)))
            {
                placePositions.Add(randomPoint);
            }
        }

        bool unlucky = placePositions.Count == 0;
        if (unlucky)
        {
            for (int x = 0; x < _grid.Size.x; x += findStep.x)
            {
                for (int y = 0; x < _grid.Size.y; y += findStep.y)
                {
                    if (!CanPlaceRoom(new RectInt(x, y, room.Size.x, room.Size.y))) { continue; }
                    placePositions.Add(new Vector2Int(x, y));
                }
            }
        }

        Vector2Int placePosition = placePositions.ElementAt(_random.Next(placePositions.Count - 1));
        placePositions.Clear();
        Room placedRoom = PlaceRoom(room, placePosition);

        return placedRoom;
    }

    private bool CanPlaceRoom(RectInt roomData)
    {
        bool includedToGrid = roomData.xMax <= _grid.Size.x &&
            roomData.yMax <= _grid.Size.y &&
            roomData.xMin >= 0 && roomData.yMin >= 0;

        if (!includedToGrid)
        {
            return false;
        }

        foreach (Vector2Int point in roomData.allPositionsWithin)
        {
            if (_grid[point] != CellType.None)
            {
                return false;
            }
        }

        return true;
    }

    private void Clear()
    {
        List<Room> roomsToDestroy = new List<Room>(_roomsPositions.Keys);
        foreach (Room precreatedRoom in _precreatedRooms)
        {
            roomsToDestroy.Remove(precreatedRoom);
        }
        List<Hallway> hallwaysToDestroy = _hallways;

        foreach (Room room in roomsToDestroy)
        {
            Destroy(room.gameObject);
        }
        foreach (Hallway hallway in hallwaysToDestroy)
        {
            Destroy(hallway.gameObject);
        }
    }

    private RectInt GetRoomRect(Room room)
    {
        if (_roomsPositions.TryGetValue(room, out Vector2Int roomPosition))
        {
            return new RectInt(roomPosition, room.Size);
        }
        throw new KeyNotFoundException();
    }

    private RectInt CalculateRoomRect(Room room)
    {
        Vector3 localPosition = room.transform.localPosition;
        Vector2Int roomPosition = AxisConverter.XZToXYInt(localPosition);
        return new RectInt(roomPosition, room.Size);
    }
}

