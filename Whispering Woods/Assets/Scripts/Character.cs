using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int curHp;
    public int maxHp;
    public int currentMovementNumber;
    public int maxMovementRange;

    public abstract void StartTurn();
    public abstract void Move();
    public abstract void TakeCombatAction();

    public void TakeDamage(int damageToTake)
    {
        Debug.Log("Damage to take: " + damageToTake);
        curHp -= damageToTake;

        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
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
