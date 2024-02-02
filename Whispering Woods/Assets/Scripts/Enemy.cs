using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    [Header("Object References")]
    [SerializeField] private Player player;

    [Header("Movement Details")]
    [SerializeField] private int horizontalTileDistance;
    [SerializeField] private int verticalTileDistance;

    //[Header("UI Details")]


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void CalculateMovement()
    {
        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Debug.Log($"Distance to player is: {distanceToPlayer}");
        Debug.Log($"Player location: {player.gameObject.transform.position} | Enemy location: {gameObject.transform.position}");

        int horizontalDistanceToPlayer = (int)(player.gameObject.transform.position.x - gameObject.transform.position.x);
        int verticalDistanceToPlayer = (int)(player.gameObject.transform.position.y - gameObject.transform.position.y);
        Debug.Log($"H Distance: {horizontalDistanceToPlayer}:{verticalDistanceToPlayer}");

        if (horizontalDistanceToPlayer > 0 || verticalDistanceToPlayer > 0)
        {
            RaycastHit2D hit;

            if (verticalDistanceToPlayer > 0)
            {
                hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up, Vector3.up, rayCastDistance, movementLayerMask);
                Debug.DrawRay((Vector2)transform.position + Vector2.up, Vector3.up, Color.red, rayCastDistance);

                Move(hit);
            }
            else if (verticalDistanceToPlayer < 0)
            {
                hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector3.down, rayCastDistance, movementLayerMask);
                Debug.DrawRay((Vector2)transform.position + Vector2.down, Vector3.down, Color.red, rayCastDistance);

                Move(hit);

            }
            else if (horizontalDistanceToPlayer < 0)
            {
                hit = Physics2D.Raycast((Vector2)transform.position + Vector2.left, Vector3.left, rayCastDistance, movementLayerMask);
                Debug.DrawRay((Vector2)transform.position + Vector2.left, Vector3.left, Color.red, rayCastDistance);

                Move(hit);
            }
            else if (horizontalDistanceToPlayer > 0)
            {
                hit = Physics2D.Raycast((Vector2)transform.position + Vector2.right, Vector3.right, rayCastDistance, movementLayerMask);
                Debug.DrawRay((Vector2)transform.position + Vector2.right, Vector3.right, Color.red, rayCastDistance);

                Move(hit);
            }
        }
    }

    public override void Move(RaycastHit2D hit)
    {
        if (hit.collider == null)
            return;

        gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;
        currentTile = hit.collider.gameObject.GetComponent<GridTile>();

        if (currentTile.isOccupied)
        {
            Debug.Log("<color=orange> Destination Tile is occupied </color>");
            return;
        }

        currentActionCount++;

        if (currentActionCount >= maxTurnActions)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
        }
    }

    public override void EndTurn()
    {
        throw new System.NotImplementedException();
    }

    public override void StartTurn()
    {
        currentActionCount = 0;
        CalculateMovement();
    }



    public override void TakeCombatAction(CombatActions action, Character target)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseEnter()
    {
        isHighlighted = true;

        if (player.canSelect)
        {
            highLight.intensity = selectedHighLightIntensity;
        }
    }

    public override void OnMouseExit()
    {
        isHighlighted = false;
        highLight.intensity = baseHighLightIntensity;
    }
}
