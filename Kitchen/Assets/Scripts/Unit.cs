using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Unit : MonoBehaviour {
    
    protected CharacterController charControl;
    protected Vector3 move = Vector3.zero;
    public float moveSpeed = 0.05f;
    public float turnSpeed = 15f;

    // Use this for initialization
    public virtual void Start () {
        charControl = GetComponent<CharacterController>();

        if (!charControl) {
            Debug.LogError("Unit.Start(): " + name + " has no CharacterController");
            enabled = false;
        }
        if (charControl.isGrounded)
            Debug.Log("Character is grounded");
	}
	
	// Update is called once per frame
	public virtual void Update () {
        charControl.Move(move * moveSpeed);
	}
}
