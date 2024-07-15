using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    
    private Vector3 tilePosition;
    private List<GameObject> tiles = new List<GameObject>();
    private int rows = 10;
    private int columns = 10;
    private int distanceBetweenTiles = 1;

    [SerializeField] GameObject tilePrefab;

    public delegate void GridGeneratedHandler();
    public event GridGeneratedHandler OnGridGenerated;

    void Start()
    {
        //Setting the start location for the tiles
        tilePosition = new Vector3(0f, 0f, 0f);     

        generateTiles();

        OnGridGenerated?.Invoke();
    }


    //Instantiates the prefab on the givel location
    private void generateTiles()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);       
                tiles.Add(tile);
                tilePosition.z += distanceBetweenTiles;   
            }
            tilePosition.x += distanceBetweenTiles;
            tilePosition.z = 0f;    
        }
    }

    

    //getters and setters
    public int DistanceBetweenTiles { get { return distanceBetweenTiles; } }
    public int Rows { get { return rows; } }
    public int Columns { get { return columns; } }
    public List<GameObject> getTilesList() { return tiles; }

}
