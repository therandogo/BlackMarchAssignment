using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private TileGenerator tileGenerator;
    private List<GameObject> tiles;



    private void Awake()
    {
        tileGenerator = FindObjectOfType<TileGenerator>();
        if (tileGenerator != null)
        {
            tiles = tileGenerator.getTilesList();
        }
    }

    // Public method to find a path from start to end
    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        Vector2Int startIdx = WorldPositionToGridIndex(start);
        Vector2Int endIdx = WorldPositionToGridIndex(end);

        return AStar(startIdx, endIdx);
    }


    // Public method to find a path from start to end
    private List<Vector3> AStar(Vector2Int start, Vector2Int goal)
    {
        List<Vector3> path = new List<Vector3>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();

        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int next in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }


        // Reconstruct the path from the goal to the start
        Vector2Int pathStep = goal;
        while (pathStep != start)
        {
            path.Insert(0, GridIndexToWorldPosition(pathStep));
            pathStep = cameFrom[pathStep];
        }
        path.Insert(0, GridIndexToWorldPosition(start));

        return path;
    }


    // Heuristic function for A*
    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }


    // Get valid neighboring tiles
    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] possibleMoves = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (Vector2Int move in possibleMoves)
        {
            Vector2Int neighbor = new Vector2Int(node.x + move.x, node.y + move.y);
            if (IsWithinGrid(neighbor) && !IsTileBlocked(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }


    // Check if the given index is within grid bounds
    public bool IsWithinGrid(Vector2Int idx)
    {
        return idx.x >= 0 && idx.x < tileGenerator.Rows && idx.y >= 0 && idx.y < tileGenerator.Columns;
    }


    // Check if the tile at the given index is blocked
    public bool IsTileBlocked(Vector2Int idx)
    {
        int index = idx.x * tileGenerator.Columns + idx.y;
        return tiles[index].GetComponent<WorldTile>().getTileBlockedStatus();
    }


    // Convert world position to grid index
    public Vector2Int WorldPositionToGridIndex(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / tileGenerator.DistanceBetweenTiles);
        int z = Mathf.FloorToInt(worldPosition.z / tileGenerator.DistanceBetweenTiles);
        return new Vector2Int(x, z);
    }


    // Convert grid index to world position
    public Vector3 GridIndexToWorldPosition(Vector2Int gridIndex)
    {
        float x = gridIndex.x * tileGenerator.DistanceBetweenTiles;
        float z = gridIndex.y * tileGenerator.DistanceBetweenTiles;
        return new Vector3(x, 0, z);
    }
}
