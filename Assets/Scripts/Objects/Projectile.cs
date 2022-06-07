using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotParameters
{
    public float speed;
    public float lifetime;
    public float damage;
    public Weapon firedFrom;

    public ShotParameters (float _speed, float _lifetime, float _damage, Weapon _firedFrom)
    {
        speed = _speed;
        lifetime = _lifetime;
        damage = _damage;
        firedFrom = _firedFrom;
    }
}

public abstract class Projectile : MonoBehaviour
{
    public GameObject Model;
    public TrailRenderer Trail;
    public ParticleSystem ImpactParticles;
    public float PoolReturnDelay = 0.5f;
    public bool ActiveStatus { get; private set; }

    protected ShotParameters shot;
    private float lifetimeLeft;
    private bool impacted;

    protected abstract void HitEffect(RaycastHit _hitInfo);

    public void Shot (ShotParameters _params)
    {
        impacted = false;
        shot = _params;
        lifetimeLeft = shot.lifetime;
        SetActivity(true);
    }

    private void SetActivity (bool _status)
    {
        ActiveStatus = _status;
        Model.SetActive(_status);

        if (Trail)
        {
            Trail.Clear();
            Trail.emitting = _status;
        }

        // on inactivity: if this projectile is pooled, then return it to the pool, otherwise just delete it
        if (!_status)
        {
            if (shot.firedFrom)
                shot.firedFrom.ReturnProjectile(this);
            else
                Destroy(this.gameObject);
        }
    }

    // bullet hits are raycasted instead of relying on colliders
    // to avoid flying through objects on high speeds
    private bool TraceHit (float _distance, out RaycastHit _hitInfo)
    {
        return Physics.Raycast(transform.position, transform.forward, out _hitInfo, _distance);
    }

    IEnumerator ProcessHit(RaycastHit _hitInfo)
    {
        impacted = true;
        transform.position = _hitInfo.point;
        Model.SetActive(false);
        ImpactParticles.Play();

        if (Trail)
            Trail.Clear();

        HitEffect(_hitInfo);

        // returning to the pool is delayed to give some time to the particles
        yield return new WaitForSeconds(PoolReturnDelay);
        SetActivity(false);
    }

    private void Live()
    {
        if ((!ActiveStatus) || impacted)
            return;

        lifetimeLeft = Mathf.MoveTowards(lifetimeLeft, 0, Time.deltaTime);
        float coveredDistance = Time.deltaTime * shot.speed;

        if (TraceHit(coveredDistance, out RaycastHit _hitInfo))
            StartCoroutine (ProcessHit(_hitInfo));
        else
            transform.Translate(Vector3.forward * coveredDistance);

        if (lifetimeLeft <= 0)
            SetActivity(false);
    }

    private void Start()
    {
        if (Trail)
            Trail.emitting = false;
    }

    private void Update()
    {
        Live();
    }
}
