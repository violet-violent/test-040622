using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    public PlayerParameters Parameters;
    public Transform Head;
    public Transform WeaponModelAttach;
    public Camera LookCamera;
    public bool IsPlayable => GetComponent<PlayerMovement>().enabled;

    private Weapon currentWeapon;

    protected override void OnTriggerEnter(Collider other)
    {
        // switch is too bulky to write it for two options
        if (other.gameObject.tag == "KillZone")
            Die();

        if (other.gameObject.tag == "Finish")
            GameManager.Instance.FinishLevel();
    }

    protected override void Spawn()
    {
        IsPlayer = true;
    }

    public override void Damage(float _value)
    {
        // player doesn't have any hit point values in this prototype
    }

    public override void BulletHit(float _damage, Vector3 _impactPt)
    {
        // player can't be hurt by enemy bullets
    }

    protected override void Live()
    {
        // all player's 'life' is controlled by keyboard/mouse
    }

    protected override void Die()
    {
        GameManager.Instance.EndGame();
    }

    public void TiltHead (float _delta)
    {
        // head rotations should be clamped to avoid rolling
        float clamping = GameManager.Instance.MaxPlayerHeadTilt;
        if (_delta > 0)
            clamping = -clamping;

        // rotate & lock unneeded axis
        Vector3 eulers = Head.localRotation.eulerAngles;
        eulers.x = Mathf.MoveTowardsAngle (eulers.x, clamping, Mathf.Abs(_delta));
        eulers.y = 0;
        eulers.z = 0;
        Head.localRotation = Quaternion.Euler(eulers);
    }

    private void DisableWeapon()
    {
        if (WeaponModelAttach.childCount > 0)
            Destroy(WeaponModelAttach.GetChild(0).gameObject);
    }

    private void PickUpWeapon (GameObject _weaponObj)
    {
        GameObject wpn = Instantiate(_weaponObj, WeaponModelAttach);
        currentWeapon = wpn.GetComponent<Weapon>();
    }

    public void EnablePlayer (bool _status)
    {
        GetComponent<PlayerMovement>().enabled = _status;
        LookCamera.enabled = _status;

        if (_status)
            PickUpWeapon(Parameters.DefaultWeapon);
        else
            DisableWeapon();
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0)) && currentWeapon)
            if (currentWeapon.TryShoot())
                GameManager.Instance.EventLogger.OnShot(currentWeapon.Stats.WeaponName);
    }
}
