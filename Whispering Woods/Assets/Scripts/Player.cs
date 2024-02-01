using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Player Details")]
    public bool isPlayerTurn;

    [Header("UI Details")]
    public Slider healthSlider;
    public Slider movementSlider;


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

    
}
