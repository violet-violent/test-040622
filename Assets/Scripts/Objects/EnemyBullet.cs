using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile
{
    private void KickPlayer(GameCharacter _player)
    {
        _player.ForceMove (shot.damage * transform.forward);
    }

    protected override void HitEffect(RaycastHit _hitInfo)
    {
        // if the target is a player character - kick back
        if (_hitInfo.collider.gameObject.TryGetComponent(out GameCharacter gc) &&
                (gc.IsPlayer))
            KickPlayer(gc);
    }
}
