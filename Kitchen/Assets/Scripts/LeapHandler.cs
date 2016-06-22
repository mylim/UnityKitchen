using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;

public class LeapHandler : MonoBehaviour {
    Animator animator;
    bool open = false;

    void OnTriggerEnter(Collider other)
    //void OnCollisionEnter(Collision other)
    {
        /*foreach (ContactPoint contact in other.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }*/

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
