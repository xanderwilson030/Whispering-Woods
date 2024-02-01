using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public abstract class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int curHp;
    public int maxHp;
    public int currentMovementNumber;
    public int maxMovementRange;

    [Header("Movement Details")]
    [SerializeField] protected GridTile startingPos;
    public float rayCastDistance;
    public LayerMask movementLayerMask;
    public GridTile currentTile;
    public bool canMove;

    [Header("Additional References")]
    [SerializeField] private Light2D highLight;

    private void Awake()
    {
        if (startingPos != null)
        {
            currentTile = startingPos.GetComponent<GridTile>();
        }
    }

    public abstract void StartTurn();
    public abstract void EndTurn();

    public abstract void CalculateMovement();
    public virtual void Move(RaycastHit2D hit)
    {
        if (hit.collider == null)
            return;

        gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;
        currentTile = hit.collider.gameObject.GetComponent<GridTile>();

        currentMovementNumber++;

        if (currentMovementNumber >= maxMovementRange)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
        }
    }
    public abstract void TakeCombatAction(CombatActions action, Character target);

    public void TakeDamage(int damageToTake)
    {
        Debug.Log("Damage to take: " + damageToTake);
        curHp -= damageToTake;

        if (curHp <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        curHp += healAmount;

        if (curHp > maxHp)
        {
            curHp = maxHp;
        }
    }

}
