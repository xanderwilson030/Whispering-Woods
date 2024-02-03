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
    public int meleeRange;
    public int rangedRange;

    [Header("UI Details")]
    public Slider movementSlider;
    public Button meleeButton;
    public Button healButton;
    public Button rangedButton;
    public GameObject combatButtons;
    public GameObject EndTurnButton;
    public GameObject turnText;
    public GameObject background;

    [Header("Debug")]
    public List<GridTile> debugList;
    public Tilemap tilemap;


    private void Start()
    {
        movementSlider.value = maxTurnActions;

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

        if (currentTile.isOccupied){
            Debug.Log("<color=orange> Destination Tile is occupied </color>");
            return;
        }

        currentActionCount++;
        movementSlider.value = (maxTurnActions - currentActionCount);

        audioSource.clip = clips[4];
        audioSource.Play();

        if (currentActionCount >= maxTurnActions)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
        }
    }

    public override void StartTurn()
    {
        isPlayerTurn = true;
        canMove = true;
        currentActionCount = 0;
        movementSlider.value = maxTurnActions;
        turnText.SetActive(false);
        combatButtons.SetActive(true);
        EndTurnButton.SetActive(true);
        background.SetActive(true);
    }

    public override void EndTurn()
    {
        isPlayerTurn = false;
        GameEvents.instance.e_TurnOver.Invoke();

        combatButtons.SetActive(false);
        EndTurnButton.SetActive(false);
        background.SetActive(false);
        turnText.SetActive(true);
    }

    public override void TakeCombatAction(CombatActions action, Character target)
    {
        switch (action.ActionType)
        {
            case Type.Melee:
                if (Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) > meleeRange){
                    canSelect = true;
                    Debug.Log("<color=orange> Enemy is not close enough for melee attack </color>");
                    return;
                }
                target.TakeDamage(action.Damage);
                audioSource.clip = clips[2];
                audioSource.Play();
                Debug.Log($"Executing melee attack for {action.Damage} damage");
                break;
            case Type.Heal:
                audioSource.clip = clips[5];
                audioSource.Play();
                Heal(action.HealAmount);
                Debug.Log($"Healing target for {action.HealAmount}");
                break;
            case Type.Ranged:
                if (Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) > rangedRange)
                {
                    canSelect = true;
                    Debug.Log("<color=orange> Enemy is not close enough for ranged attack </color>");
                    return;
                }
                audioSource.clip = clips[3];
                audioSource.Play();

                //GameObject arrow = Instantiate(action.ProjectilePrefab, gameObject.transform.position, Quaternion.identity);
                //arrow.GetComponent<Projectile>().LookTowardsTarget(target.transform);

                target.TakeDamage(action.Damage);
                Debug.Log($"Executing ranged attack for {action.Damage} damage");
                break;
            default:
                Debug.LogError("Incompatible Combat Action Type Inputted");
                break;
        }

        currentActionCount++;
        movementSlider.value = (maxTurnActions - currentActionCount);
    }

    private void SelectTarget(CombatActions action)
    {
        if (currentActionCount + attackActionCost >= maxTurnActions)
        {
            Debug.Log("<color=orange> Player doesn't have enough action points left to do the requested action </color>");
            return;
        }
        else
        {
            canSelect = true;
            selectedCombatAction = action;
            currentActionCount += attackActionCost;
        }
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
                else
                {
                    Player player = hit.collider.gameObject.GetComponent<Player>();

                    if (player != null)
                    {
                        canSelect = false;
                        Debug.Log("Selected Player");
                        
                        if (selectedCombatAction.ActionType == Type.Heal)
                        {
                            TakeCombatAction(selectedCombatAction, player);
                        }
                    }
                }
            }
        }
    }

    public override void OnMouseEnter()
    {
        isHighlighted = true;

        if (canSelect)
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
