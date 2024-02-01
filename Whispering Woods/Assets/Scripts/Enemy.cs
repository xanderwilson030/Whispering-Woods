using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Header("Object References")]
    [SerializeField] private Player player;    


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>(); ;
    }

    public override void CalculateMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void EndTurn()
    {
        throw new System.NotImplementedException();
    }

    public override void StartTurn()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeCombatAction(CombatActions action, Character target)
    {
        throw new System.NotImplementedException();
    }

    private void OnMouseEnter()
    {
        isHighlighted = true;

        if (player.canSelect)
        {
            highLight.intensity = selectedHighLightIntensity;
        }
    }

    private void OnMouseExit()
    {
        isHighlighted = false;
        highLight.intensity = baseHighLightIntensity;
    }
}
