using Graphs;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using Random = System.Random;

public class Generator2D : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        Hallway
    }

    [SerializeField] private Vector2Int _size;
    [SerializeField] private int _roomCount;
    [SerializeField] private int _seed;
    [SerializeField, Range(0, 1)] private float _roomConnectionChance = 0.125f;
    [SerializeField] Pool<Room> _roomsPool;
    [SerializeField] private Room _hallwayPrefab;
    [SerializeField] private Transform _root;

    private Random _random;
    private Grid2D<CellType> _grid;
    private List<Room> _rooms;
    private Delaunay2D _delaunay;
    private HashSet<Prim.Edge> _selectedEdges;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        _random = new Random(_seed);
        _grid = new Grid2D<CellType>(_size, Vector2Int.zero);
        _rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
    }

    private void PlaceRooms()
    {
        for (int i = 0; i < _roomCount; i++)
        {
            Room currentRoom = _roomsPool.GetRandomWeightedObject().obj;
            currentRoom.BoundsInit();

            Vector2Int location = new Vector2Int(_random.Next(1, _size.x), _random.Next(1 ,_size.y));
            currentRoom.MoveBounds(location);
            Vector2Int roomSize = currentRoom.bounds.size;

            bool add = true;
            RectInt newRoom = new RectInt(location, roomSize);
            RectInt buffer = new RectInt(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in _rooms)
            {
                if (Room.Intersect(room.bounds, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.xMin < 0 || newRoom.xMax >= _size.x
                || newRoom.yMin < 0 || newRoom.yMax >= _size.y)
            {
                add = false;
            }

            if (add)
            {
                _rooms.Add(currentRoom);
                PlaceRoom(currentRoom, location);

                foreach (var pos in newRoom.allPositionsWithin)
                {
                    _grid[pos] = CellType.Room;
                }
            }
        }
    }

    private void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in _rooms)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
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
            if (_random.NextDouble() < 1f / _roomConnectionChance)
            {
                _selectedEdges.Add(edge);
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

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
            {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

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
                        PlaceRoom(_hallwayPrefab, pos);
                    }
                }
            }
        }
    }

    private void PlaceRoom(Room room, Vector2Int position)
    {
        Instantiate(room, _root.position + Vector3.Scale((Vector3Int)room.bounds.position + new Vector3(position.x, 0, position.y), _root.right + _root.forward), _root.rotation);
    }
}
