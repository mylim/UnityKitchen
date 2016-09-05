using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

public class InputHandler : MonoBehaviour {
    Animator animator;
    bool open = false;

    RaycastHit rayCastHit;
    GameObject grabbedObject;
    Vector3 grabbedObjectSize;
    Vector3 position;
    Vector3 target;

    /*Collider GetMouseHoverObject(float range)
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

    }*/

    GameObject GetMouseHoverObject(float range)
    {
     
        position = gameObject.transform.position;
        RaycastHit rayCastHit;
       
        target = position + Camera.main.transform.forward * range;
        if (Physics.Linecast(position, target, out rayCastHit))
            return rayCastHit.collider.gameObject;
        return null;
    }

    // Update is called once per frame
    void Update()
    {
       
        Debug.Log(GetMouseHoverObject(5));
        Debug.DrawRay(position, target);
                if (Input.GetMouseButtonDown(1))
                    {
                        if (grabbedObject == null)
                TryGrabObject(GetMouseHoverObject(5));
                        else
DropObject();
                    }
        
                if (grabbedObject != null)
                    {
            Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize.x;
            grabbedObject.transform.position = newPosition;
                    }
        /*if (Input.GetMouseButtonDown(1))
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
                } else { 
                
                    if (grabbedObject == null)
                        TryGrabObject(collider);
                    else
                        DropObject();

                    if (grabbedObject != null)
                    {
                        Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize;
                        grabbedObject.transform.position = newPosition;
                    }
                }
            }
        }*/
    }

    
    void TryGrabObject(GameObject grabObject)
    {
        if (grabObject == null || !CanGrab(grabObject))
            return;
        grabbedObject = grabObject;
        grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size;
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;
        if (grabbedObject.GetComponent<Rigidbody>() != null)
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject = null;
    }

    bool CanGrab(GameObject candidate)
    {
        return candidate.GetComponent<Rigidbody>() != null;
    }

    /*void TryGrabObject(Collider grabObject)
    {
        if (grabObject == null || !CanGrab(grabObject.gameObject))
            return;
        grabbedObject = grabObject.gameObject;
        grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size.magnitude;
        //grabbedObjectSize = grabObject.GetComponent<Collider>().bounds.size.magnitude;
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;
        if (grabbedObject.GetComponent<Rigidbody>() != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                Vector3 dropPosition = hit.point;
                grabbedObject.transform.position = dropPosition * grabbedObjectSize;
            }
        }
        //grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject = null;
    }

    bool CanGrab(GameObject candidate)
    {
        return candidate.GetComponent<Rigidbody>() != null;
    }*/
}
