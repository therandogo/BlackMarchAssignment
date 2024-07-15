using UnityEngine;

// This attribute allows you to create instances of this ScriptableObject from the Unity editor menu.
[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData", order = 1)]


public class ObstacleData : ScriptableObject
{
    // An array of booleans representing the presence of obstacles in a grid or level.
    public bool[] obstacles = new bool[100];

    // A reference to a prefab for the obstacle. This can be used to instantiate obstacles in the scene.
    public GameObject obstaclePrefab;
}
