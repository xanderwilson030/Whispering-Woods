using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Action", menuName = "New Combat Action")]
public class CombatActions : ScriptableObject
{
    public string DisplayName;
    public Type ActionType;

    [Header("Damage")]
    public int Damage;
    public GameObject ProjectilePrefab;

    [Header("Heal")]
    public int HealAmount;
}
