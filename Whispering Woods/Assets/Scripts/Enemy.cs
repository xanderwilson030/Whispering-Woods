using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    [Header("Object References")]
    [SerializeField] private Player player;

    //[Header("UI Details")]


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
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
        currentActionCount = 0;
        
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
