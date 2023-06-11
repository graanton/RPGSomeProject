using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public int width;
        public int height;
        public Vector2Int position;
    }

    [SerializeField]
    private int _dungeonWidth = 5;
    [SerializeField]
    private int _dungeonHeight = 5;
    [SerializeField]
    private Room[] _rooms;
    [SerializeField]
    private GameObject _floorTile;
    [SerializeField]
    private GameObject _wallTile;
    [SerializeField]
    private GameObject _corridorTile;
    [SerializeField]
    private GameObject _doorTile;

    private List<Vector2Int> _roomPositions;

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        _roomPositions = new List<Vector2Int>();

        foreach (Room room in _rooms)
        {
            GenerateRoom(room);
        }

        foreach (Vector2Int position in _roomPositions)
        {
            GenerateCorridors(position);
        }
    }

    private void GenerateRoom(Room room)
    {
        int roomX = Random.Range(0, _dungeonWidth - room.width);
        int roomY = Random.Range(0, _dungeonHeight - room.height);

        room.position = new Vector2Int(roomX, roomY); // Запоминаем позицию комнаты

        // Генерируем тайлы комнаты
        for (int x = roomX; x < roomX + room.width; x++)
        {
            for (int y = roomY; y < roomY + room.height; y++)
            {
                Vector3 tilePosition = new Vector3(x, 0, y);
                Instantiate(_floorTile, tilePosition, Quaternion.identity);
            }
        }

        // Генерируем тайлы стен комнаты
        for (int x = roomX - 1; x <= roomX + room.width; x++)
        {
            for (int y = roomY - 1; y <= roomY + room.height; y++)
            {
                if (x == roomX - 1 || x == roomX + room.width || y == roomY - 1 || y == roomY + room.height)
                {
                    Vector3 tilePosition = new Vector3(x, 0.5f, y);
                    Instantiate(_wallTile, tilePosition, Quaternion.identity);
                }
                else if (x == roomX + room.width / 2 && y == roomY + room.height)
                {
                    Vector3 tilePosition = new Vector3(x, 0, y);
                    Instantiate(_doorTile, tilePosition, Quaternion.identity);
                }
            }
        }

        _roomPositions.Add(new Vector2Int(roomX, roomY));
    }

    private void GenerateCorridors(Vector2Int roomPosition)
    {
        Room currentRoom = _rooms[_roomPositions.IndexOf(roomPosition)];

        foreach (Room room in _rooms)
        {
            if (room == currentRoom)
                continue;

            // Найти путь между комнатами с помощью алгоритма A*
            List<Vector2Int> path = FindPath(roomPosition, room.position);

            // Создать коридоры по найденному пути
            if (path != null)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    Vector2Int from = path[i - 1];
                    Vector2Int to = path[i];

                    GenerateStraightCorridor(from, to);
                }
            }
        }
    }

    private List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();

        // Приоритетная очередь для хранения позиций в порядке приоритета
        SortedSet<Vector2Int> frontier = new SortedSet<Vector2Int>(Comparer<Vector2Int>.Create((a, b) =>
        {
            float priorityA = costSoFar[a] + Heuristic(a, goal);
            float priorityB = costSoFar[b] + Heuristic(b, goal);
            return priorityA.CompareTo(priorityB);
        }));

        frontier.Add(start);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Min;
            frontier.Remove(current);

            if (current == goal)
                break;

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + 1; // Assuming all moves have a cost of 1

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    frontier.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found!");
            return null;
        }

        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPos = goal;

        while (currentPos != start)
        {
            path.Add(currentPos);
            currentPos = cameFrom[currentPos];
        }

        path.Reverse();

        return path;
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = position + direction;
            neighbors.Add(neighbor);
        }

        return neighbors;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private void GenerateStraightCorridor(Vector2Int from, Vector2Int to)
    {
        Vector2Int direction = to - from;
        int corridorLength = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

        Vector2Int step = new Vector2Int(Mathf.RoundToInt(Mathf.Sign(direction.x)), Mathf.RoundToInt(Mathf.Sign(direction.y)));

        Vector2Int currentPos = from;
        for (int i = 0; i <= corridorLength; i++)
        {
            Vector3 tilePosition = new Vector3(currentPos.x, 0, currentPos.y);
            Instantiate(_corridorTile, tilePosition, Quaternion.identity);
            currentPos += step;
        }
    }



}
