using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

/**
Open or close doors using hands - hands have capsule colliders and are triggers
*/

public class LeapOpenCloseHandler : MonoBehaviour {
    Animator animator;
    bool open = false;

    void OnTriggerEnter(Collider other)
    //void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collider " + other.gameObject.name);
        //if (other.gameObject.transform.parent.name.Equals("RigidRoundHand_R"))
        {
            Debug.Log("collider " + other.ToString());
            animator = other.GetComponent<Animator>();
            if (animator)
            {
                Debug.Log("Script found");
                animator.SetBool("isOpened", !open);
                open = !open;
            }
            else
            {
                Debug.Log("Script not found");
            }
        }
    }
}
