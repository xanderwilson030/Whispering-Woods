using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Player Details")]
    public bool isPlayerTurn;
    public CombatActions selectedCombatAction;
    public bool canSelect;
    public List<CombatActions> abilities;

    [Header("UI Details")]
    public Slider healthSlider;
    public Slider movementSlider;
    public Button meleeButton;
    public Button healButton;
    public Button rangedButton;

    [Header("Debug")]
    public List<GridTile> debugList;
    public Tilemap tilemap;


    private void Start()
    {
        curHp = maxHp;
        healthSlider.maxValue = maxHp;
        healthSlider.value = curHp;

        movementSlider.value = maxMovementRange;

        canSelect = false;

        meleeButton.onClick.AddListener(() => SelectTarget(abilities[0]));
        healButton.onClick.AddListener(() => SelectTarget(abilities[1]));
        rangedButton.onClick.AddListener(()=> SelectTarget(abilities[2]));
    }

    private void Update()
    {
        if (isPlayerTurn && canMove)
        {
            CalculateMovement();
        }

        if (canSelect)
        {
            SearchForTarget();
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

    public override void TakeCombatAction(CombatActions action, Character target)
    {
        switch (action.ActionType)
        {
            case Type.Melee:
                target.TakeDamage(action.Damage);
                Debug.Log($"Executing melee atack for {action.Damage} damage");
                break;
            case Type.Heal:
                Heal(action.HealAmount);
                Debug.Log($"Healing target for {action.HealAmount}");
                break;
            case Type.Ranged:
                Debug.LogError("Not Implemented Yet - Ranged Attack");
                break;
            default:
                Debug.LogError("Incompatible Combat Action Type Inputted");
                break;
        }
    }

    private void SelectTarget(CombatActions action)
    {
        canSelect = true;
        selectedCombatAction = action;
    }

    private void SearchForTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked Object: " + clickedObject.name);

                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

                if (enemy != null)
                {
                    canSelect = false;
                    Debug.Log("Enemy found and selected");
                    TakeCombatAction(selectedCombatAction, enemy);
                }
            }
        }
    }
}
