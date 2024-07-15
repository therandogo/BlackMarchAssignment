using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObstacleData data = (ObstacleData)target;

        data.obstaclePrefab = (GameObject)EditorGUILayout.ObjectField("Obstacle Prefab", data.obstaclePrefab, typeof(GameObject), false);

        for (int i = 0; i < 10; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 10; j++)
            {
                data.obstacles[i * 10 + j] = GUILayout.Toggle(data.obstacles[i * 10 + j], "");
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorUtility.SetDirty(target);
    }
}

