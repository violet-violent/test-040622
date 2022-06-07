using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public EnemyWeaponStats Stats;
    public Transform MuzzlePoint;

    private ShotParameters shotParams;
    private float refireDelay;

    public void TryShoot()
    {
        if (refireDelay > 0)
            return;

        GameObject bullet = Instantiate(Stats.ProjectileModel, GameManager.Instance.MainContainer);
        bullet.transform.SetPositionAndRotation(MuzzlePoint.position, MuzzlePoint.rotation);
        Projectile proj = bullet.GetComponent<Projectile>();
        proj.Shot(shotParams);

        refireDelay = Stats.RefireDelay;
    }

    private void Update()
    {
        refireDelay = Mathf.MoveTowards(refireDelay, 0, Time.deltaTime);
    }

    private void Start()
    {
        refireDelay = 0;

        shotParams = new ShotParameters(Stats.ProjectileSpeed,
            Stats.ProjectileLifetime,
            Stats.KickDistance,
            null);
    }
}
