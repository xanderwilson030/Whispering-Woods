using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Player Details")]
    public bool isPlayerTurn;

    [Header("UI Details")]
    public Slider healthSlider;
    public Slider movementSlider;

    [Header("Debug")]
    public List<GridTile> debugList;
    public Tilemap tilemap;


    private void Start()
    {
        curHp = maxHp;
        healthSlider.maxValue = maxHp;
        healthSlider.value = curHp;

        movementSlider.value = maxMovementRange;
    }

    private void Update()
    {
        if (isPlayerTurn && canMove)
        {
            CalculateMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Searching for tile");
            LocateNeighbors(new Vector3Int(-7, 0,0));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked Object: " + clickedObject.name);

                // Add more debug statements or checks here
            }
        }
    }

    public override void CalculateMovement()
    {
        RaycastHit2D hit;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up, Vector3.up, rayCastDistance, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.up, Vector3.up, Color.red, rayCastDistance);

            Move(hit);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector3.down, rayCastDistance, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.down, Vector3.down, Color.red, rayCastDistance);

            Move(hit);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.left, Vector3.left, rayCastDistance, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.left, Vector3.left, Color.red, rayCastDistance);

            Move(hit);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.right, Vector3.right, rayCastDistance, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.right, Vector3.right, Color.red, rayCastDistance);

            Move(hit);
        }
    }

    public override void Move(RaycastHit2D hit)
    {
        if (hit.collider == null)
            return;

        gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;
        currentTile = hit.collider.gameObject.GetComponent<GridTile>();

        currentMovementNumber++;
        movementSlider.value = (maxMovementRange - currentMovementNumber);

        if (currentMovementNumber >= maxMovementRange)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
        }
    }

    public override void StartTurn()
    {
        isPlayerTurn = true;
        canMove = true;
        currentMovementNumber = 0;
        movementSlider.value = maxMovementRange;
    }

    public override void EndTurn()
    {
        isPlayerTurn = false;
    }

    public override void TakeCombatAction(CombatActions action)
    {
        throw new System.NotImplementedException();
    }

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


}
