using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    public bool IsPlayer { get; protected set; }
    protected CharacterController thisCtrl;
    public Vector3 GetCentralPoint => thisCtrl.center + transform.position;

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void Spawn();
    public abstract void Damage(float _value);
    public abstract void BulletHit (float _damage, Vector3 _impactPt);
    protected abstract void Die();
    protected abstract void Live();

    // every character has a gravity effect on it
    void GravityEffect ()
    {
        if (thisCtrl && (!thisCtrl.isGrounded))
            thisCtrl.Move (Physics.gravity * Time.deltaTime);
    }   
    
    // activating the character controller - for all types of characters
    void Start()
    {
        // quick reference to avoid constant component polling
        thisCtrl = this.GetComponent<CharacterController>();

        if (!thisCtrl)
            Debug.LogError("No CharacterController found on " + this.name);

        // activating the controller because unity can't do that for some reason
        if (thisCtrl)
            thisCtrl.Move(transform.up * thisCtrl.minMoveDistance);

        // spawn method in inherited classes
        Spawn();
    }
    
    // for kicking a player and other possible forced movements
    public void ForceMove (Vector3 _move)
    {
        thisCtrl.Move(_move);
    }

    private void Update()
    {
        Live();
    }

    void LateUpdate()
    {
        GravityEffect();
    }
}
