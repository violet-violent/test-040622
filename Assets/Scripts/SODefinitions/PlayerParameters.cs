using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "ScriptableObjects/Player Parameters", order = 1)]
public class PlayerParameters : ScriptableObject
{
    public float MovementSpeed;
    public GameObject DefaultWeapon;
}
