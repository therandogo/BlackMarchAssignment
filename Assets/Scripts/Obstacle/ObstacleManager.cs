using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    TileGenerator tileGenerator;

    [SerializeField] ObstacleData obstacleData;
    [SerializeField] float HeightofObstacle;


    void Awake()
    {
        tileGenerator = GetComponent<TileGenerator>();

        if (tileGenerator == null)
        {
            Debug.LogError("tileGenerator is not assigned in the inspector.");
        }
    }

    void OnEnable()
    {
        // Subscribe to the event
        tileGenerator.OnGridGenerated += GenerateObstacles;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        tileGenerator.OnGridGenerated -= GenerateObstacles;
    }


    // Method to generate obstacles on the grid
    void GenerateObstacles()
    {
        var tilesList = new List<GameObject>(tileGenerator.getTilesList());

        // Loop through each tile and check if it should have an obstacle
        for (int i = 0;i < tilesList.Count; i++)   
        {
            if (obstacleData.obstacles[i])
            {
                Transform t = tilesList[i].transform;
                Instantiate(obstacleData.obstaclePrefab, new Vector3(t.position.x, HeightofObstacle, t.position.z), Quaternion.identity);
                tilesList[i].GetComponent<WorldTile>().setTileBlockedStatus(true);
            }
        }
    }


}
