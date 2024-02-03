using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public UnityEvent e_TurnStart;
    public UnityEvent e_TurnOver;
    public UnityEvent<Character> e_CharacterDied;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if (e_TurnStart == null)
        {
            e_TurnStart = new UnityEvent();
        }

        if (e_TurnOver == null)
        {
            e_TurnOver = new UnityEvent();
        }

        if (e_CharacterDied == null)
        {
            e_CharacterDied = new UnityEvent<Character>();
        }
    }
}
