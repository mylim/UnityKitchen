using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

/** 
Handles the opening and closing doors
*/

public class InputHandler : MonoBehaviour {
    Animator animator;
    bool open = false;

    RaycastHit rayCastHit;
    GameObject grabbedObject;
    Vector3 grabbedObjectSize;
    Vector3 position;
    Vector3 target;

    Collider GetMouseHoverObject(float range)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Debug ray
        Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 2f);
        //Debug.Log("Ray direction " + ray.direction.ToString());

        if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, range))
        {
            return rayCastHit.collider;
        }
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Collider collider = GetMouseHoverObject(2);

            if (collider != null)
            {
                if (collider.isTrigger)
                {
                    animator = rayCastHit.collider.GetComponent<Animator>();
                    if (animator)
                    {
                        Debug.Log("Script found");
                        animator.SetBool("isOpened", !open);
                        open = !open;
                        //fridgeDoorAnim.PlayDoorAnim();
                    }
                }
            }
        }
    }
}
