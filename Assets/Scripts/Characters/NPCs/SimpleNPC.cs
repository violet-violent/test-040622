using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPC : GameCharacter
{
    public NPCParameters Parameters;
    public GameObject Model;
    public Collider CharacterCollider;
    public ParticleSystem HitParticles;
    public Transform[] Waypoints;
    public EnemyWeapon UsedWeapon;

    private float hitPts;
    private int nextWpt;
    private int wptFollowDirection;
    private GameCharacter targetedPlayer;
    private bool isKilled;

    protected override void OnTriggerEnter(Collider other)
    {
        // nothing
    }

    protected override void Spawn()
    {
        IsPlayer = false;
        hitPts = Parameters.HitPoints;
        targetedPlayer = null;
        isKilled = false;

        if (Waypoints.Length == 0)
        {
            nextWpt = -1;
            wptFollowDirection = 0;
        }
        else
        {
            nextWpt = 0;
            wptFollowDirection = 1;
        }
    }

    public override void Damage(float _value)
    {
        hitPts -= _value;
        if (hitPts <= 0)
            Die();
    }

    public override void BulletHit(float _damage, Vector3 _impactPt)
    {
        Damage(_damage);

        // instantiating a hit particles (in case of multiple consecutive hits)
        if (HitParticles)
        {
            GameObject newHp = Instantiate(HitParticles.gameObject, transform);
            newHp.transform.position = _impactPt;
            newHp.GetComponent<ParticleSystem>().Play();
            Destroy(newHp, Parameters.DeletionDelay);
        }
    }

    protected override void Die()
    {
        // to prevent multiple executions
        if (isKilled)
            return;

        isKilled = true;
        GameManager.Instance.RecordKill(this.name);
        StartCoroutine(Death());
    }

    protected override void Live()
    {
        if (hitPts <= 0) return;

        FollowWaypoints();
        if (UsedWeapon)
        {
            AimAtPlayer();
            UsedWeapon.TryShoot();
        }
    }

    private void AimAtPlayer()
    {
        if (!targetedPlayer)
            targetedPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCharacter>();

        UsedWeapon.transform.LookAt(targetedPlayer.GetCentralPoint, Vector3.up);
    }

    private void ProceedToNextWpt()
    {
        nextWpt += wptFollowDirection;

        // last waypoint
        if ((wptFollowDirection == 1) && (nextWpt >= Waypoints.Length))
        {
            wptFollowDirection = -1;
            nextWpt = Waypoints.Length - 2;
        }
        else
        // last waypoint if going back
        if ((wptFollowDirection == -1) && (nextWpt < 0))
        {
            wptFollowDirection = 1;
            nextWpt = 1;
        }
    }

    private void FollowWaypoints()
    {
        if (wptFollowDirection == 0)
            return;

        Vector3 oldPos = transform.position;
        Vector3 newPos = Vector3.MoveTowards(oldPos, Waypoints[nextWpt].position, Parameters.MovementSpeed * Time.deltaTime);
        thisCtrl.Move(newPos - oldPos);

        // reached the waypoint
        if (Vector3.Distance (newPos, Waypoints[nextWpt].position) <= GameManager.Instance.NPCWaypointReachDist)
            ProceedToNextWpt();
    }

    IEnumerator Death()
    {
        Destroy(Model);
        CharacterCollider.enabled = false;

        // letting the particles play
        yield return new WaitForSeconds(Parameters.DeletionDelay);

        Destroy(this.gameObject);
    }
}
