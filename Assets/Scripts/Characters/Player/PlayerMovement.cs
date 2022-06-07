using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController thisCtrl;
    private PlayerCharacter thisPCharacter;
    private Vector3 mousePos;

    // keyboard input summary
    private Vector3 PollKeyboard()
    {
        Vector3 sumVector = new Vector3();

        if (Input.GetKey(KeyCode.W))
            sumVector += transform.forward;
        if (Input.GetKey(KeyCode.S))
            sumVector += -transform.forward;
        if (Input.GetKey(KeyCode.A))
            sumVector += -transform.right;
        if (Input.GetKey(KeyCode.D))
            sumVector += transform.right;

        return sumVector.normalized;
    }

    // mouse tracking and character rotations
    private void ProcessRotations()
    {
        Vector3 delta = Input.mousePosition - mousePos;
        mousePos = Input.mousePosition;

        // transform is being rotated only by y axis
        transform.Rotate (new Vector3 (0, delta.x * GameManager.Instance.MouseSensitivity, 0));

        // vertical aiming (x axis) is performed with player's head
        thisPCharacter.TiltHead (delta.y * GameManager.Instance.MouseSensitivity);
    }

    // in case of any extension, for running, jumping, etc.
    private Vector3 MovementVector()
    {
        return PollKeyboard() * thisPCharacter.Parameters.MovementSpeed * Time.deltaTime;
    }

    void Start()
    {
        // quick reference to avoid constant component polling
        thisCtrl = this.GetComponent<CharacterController>();
        thisPCharacter = this.GetComponent<PlayerCharacter>();
        // mouse position init
        mousePos = Input.mousePosition;
    }

    void LateUpdate()
    {
        ProcessRotations();
        thisCtrl.Move (MovementVector());
    }
}
