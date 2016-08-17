using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

public class LeapHandler : MonoBehaviour {

    LeapProvider provider;
    Frame frame;
    public float gravity = 20f;
    //Controller controller;

    Vector3 position;
    Vector3 target;
    GameObject grabbedObject;
    Vector3 grabbedObjectSize;

    Animator animator;
    bool open = false;

    // Use this for initialization
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        //controller = new Controller();
    }
    
    GameObject GetHandHoverObject(float range)
    {
        frame = provider.CurrentFrame;
        //Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                List<Finger> fingers = hand.Fingers;
                foreach (Finger finger in fingers)
                {
                    Finger.FingerType fingerType = finger.Type;
                    if (fingerType.Equals(Finger.FingerType.TYPE_INDEX))
                    {
                      
                        position = finger.TipPosition.ToVector3();                       
                        Debug.Log("Finger position " + position);
                      
                        RaycastHit rayCastHit;
                        target = finger.Direction.ToVector3() * range;                     
                        Debug.Log("Finger direction" + target);

                        /*if (Physics.Raycast(position, target, out rayCastHit, range))
                        {
                            Debug.Log("Hand hit object " + rayCastHit.collider.gameObject);
                            return rayCastHit.collider.gameObject;
                        }*/

                        // Open close doors by pointing fingers - hands don't need capsule colliders
                        if (Physics.Raycast(position, target, out rayCastHit, range) && rayCastHit.collider.isTrigger)
                        {
                            Debug.Log("Hand hit object " + rayCastHit.collider.gameObject);
                            animator = rayCastHit.transform.GetComponent<Animator>();
                            if (animator)
                            {
                                Debug.Log("Script found");
                                animator.SetBool("isOpened", !open);
                                open = !open;
                                //fridgeDoorAnim.PlayDoorAnim();
                            }
                            else
                            {
                                Debug.Log("Script not found");
                            }
                            return rayCastHit.collider.gameObject;
                        }
                    }
                }
            }           
        }
        return null;
    }

      void TryGrabObject(GameObject grabObject)
    {
        if (grabObject == null || !CanGrab(grabObject))
            return;
        grabbedObject = grabObject;
        //grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size;
        grabbedObjectSize = grabObject.GetComponent<Collider>().bounds.size;
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;

        // Drop the object onto a surface
        if (grabbedObject.GetComponent<Rigidbody>() != null)
        {
            grabbedObject.GetComponent<Rigidbody>().AddForce(-transform.up*gravity);
            grabbedObject.GetComponent<Rigidbody>().AddTorque(transform.forward);
        }
        /*if (grabbedObject.GetComponent<Rigidbody>() != null)
        {
            //grabbedObject.GetComponent<Rigidbody>().AddForce(transform.forward);
            //grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                Vector3 dropPosition = hit.point;
                grabbedObject.transform.position = dropPosition + grabbedObjectSize * 0.5f;
            }
        }*/
        grabbedObject = null;
    }

    bool CanGrab(GameObject candidate)
    {
        return candidate.GetComponent<Rigidbody>() != null;
    }

    bool isHandExtended()
    {
        int extendedFingers = 0;

        Frame frame = provider.CurrentFrame;
        //Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                List<Finger> fingers = hand.Fingers;
                foreach (Finger finger in fingers)
                {
                    if (finger.IsExtended)
                        extendedFingers++;
                }
                if (extendedFingers == 5)
                    return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

        GetHandHoverObject(1f);
        
        Debug.DrawRay(position, target, Color.red, 2f);

        // For grabbing object
        /*if (grabbedObject == null)
            TryGrabObject(GetHandHoverObject(1f));
        else if ((grabbedObject != null) && (!isHandExtended()))
            // do nothing
            ;
        else
            DropObject();     

        if (grabbedObject != null)
        {
            Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize.x;
            grabbedObject.transform.position = newPosition;
        }*/
    }
}
