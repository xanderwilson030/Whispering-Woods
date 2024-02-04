using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * This class is the parent for all tiles in the game
 * 
 */

public class GridTile : MonoBehaviour
{
    [Header("Location Data")]
    public Transform gridCenter;
    public Vector3 cellInWorldPos;
    public Vector3Int cellInTileMapPos;
    public Tilemap tilemap;

    [Header("Object Data")]
    public bool isLit;
    public bool isOccupied = false;
    public bool isWalkable;
    private GameObject currentOccupant;

    [Header("Tile Variations")]
    [SerializeField] Sprite[] tileVariations;
    public bool hasMultipleVariations;

    [Header("Neighbors")]
    [SerializeField] private List<GridTile> neighbors;

    private SpriteRenderer sr;

    void Start()
    {
        tilemap = GameObject.Find("TilemapFloor").GetComponent<Tilemap>();
        sr = GetComponent<SpriteRenderer>();

        SetWorldLocation();

        //// Get the world position of the tile's GameObject
        //Vector3 worldPosition = gameObject.transform.position;

        //// Convert the world position to cell position (Vector3Int)
        //Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        //cellInTileMapPos = cellPosition;

        //neighbors = LocateNeighbors(cellPosition);

        if (hasMultipleVariations)
        {
            RandomizeApppearance();
        }
    }

    /*
     * This method assigns a world location to the tile's center position
     * The player must use the world position when moving to a tile
     */
    private void SetWorldLocation()
    {
        Grid grid = transform.parent.gameObject.transform.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        cellInWorldPos = grid.GetCellCenterWorld(cellPosition);
    }

    /*
     * This method identifies the tiles neighbors
     */
    public List<GridTile> LocateNeighbors(Vector3Int cellPosition)
    {
        List<GridTile> neighborTiles = new List<GridTile>();

        Debug.Log("Current Cell Position: " + cellPosition);

        Vector3Int[] neighborOffsets = new Vector3Int[]
        {
        new Vector3Int(0, 1, 0), // Top
        new Vector3Int(1, 0, 0), // Right
        new Vector3Int(0, -1, 0), // Bottom
        new Vector3Int(-1, 0, 0) // Left
        };

        foreach (var offset in neighborOffsets)
        {
            Vector3Int neighborPosition = cellPosition + offset;
            Debug.Log("Checking Neighbor Position: " + neighborPosition);

            TileBase neighborTile = tilemap.GetTile(neighborPosition);

            if (neighborTile != null)
            {
                GameObject neighborGameObject = tilemap.GetInstantiatedObject(neighborPosition);
                Debug.Log("Running");

                if (neighborGameObject != null)
                {
                    GridTile gridTileScript = neighborGameObject.GetComponent<GridTile>();
                    Debug.Log("Running2");

                    if (gridTileScript != null)
                    {
                        neighborTiles.Add(gridTileScript);
                        Debug.Log("Adding");
                    }
                    else
                    {
                        Debug.LogWarning("GridTile script not found on neighbor tile GameObject.");
                    }
                }
                else
                {
                    Debug.LogWarning("No tile GameObject found for neighbor at position: " + neighborPosition);
                }
            }
            else
            {
                Debug.LogWarning("No tile found for neighbor at position: " + neighborPosition);
            }
        }

        Debug.Log("Size of neighbor tiles: " + neighborTiles.Count);
        return neighborTiles;
    }

    /*
     * This method randomizes the appearance of a tile
     */
    private void RandomizeApppearance()
    {
        sr.sprite = tileVariations[Random.Range(0, tileVariations.Length)];
    }

    /*
     * This method is called when an enemy (or player) occupies the tile
     */
    public void OnTileEnter(GameObject occupant)
    {
        isOccupied = true;
        currentOccupant = occupant;
    }

    /*
     * This method is called when an enemy (or player) leaves a tile
     */
    public void OnTileExit()
    {
        isOccupied = false;
        currentOccupant = null;
    }

}
