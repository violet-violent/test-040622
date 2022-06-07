using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Projectile
{
    protected override void HitEffect(RaycastHit _hitInfo)
    {
        // if the target is an enemy character - do some damage and log it
        if (_hitInfo.collider.gameObject.TryGetComponent(out GameCharacter gc) &&
                (!gc.IsPlayer))
        {
            gc.BulletHit(shot.damage, _hitInfo.point);
            GameManager.Instance.EventLogger.OnEnemyHit(_hitInfo.collider.gameObject.name);
        }
    }
}
