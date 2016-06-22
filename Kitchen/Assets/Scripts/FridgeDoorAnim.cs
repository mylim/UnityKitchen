using UnityEngine;
using System.Collections;

public class FridgeDoorAnim : MonoBehaviour {
    Animator animator;
    bool open = false;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collider " + collider.gameObject.transform.parent.name);
        if (collider.gameObject.transform.parent.name.Equals("RigidRoundHand_R"))
        {
            Debug.Log("collider " + collider.ToString());
            animator = this.GetComponent<Animator>();
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
