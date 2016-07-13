using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using Leap.Unity;

public class UnitPlayerWaypointsNavigation : MonoBehaviour {

    LeapProvider provider;
    float leapUnityFactor = 0.01f;
    Vector3 position;
    Vector3 target;
    public static int NAVIGATE = 0;
    public static int INTERACT = 1;
    private int interactionMode;

    Frame frame;
    //Controller controller;

    GameObject grabbedObject;
    Vector3 grabbedObjectSize;

    public ArrayList waypointsList = new ArrayList();
    public GameObject[] waypoints;
    NavMeshAgent agent;
    public Transform goal;


    // Use this for initialization
    public void Start()
    {
        //base.Start();
        //waypointsList = new List<String>();
        waypointsList.Add("Fridge");
        waypointsList.Add("Stove");
        waypointsList.Add("Cupboard");
        waypointsList.Add("Shelf");
        waypointsList.Add("Sink");
        waypointsList.Add("Table");

        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        interactionMode = NAVIGATE;
        //turnSpeed = 5.0f;
        //moveSpeed = 0.01f;
        agent = GetComponent<NavMeshAgent>();
    }    

    // Update is called once per frame
    public void Update() {

        Frame frame = provider.CurrentFrame;
        GameObject hitObject = null;

        //Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            /*if (hand.IsRight)
            {
                //if (hand.PinchStrength > 0.5f)
                if (!isHandExtended())
                {
                    if (interactionMode == INTERACT)
                        interactionMode = NAVIGATE;
                    else
                        interactionMode = INTERACT;
                }
            }*/

            if (interactionMode == NAVIGATE) {
                if (hand.IsRight)
                {
                    List<Finger> fingers = hand.Fingers;
                    foreach (Finger finger in fingers)
                    {
                        /*Finger.FingerType fingerType = finger.Type;
                            if (fingerType.Equals(Finger.FingerType.TYPE_INDEX))
                            {

                                position = finger.TipPosition.ToVector3();
                                Debug.Log("Finger position " + position);

                                RaycastHit rayCastHit;
                                target = finger.Direction.ToVector3() * 5.0f;
                                Debug.Log("Finger direction" + target);

                                if (Physics.Raycast(position, target, out rayCastHit, 5.0f))
                                {
                                    Debug.Log("Hand hit object " + rayCastHit.collider.gameObject);
                                    //Vector3 grabbedObjectSize = rayCastHit.collider.gameObject.GetComponent<Renderer>().bounds.size;

                                    // movement
                                    //move = new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal"));
                                    move = rayCastHit.point; //- grabbedObjectSize/2;
                                    move.y -= gravity * Time.deltaTime;
                                    //Debug.Log("Vertical " + Input.GetAxis("Vertical") + " Horizontal " + Input.GetAxis("Horizontal"));
                                    //Debug.Log(move.ToString());
                                    move.Normalize();
                                    // transform the movement to the character's local orientation
                                    move = transform.TransformDirection(move);
                                }
                            }*/


                        Finger.FingerType fingerType = finger.Type;
                        if (fingerType.Equals(Finger.FingerType.TYPE_INDEX))
                        { 
                            position = finger.TipPosition.ToVector3();
                            Debug.Log("Finger position " + position);

                            RaycastHit rayCastHit;
                            target = finger.Direction.ToVector3() * 2f;
                            Debug.Log("Finger direction" + target);

                            if (Physics.Raycast(position, target, out rayCastHit, 2f))
                            {
                                hitObject = rayCastHit.collider.gameObject;
                                if (hitObject != null)
                                {
                                    Debug.DrawRay(position, target, Color.green, 2f);
                                    //Debug.Log("hitObject tag " + rayCastHit.collider.gameObject.tag.ToString() + "Waypoints " + waypointsList[0].ToString());

                                    for (int i = 0; i < waypoints.Length; i++)
                                    {
                                        Debug.Log("hitObject: " + hitObject.name);
                                        Debug.Log("Waypoint: " + waypoints[i].tag);

                                        if (Equals(hitObject.name, waypoints[i].tag))
                                        {
                                            Debug.Log("hitObject: " + hitObject.name + ", Waypoint: " + waypoints[i].tag);
                                            goal.position = waypoints[i].transform.position;
                                            agent.destination = goal.position;
                                            /*move = waypoints[i].transform.position;
                                            move.y -= gravity * Time.deltaTime;

                                            //Debug.Log(move.ToString());
                                            move.Normalize();
                                            // transform the movement to the character's local orientation
                                            move = transform.TransformDirection(move);
                                            hitObject = null;*/
                                        }
                                    }
                                }
                            }
                            else if (hitObject == null || Equals(hitObject.name, "Room"))
                            {
                                transform.Rotate(0f, -finger.Direction.z * 0.5f * Time.deltaTime, 0f);

                                // camera rotation        
                                /*cameraRotX -= finger.Direction.y;
                                //Debug.Log("X " + cameraRotX);
                                cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
                                Camera.main.transform.forward = transform.forward; // reset the camera view
                                Camera.main.transform.Rotate(cameraRotX, 90f, 0f);*/
                            }
                        }
                    }
                }
                /*else
                {
                    Debug.Log("Left hand");
                    Vector normal = hand.PalmNormal;
                    Vector direction = hand.Direction;

                    move = new Vector3(hand.Direction.y * leapUnityFactor, normal.y * leapUnityFactor, hand.Direction.z * leapUnityFactor);
                    move.y -= gravity * Time.deltaTime;
                    move.Normalize();
                    move = transform.TransformDirection(move);
                }*/
            }
            else if (interactionMode == INTERACT)
            {
                GetHandHoverObject(1f);

                Debug.DrawRay(position, target, Color.red, 2f);

                if (grabbedObject == null)
                    TryGrabObject(GetHandHoverObject(1f));
                else if ((grabbedObject != null) && (isGrabbing()))
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


        /*// movement
        move = new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal"));           
        move.y -= gravity * Time.deltaTime;
        //Debug.Log("Vertical " + Input.GetAxis("Vertical") + " Horizontal " + Input.GetAxis("Horizontal"));
        //Debug.Log(move.ToString());
        move.Normalize();
        // transform the movement to the character's local orientation
        move = transform.TransformDirection(move);*/

        //base.Update();
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

                        if (Physics.Raycast(position, target, out rayCastHit, range))
                        {
                            Debug.Log("Hand hit object " + rayCastHit.collider.gameObject);
                            return rayCastHit.collider.gameObject;
                        }

                        // Open close doors by pointing fingers - hands don't need capsule colliders
                        /*if (Physics.Raycast(position, target, out rayCastHit, range) && rayCastHit.collider.isTrigger)
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
                        }*/
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
        //grabbedObjectSize = grabObject.GetComponent<Collider>().bounds.size;
    }

    void DropObject()
    {
        if (grabbedObject == null)
            return;

        // Drop the object onto a surface
        if (grabbedObject.GetComponent<Rigidbody>() != null)
        {
            grabbedObject.GetComponent<Rigidbody>().AddForce(-transform.up * 20f);
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

    bool isGrabbing()
    {
        Frame frame = provider.CurrentFrame;
        //Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                if (hand.GrabStrength > 0.5f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /*void FixedUpdate()
    {
        direction = Vector3.zero;
        //get the vector from your position to current waypoint
        direction = checkpoints[counter].transform.position - transform.position;
        //check our distance to the current waypoint, Are we near enough?
        if (direction.magnitude < distance)
        {
            if (counter < checkpoints.Length - 1) //switch to the nex waypoint if exists
            {
                counter++;
            }
            else //begin from new if we are already on the last waypoint
            {
                counter = 0;
            }
        }
        direction = direction.normalized;
        Vector3 dir = direction;
        move = transform.TransformDirection(direction);
        base.Update();
    }*/
}