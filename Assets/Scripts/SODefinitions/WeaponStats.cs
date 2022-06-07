using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon Stats", order = 2)]
public class WeaponStats : ScriptableObject
{
    [Header("Main Tech Stats")]
    public string WeaponName;
    public int ProjectileDamage;
    public float ProjectileSpeed;
    public float ProjectileLifetime;
    public float RefireDelay;
    public float SpreadAngle;
    [Header("Bullet Mechanics")]
    public int BulletPoolSize;
    public GameObject ProjectileModel;
}
