using Graphs;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

//Origin https://github.com/vazgriz/DungeonGenerator/blob/master/Assets/Scripts2D/Generator2D.cs

public class Generator2D : NetworkBehaviour
{
    private enum CellType
    {
        None,
        Room,
        Hallway
    }

    [SerializeField] private Vector2Int _size;
    [SerializeField] private int _roomCount;
    [SerializeField] private int _seed;
    [SerializeField] Pool<Room> _roomsPool;
    [SerializeField] private Hallway _hallwayPrefab;
    [SerializeField] private Hallway _hallwayBorderPrefab;
    [SerializeField] private Transform _startSpawnPoint;
    [SerializeField] private List<Room> _precreatedRoom;
    [SerializeField] private int _borderLength = 1;
    [Space]
    [SerializeField] private bool _seedIsRandom, _runInStart;

    private Random _random;
    private Grid2D<CellType> _grid;
    private List<Room> _rooms;
    private Delaunay2D _delaunay;
    private HashSet<Prim.Edge> _selectedEdges;

    private const int TRIES_COUNT = 10;

    public override void OnNetworkSpawn()
    {
        if (IsServer && _runInStart)
        {
            StartCoroutine(Generate());
        }
    }

    private void OnValidate()
    {
        _random = new Random(_seed);
        _grid = new Grid2D<CellType>(_size, Vector2Int.zero);
        _rooms = new List<Room>();
    }

    private IEnumerator Generate()
    {
        yield return null;  
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
        FillGrid();
    }

    private void PlaceRooms()
    {
        for (int i = 0; i < _roomCount + _precreatedRoom.Count; i++)
        {
            Room currentRoom;

            bool currentRoomIsPrecreated = i < _precreatedRoom.Count;

            if (currentRoomIsPrecreated)
            {
                currentRoom = _precreatedRoom[i];
            }
            else
            {
                currentRoom = _roomsPool.GetRandomWeightedObject().obj;
            }

            Vector2Int location = new Vector2Int(_random.Next(_borderLength, _size.x), _random.Next(0, _size.y));
            Vector2Int roomSize = currentRoom.LocalBounds.size;

            RectInt newRoom = new RectInt(location, roomSize);

            bool add = CanPlaceRoom(newRoom);

            if (add)
            {
                if (currentRoomIsPrecreated)
                {
                    AddRoom(currentRoom);
                }
                else
                {
                    PlaceRoom(currentRoom, location);
                }
            }
        }
    }

    private void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in _rooms)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.LocalBounds.position + ((Vector2)room.LocalBounds.size) / 2, room));
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
                PlaceHalway(_hallwayBorderPrefab, point);
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

            Room[] connectedRooms = new Room[2] { startRoom, endRoom };

            var startPosf = startRoom.LocalBounds.center;
            var endPosf = endRoom.LocalBounds.center;
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
                    pathCost.cost += 5;
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

                foreach (var pos in path)
                {
                    if (_grid[pos] == CellType.Hallway)
                    {
                        var placedHallway = PlaceHalway(_hallwayPrefab, pos);
                        foreach (Room connectedRoom in connectedRooms)
                        {
                            if (connectedRoom.OnTheBuffer(placedHallway.LocalBounds))
                            {
                                connectedRoom.AddBufferedHallway(placedHallway);
                            }
                        }
                    }
                }
            }
        }
    }

    private void FillGrid()
    {
        for (int x = _borderLength; x < _grid.Size.x - _borderLength; x++)
        {
            for (int y = _borderLength; y < _grid.Size.y - _borderLength; y++)
            {
                Vector2Int hallwayPoint = new Vector2Int(x, y);
                if (_grid[hallwayPoint] == CellType.None)
                {
                    PlaceHalway(_hallwayPrefab, hallwayPoint);
                }
            }
        }

        HashSet<Vector2Int> borderPoints = new();
        RectInt filledGrid = new RectInt(Vector2Int.one, _grid.Size - Vector2Int.one * 2);
        foreach (Vector2Int point in
            new RectInt(Vector2Int.zero, _grid.Size).allPositionsWithin)
        {
            if (filledGrid.Contains(point) == false)
                borderPoints.Add(point);
        }
        foreach(Vector2Int point in borderPoints)
        {
            PlaceHalway(_hallwayBorderPrefab, point);
        }
    }

    private Room PlaceRoom(Room room, Vector2Int position)
    {
        Vector3 placePosition = _startSpawnPoint.position +
            _startSpawnPoint.right * position.x + _startSpawnPoint.forward * position.y;
        Room spawnedRoom = Instantiate(room, placePosition, _startSpawnPoint.rotation);
        spawnedRoom.MoveBoundsPosition(position);

        NetworkObject networkRoom = spawnedRoom.GetComponent<NetworkObject>();
        networkRoom.Spawn(true);

        AddRoom(spawnedRoom);

        return spawnedRoom;
    }

    public void AddRoom(Room room)
    {
        _rooms.Add(room);

        foreach (var pos in room.LocalBounds.allPositionsWithin)
        {
            _grid[pos] = CellType.Room;
        }
    }

    private Hallway PlaceHalway(Hallway hallway, Vector2Int position)
    {
        _grid[position] = CellType.Hallway;
        Vector3 placePosition = _startSpawnPoint.position +
            _startSpawnPoint.right * position.x + _startSpawnPoint.forward * position.y;
        Hallway spawnedHallway = Instantiate(hallway, placePosition, _startSpawnPoint.rotation);
        spawnedHallway.MoveBoundsPosition(position);

        NetworkObject networkHallway = spawnedHallway.GetComponent<NetworkObject>();
        networkHallway.Spawn(true);

        return spawnedHallway;
    }

    public Room GarantedRandomPlaceRoom(Room room, Vector2Int step)
    {
        List<Vector2Int> placePositions = new();

        for (int tries = 0; tries < TRIES_COUNT; tries++)
        {
            Vector2Int randomPoint = new Vector2Int(_random.Next(0, _grid.Size.x - 1), _random.Next(0, _grid.Size.y - 1));
            Vector2Int roomSize = room.LocalBounds.size;
            if (CanPlaceRoom(new RectInt(randomPoint, roomSize)))
            {
                placePositions.Add(randomPoint);
            }
        }

        bool unlucky = placePositions.Count == 0;
        if (unlucky)
        {
            for (int x = 0; x < _grid.Size.x; x += step.x)
            {
                for (int y = 0; x < _grid.Size.y; y += step.y)
                {
                    if (!CanPlaceRoom(new RectInt(x, y, room.LocalBounds.width, room.LocalBounds.height))) { continue; }
                    placePositions.Add(new Vector2Int(x, y));
                }
            }
        }

        Vector2Int placePosition = placePositions[_random.Next(placePositions.Count - 1)];
        placePositions.Clear();
        Room placedRoom = PlaceRoom(room, placePosition);
        AddRoom(placedRoom);

        return placedRoom;
    }

    private bool CanPlaceRoom(RectInt roomData)
    {
        bool overlapGridBounds = roomData.xMax > _grid.Size.x - _borderLength ||
            roomData.yMax > _grid.Size.y - _borderLength ||
            roomData.position.x < _borderLength || roomData.position.y < _borderLength;

        if (overlapGridBounds)
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
}
