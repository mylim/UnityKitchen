using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

public class LeapHandler : MonoBehaviour {
   
    Controller controller;
 
    Vector3 position;
    Vector3 target;
    GameObject grabbedObject;
    Vector3 grabbedObjectSize;

    // Use this for initialization
    void Start()
    {
        controller = new Controller();
    }
    
    GameObject GetHandHoverObject(float range)
    {
        Frame frame = controller.Frame();
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
                        //position = gameObject.transform.position;
                        position = finger.TipPosition.ToVector3() * 0.001f;
                        position.x = -position.x;
                        position.z = -position.z;
                        Debug.Log("Position " + position);
                        Debug.Log("Finger position" + finger.Direction.ToVector3() * 0.001f);
                        RaycastHit rayCastHit;
                        target = finger.Direction.ToVector3() * range;
                        target.x = -target.x;
                        target.z = -target.z;
                        Debug.Log("Finger direction" + target);
                        if (Physics.Raycast(position, target, out rayCastHit, range))
                        {
                            Debug.Log("Hand hit object " + rayCastHit.collider.gameObject);
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
        grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size;
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;
        if (grabbedObject.GetComponent<Rigidbody>() != null)
            grabbedObject.GetComponent<Rigidbody>().AddForce(transform.forward);
        //grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        grabbedObject = null;
    }

    bool CanGrab(GameObject candidate)
    {
        return candidate.GetComponent<Rigidbody>() != null;
    }

    bool isHandExtended()
    {
        int extendedFingers = 0;

        Frame frame = controller.Frame();
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
        Debug.DrawRay(position, target, Color.red, 2f);

        if (grabbedObject == null)
            TryGrabObject(GetHandHoverObject(10f));
        else if ((grabbedObject != null) && (!isHandExtended()))
            // do nothing
            ;
        else
            DropObject();     

        if (grabbedObject != null)
        {
            Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize.x;
            grabbedObject.transform.position = newPosition;
        }
    }
}
