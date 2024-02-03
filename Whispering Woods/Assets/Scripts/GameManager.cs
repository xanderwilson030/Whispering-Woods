using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    public TurnState currentTurn;

    [Header("Object references")]
    public GameObject player;
    public List<Character> characterList;
    public int currentCharacterIndex = -1;
    public Character currentCharacter;

    [Header("Camera Variables")]
    public CinemachineVirtualCamera virtualCamera;
    
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
        GameEvents.instance.e_CharacterDied.AddListener(CharacterDied);

        BeginNextTurn();
    }

    private void Update()
    {
        if (player == null)
        {
            SceneManager.LoadScene(0);
        }

        if (characterList.Count == 1 && characterList[0] == player)
        {
            SceneManager.LoadScene(0);
        }
    }

    /*
     * This method runs when the Turn Over event is called
     */
    private void BeginNextTurn()
    {
        currentCharacterIndex++;

        if (currentCharacterIndex >= characterList.Count)
        {
            currentCharacterIndex = 0;
        }

        Debug.Log($"Current character index is: {currentCharacterIndex}");

        currentCharacter = characterList[currentCharacterIndex];

        virtualCamera.Follow = currentCharacter.transform;
        virtualCamera.LookAt = currentCharacter.transform;

        currentCharacter.StartTurn();

        Debug.Log("<color=green> New Turn Started for Character: " + currentCharacter.gameObject.name + "</color>");
    }

    /*
     *  This method is called when a character died
     */
    private void CharacterDied(Character character)
    {
        characterList.Remove(character);

        if (characterList.Count == 1)
        {
            SceneManager.LoadScene(0);
        }
    }


}
