using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is the parent for all tiles in the game
 * 
 */

public class GridTile : MonoBehaviour
{
    [Header("Location Data")]
    public Transform gridCenter;
    public Vector3 cellInWorldPos;

    [Header("Object Data")]
    public bool isLit;
    public bool isOccupied;
    public bool isWalkable;
    private GameObject currentOccupant;

    [Header("Tile Variations")]
    [SerializeField] Sprite[] tileVariations;
    public bool hasMultipleVariations;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        SetWorldLocation();

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
