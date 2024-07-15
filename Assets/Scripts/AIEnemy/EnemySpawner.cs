using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnHeight = 0.5f;

    TileGenerator tileGenerator;
    AStarPathfinding aStarPathfinding;
    PlayerBehavior playerBehavior;
    GameObject player;

    private void Start()
    {
        tileGenerator = GetComponent<TileGenerator>();
        aStarPathfinding = GetComponent<AStarPathfinding>();
        playerBehavior = GetComponent<PlayerBehavior>();


        if (tileGenerator != null)
        {
            playerBehavior.OnPlayerSpawned += SpawnUnits;
        }
        else
        {
            Debug.LogError("TileGenerator not found in the scene.");
        }

        if (aStarPathfinding == null)
        {
            Debug.LogError("AStarPathfinding component not found in the scene.");
        }

        if (playerBehavior == null)
        {
            Debug.LogError("playerBehavior component not found in the scene.");
        }

    }

    // Method to spawn enemy units.
    private void SpawnUnits()
    {

        List<GameObject> tiles = tileGenerator.getTilesList();
        

        if (tiles != null && tiles.Count > 0)
        {
            // Get the position of the last tile in the list and adjust the spawn height.
            Vector3 enemySpawnPos = tiles[tiles.Count - 1].transform.position;
            enemySpawnPos += new Vector3(0, spawnHeight, 0);

            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPos, enemyPrefab.transform.rotation);

             player = GetComponent<PlayerBehavior>().getPlayerRef();

            if (player != null && aStarPathfinding != null)
            {
                // Set references in the enemy's AI behavior script.
                enemy.GetComponent<AIBehavior>().setPlayerTransformRef(player.transform);
                enemy.GetComponent<AIBehavior>().setAstarRef(aStarPathfinding);
                enemy.GetComponent<AIBehavior>().setSpawnHeight(spawnHeight);
                enemy.GetComponent<AIBehavior>().setPlayerBehaviorRef(playerBehavior);
            }
        }
        else
        {
            Debug.LogError("Tiles list is empty or not initialized.");
        }
    }

}
