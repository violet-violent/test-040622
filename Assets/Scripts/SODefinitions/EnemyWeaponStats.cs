using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeapon", menuName = "ScriptableObjects/Enemy Weapon Stats", order = 2)]
public class EnemyWeaponStats : ScriptableObject
{
    [Header("Main Tech Stats")]
    public string WeaponName;
    public float KickDistance;
    public float ProjectileSpeed;
    public float ProjectileLifetime;
    public float RefireDelay;
    [Header("Bullet Mechanics")]
    public GameObject ProjectileModel;
}
