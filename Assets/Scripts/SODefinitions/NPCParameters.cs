using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCParams", menuName = "ScriptableObjects/NPC Parameters", order = 1)]
public class NPCParameters : ScriptableObject
{
    public string NPCType;
    public int HitPoints;
    public float MovementSpeed;
    public float DeletionDelay;
}
