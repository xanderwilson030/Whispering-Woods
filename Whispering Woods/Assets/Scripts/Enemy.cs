using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Enemy : Character
{
    [Header("Object References")]
    [SerializeField] private Player player;

    [Header("Movement Details")]
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private int horizontalDistanceToPlayer;
    [SerializeField] private int verticalDistanceToPlayer;
    public int movementCooldown;

    [Header("Combat Details")]
    public List<CombatActions> abilities;
    [SerializeField] private AnimationCurve healChanceCurve;

    //[Header("UI Details")]


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public override void CalculateMovement()
    {
        //Debug.Break();
        distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Debug.Log($"Distance to player is: {distanceToPlayer}");
        Debug.Log($"Player location: {player.gameObject.transform.position} | Enemy location: {gameObject.transform.position}");

        horizontalDistanceToPlayer = (int)(player.gameObject.transform.position.x - gameObject.transform.position.x);
        verticalDistanceToPlayer = (int)(player.gameObject.transform.position.y - gameObject.transform.position.y);
        Debug.Log($"H Distance: {horizontalDistanceToPlayer}:{verticalDistanceToPlayer}");

        

        if (distanceToPlayer > 1)
        {
            bool canMoveNorth = false;
            bool canMoveSouth = false;
            bool canMoveEast = false;
            bool canMoveWest = false;
            
            if (verticalDistanceToPlayer > 0)
            {
                canMoveNorth = TryMovementDirection(Vector2.up);
            }

            if (verticalDistanceToPlayer < 0)
            {
                canMoveSouth = TryMovementDirection(Vector2.down);
            }

            if (horizontalDistanceToPlayer < 0)
            {
                canMoveWest = TryMovementDirection(Vector2.left);
            }

            if (horizontalDistanceToPlayer > 0)
            {
                canMoveEast = TryMovementDirection(Vector2.right);
            }

            Debug.Log($"North: {canMoveNorth} | South: {canMoveSouth} | East: {canMoveEast} | West: {canMoveWest}");

        }
        else
        {
            Debug.Log("Should be attacking player");
            DetermineCombatAction();
        }
    }

    private bool TryMovementDirection(Vector2 dir)
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast((Vector2)transform.position + dir, Vector3.up, rayCastDistance, movementLayerMask);
        Debug.DrawRay((Vector2)transform.position + dir, Vector3.up, Color.red, rayCastDistance);

        if (hit.collider == null)
        {
            Debug.Log($"<color=orange> Identified space is null, returning false for {dir} movement </color>");
            return false;
        }
        else
        {
            gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;
            currentTile = hit.collider.gameObject.GetComponent<GridTile>();

            if (currentTile.isOccupied)
            {
                Debug.Log($"<color=orange> Destination Tile is occupied, returning false for {dir} movement </color>");
                return false;
            }
            else
            {
                Move(hit);
                return true;
            }
        }
    }

    public override void Move(RaycastHit2D hit)
    {       
        gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;
        currentTile = hit.collider.gameObject.GetComponent<GridTile>();

        currentActionCount++;

        audioSource.clip = clips[3];
        audioSource.Play();

        if (currentActionCount >= maxTurnActions)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
            //EndTurn();
        }
        else
        {
            StartCoroutine(MovementDelay());
        }
    }

    private IEnumerator MovementDelay()
    {
        float timer = movementCooldown;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        Debug.Log("Movement Cooldown over");
        CalculateMovement();
    }



    private void DetermineCombatAction()
    {
        float healthPercentage = curHp / maxHp;
        bool wantToHeal = Random.value < healChanceCurve.Evaluate(healthPercentage);
        
        if (wantToHeal)
        {
            TakeCombatAction(abilities[1], this);
        }
        else
        {
            TakeCombatAction(abilities[0], player);
        }

        currentActionCount++;

        if (currentActionCount >= maxTurnActions)
        {
            //EndTurn();
        }
        else
        {
            StartCoroutine(MovementDelay());
        }
    }

    public override void TakeCombatAction(CombatActions action, Character target)
    {
        if (action.ActionType == Type.Heal)
        {
            Heal(action.HealAmount);
            audioSource.clip = clips[4];
            audioSource.Play();
        }

        if (action.ActionType == Type.Melee)
        {
            Debug.Log($"Attacked {target.name} for {action.Damage} damage");
            target.TakeDamage(action.Damage);
            audioSource.clip = clips[2];
            audioSource.Play();
        }
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

    public override void EndTurn()
    {
        canMove = false;
        GameEvents.instance.e_TurnOver.Invoke();
    }

    public override void StartTurn()
    {
        currentActionCount = 0;
        canMove = true;
        CalculateMovement();
        StartCoroutine(EnemyTurnCounter());
    }

    private IEnumerator EnemyTurnCounter()
    {
        while (currentActionCount < maxTurnActions)
        {
            yield return null;
        }

        EndTurn();
    }
}
