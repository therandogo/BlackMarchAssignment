using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    InputHandler Ih;
    TileUIHandler Tui;
    TileGenerator tileGenerator;
    AStarPathfinding aStarPathfinding;

    private GameObject player;
    private List<GameObject> tiles;
    private bool isMoving = false;
    private List<Vector3> path = new List<Vector3>();

    [SerializeField] Camera _camera;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float spawnHeight = 0.5f;

    public delegate void PlayerSpawnHandler();
    public event PlayerSpawnHandler OnPlayerSpawned;

    public delegate void PlayerMovementHandler();
    public event PlayerMovementHandler OnPlayerMoved;

    private void Start()
    {
        Ih = GetComponent<InputHandler>();
        Tui = GetComponent<TileUIHandler>();
        tileGenerator = FindObjectOfType<TileGenerator>();
        aStarPathfinding = FindObjectOfType<AStarPathfinding>();

        if (tileGenerator != null)
        {
            tileGenerator.OnGridGenerated += SpawnPlayer;
        }
        else
        {
            Debug.LogError("TileGenerator not found in the scene.");
        }

        if (_camera == null)
        {
            Debug.LogError("Camera is not assigned in the inspector.");
        }

        if (aStarPathfinding == null)
        {
            Debug.LogError("AStarPathfinding component not found in the scene.");
        }

        if (Tui == null)
        {
            Debug.LogError("TileUIHandler component not found in the scene.");
        }

        if (Ih == null)
        {
            Debug.LogError("InputHandler component not found in the scene.");
        }
    }


    private void Update()
    {
        MouseSelection();

        if (path.Count > 0 && !isMoving)
        {
            StartCoroutine(MoveAlongPath());
        }
    }


    //Function to do a raycast and display the location of the hit instanced tile
    //Also does the check if the hovered tile is selected
    private void MouseSelection()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            WorldTile tileInfo = hit.collider.GetComponent<WorldTile>();
            if (tileInfo != null)
            {
                //tileInfo.onHover();
                Tui.setText(tileInfo.getTileTransform().position.ToString());
                Tui.setTextPosition(tileInfo.getTileTransform().position);
                Tui.getTextTransform().LookAt(Tui.getTextTransform().position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);

                if (Ih.LMBTap() && !isMoving && !tileInfo.getTileBlockedStatus())
                {
                    path = aStarPathfinding.FindPath(player.transform.position, tileInfo.getTileTransform().position);
                    if (!tileInfo.getSelected())
                    {
                        tileInfo.OnSelectionSuccess();
                    }
                }
                else if(Ih.LMBTap() && tileInfo.getTileBlockedStatus())
                {
                    if (!tileInfo.getSelected())
                    {
                        tileInfo.OnSelectionFailure();
                    }
                }
            }
        }
    }


    // Spawns the player at the initial tile
    private void SpawnPlayer()
    {
        tiles = tileGenerator.getTilesList();
        if (tiles != null && tiles.Count > 0)
        {
            Vector3 spawnPos = tiles[0].transform.position;
            spawnPos += new Vector3(0, spawnHeight, 0);
            player = Instantiate(playerPrefab, spawnPos, playerPrefab.transform.rotation);

            OnPlayerSpawned?.Invoke();
        }
        else
        {
            Debug.LogError("Tiles list is empty or not initialized.");
        }
    }


    // Coroutine to move the player along the path
    private IEnumerator MoveAlongPath()
    {
        isMoving = true;

        while (path.Count > 0)
        {
            Vector3 targetPosition = path[0];
            targetPosition +=  new Vector3(0, spawnHeight, 0);
            path.RemoveAt(0);

            while (player.transform.position != targetPosition)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition , moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
        OnPlayerMoved?.Invoke();
    }

    //getter for player ref
    public GameObject getPlayerRef() { return player; }
}