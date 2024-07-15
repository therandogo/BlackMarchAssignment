using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour, AI
{
    AStarPathfinding aStarPathfinding;
    PlayerBehavior playerBehavior;
    private List<Vector3> path = new List<Vector3>();
    private bool isMoving = false;
    private Transform playerTransform;
    private float spawnHeight;


    [SerializeField] float moveSpeed = 2f;

    private void Start()
    {
        playerBehavior.OnPlayerMoved += UpdatePathMethod;
        
    }


    // Helper Method to trigger the path update
    private void UpdatePathMethod()
    {
        StartCoroutine(UpdatePath());
    }


    // Coroutine to update the path towards the player
    private IEnumerator UpdatePath()
    {
        if (playerTransform != null)
        {
            MoveTowardsTarget(playerTransform.position);
        }
        yield return null; 
    }


    // Method to move the AI towards the target position
    public void MoveTowardsTarget(Vector3 targetPosition)
    {
        if (!isMoving)
        {
            Vector3 currentPosition = transform.position;
            path = aStarPathfinding.FindPath(currentPosition, GetAdjacentTile(targetPosition));

            if (path.Count > 0)
            {
                StartCoroutine(MoveAlongPath());
            }
        }
    }


    // Coroutine to move the AI along the calculated path
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;

        while (path.Count > 0)
        {
            Vector3 targetPosition = path[0];
            targetPosition += new Vector3(0, spawnHeight, 0);
            path.RemoveAt(0);

            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
    }


    // Method to get an adjacent tile to the target position
    private Vector3 GetAdjacentTile(Vector3 targetPosition)
    {
        // Find an adjacent tile to the player
        Vector3[] adjacentTiles = new Vector3[]
        {
            targetPosition + new Vector3(1, 0, 0),
            targetPosition + new Vector3(-1, 0, 0),
            targetPosition + new Vector3(0, 0, 1),
            targetPosition + new Vector3(0, 0, -1)
        };

        foreach (var tile in adjacentTiles)
        {
            if (aStarPathfinding.IsWithinGrid(aStarPathfinding.WorldPositionToGridIndex(tile)))
            {
                if (!aStarPathfinding.IsTileBlocked(aStarPathfinding.WorldPositionToGridIndex(tile)))
                {
                    return tile;
                }
            }
           
        }

        // If all adjacent tiles are blocked, return the target position itself
        return targetPosition; 
    }


    //getters and setters
    public void setPlayerTransformRef(Transform t) { playerTransform = t; }

    public void setAstarRef(AStarPathfinding a) { aStarPathfinding = a; }

    public void setSpawnHeight(float s) { spawnHeight = s; }

    public void setPlayerBehaviorRef(PlayerBehavior p) { playerBehavior = p; }
}
