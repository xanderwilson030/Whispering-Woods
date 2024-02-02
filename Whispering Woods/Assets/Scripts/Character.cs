using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public int curHp;
    public int maxHp;
    public int currentActionCount;
    public int maxTurnActions;
    public int attackActionCost;

    [Header("Movement Details")]
    [SerializeField] protected GridTile startingPos;
    public float rayCastDistance;
    public LayerMask movementLayerMask;
    public GridTile currentTile;
    public bool canMove;

    [Header("Highlight Details")]
    [SerializeField] protected bool isHighlighted;
    [SerializeField] protected Light2D highLight;
    [SerializeField] protected int baseHighLightIntensity;
    [SerializeField] protected int selectedHighLightIntensity;

    [Header("Health Slider")]
    public Slider healthSlider;

    [Header("Particle FX")]
    [SerializeField] protected ParticleSystem damageParticles;

    [Header("Audio FX")]
    public AudioSource audioSource;
    public AudioClip[] clips;

    private void Awake()
    {
        if (startingPos != null)
        {
            currentTile = startingPos.GetComponent<GridTile>();
        }

        curHp = maxHp;
        healthSlider.maxValue = maxHp;
        healthSlider.value = curHp;
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

        currentActionCount++;

        if (currentActionCount >= maxTurnActions)
        {
            canMove = false;
            Debug.Log("<color=orange> Current character has exhausted their movement range </color>");
        }
    }
    public abstract void TakeCombatAction(CombatActions action, Character target);

    public abstract void OnMouseEnter();
    public abstract void OnMouseExit();

    public void TakeDamage(int damageToTake)
    {
        Debug.Log("Damage to take: " + damageToTake);
        curHp -= damageToTake;

        healthSlider.value = curHp;

        damageParticles.Play();
        audioSource.clip = clips[0];
        audioSource.Play();

        if (curHp <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
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
