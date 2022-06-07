using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStats Stats;
    public Transform MuzzlePoint;

    float refireTimer;
    private Projectile[] projectilePool;
    private ShotParameters shotParams;
    private Animator weaponAnim;

    private void InitPool()
    {
        projectilePool = new Projectile[Stats.BulletPoolSize];

        for (int z = 0; z < projectilePool.Length; z++)
        {
            GameObject newProj = Instantiate(Stats.ProjectileModel, transform);
            projectilePool[z] = newProj.GetComponent<Projectile>();
        }
    }

    private Projectile GetProjectileFromPool()
    {
        for (int z = 0; z < projectilePool.Length; z++)
            if (!projectilePool[z].ActiveStatus)
                return projectilePool[z];

        return null;
    }

    private Quaternion RandomWeaponSpread()
    {
        // random spread angles
        float sprX = Random.Range(-0.5f, 0.5f) * Stats.SpreadAngle;
        float sprY = Random.Range(-0.5f, 0.5f) * Stats.SpreadAngle;
        float sprZ = Random.Range(-0.5f, 0.5f) * Stats.SpreadAngle;

        return Quaternion.Euler(sprX, sprY, sprZ);
    }

    public bool TryShoot()
    {
        if (refireTimer > 0)
            return false;

        Projectile toShot = GetProjectileFromPool();
        if (!toShot)
            return false;

        if (weaponAnim)
            weaponAnim.SetTrigger("Shot");



        toShot.transform.SetPositionAndRotation(MuzzlePoint.position, MuzzlePoint.rotation * RandomWeaponSpread());
        toShot.Shot(shotParams);
        toShot.transform.SetParent(GameManager.Instance.MainContainer);
        refireTimer = Stats.RefireDelay;

        return true;
    }

    public void ReturnProjectile(Projectile _proj)
    {
        _proj.transform.SetParent(transform);
    }

    void Start()
    {
        refireTimer = 0;
        InitPool();

        // parameters for each shot
        shotParams = new ShotParameters(
            Stats.ProjectileSpeed,
            Stats.ProjectileLifetime,
            Stats.ProjectileDamage,
            this);

        weaponAnim = this.GetComponent<Animator>();
    }

    void Update()
    {
        refireTimer = Mathf.MoveTowards(refireTimer, 0, Time.deltaTime);
    }
}
