using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public TurnState currentTurn;

    [Header("Object references")]
    public GameObject player;
    public List<Character> characterList;
    public int currentCharacterIndex = -1;
    public Character currentCharacter;
    
    public static GameManager instance { get; private set; }

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
    }

    private void Start()
    {
        GameEvents.instance.e_TurnOver.AddListener(BeginNextTurn);
    }

    /*
     * This method runs when the Turn Over event is called
     */
    private void BeginNextTurn()
    {
        if (currentCharacterIndex++ > characterList.Count)
        {
            currentCharacterIndex = 0;
        }
        else
        {
            currentCharacterIndex++;
        }

        currentCharacter = characterList[currentCharacterIndex];

        currentCharacter.StartTurn();
    }


}
