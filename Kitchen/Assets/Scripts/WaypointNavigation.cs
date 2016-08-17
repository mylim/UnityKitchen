using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using Leap.Unity;

public class WaypointNavigation: Unit {

    public GameObject[] waypoints;
    int counter = 0;
    public float distance = 2.0f; //on which distance you want to switch to the next waypoint
    Vector3 direction;
    //public Transform goal;
    NavMeshAgent agent;
    Vector3 vRotation;
    Quaternion qRotation;

    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        counter = 0;

        /*Vector3 rotation = goal.position - transform.position;
        //rotation.y = 0f;
        bool turned = false;
        // Do we need to adjust facing?
        Quaternion idealFacing = Quaternion.LookRotation(rotation);
        // This is how far we would like to turn
        float angle = Quaternion.Angle(transform.rotation, idealFacing);
        // This is most we are allowed to turn this frame
        float maxTurn = 25 * Time.deltaTime;
        if (maxTurn >= angle)
        {
            // Excellent. We can just face the target
            transform.rotation = idealFacing;
        }
        else {
            // We'll have to take this more gradually then; use slerp to smoothly face the target.
            transform.rotation = Quaternion.Slerp(transform.rotation, idealFacing, maxTurn / angle);
            turned = true;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, idealFacing, maxTurn / angle);
        turned = true;

        if (turned)
        {
            agent.destination = goal.position;
        }*/
        

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //System.Random rnd = new System.Random();
            //counter = rnd.Next(0, waypoints.Length);

            //Debug.Log("hitObject: " + hitObject.name);
            Debug.Log("Waypoint: " + waypoints[counter].name);
            Vector3 direction = waypoints[counter].transform.position - transform.position;
            Debug.Log("Direction: " + direction);
            //if (Equals(hitObject.name, waypoints[i].tag))
            {
                //Debug.Log("hitObject: " + hitObject.name + ", Waypoint: " + waypoints[i].tag);
                //goal.position = waypoints[i].transform.position;
                RotateTowards(waypoints[counter].transform);
                MoveTowards(waypoints[counter].transform);

                agent.destination = waypoints[counter].transform.position;

                counter++;
                Debug.Log("counter: " + counter);
                if (counter >= waypoints.Length)
                {
                    counter = 0;
                }

                /*move = waypoints[i].transform.position;
                move.y -= gravity * Time.deltaTime;

                //Debug.Log(move.ToString());
                move.Normalize();
                // transform the movement to the character's local orientation
                move = transform.TransformDirection(move);
                hitObject = null;*/
            }
        }

        /*for (int i = 0; i < waypoints.Length; i++)
        {
           // Debug.Log("hitObject: " + hitObject.name);
            Debug.Log("Waypoint: " + waypoints[i].name);
            agent.transform.Rotate(waypoints[i].transform.rotation * Vector3.forward);
            agent.destination = waypoints[i].transform.position;
        }*/
        // rotate only when left mouse button is down
        /*if (Input.GetMouseButton(0))
        {
            // character rotation
            Debug.Log("X " + Input.GetAxis("Mouse X"));
            transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f);

            // camera rotation        
            cameraRotX -= Input.GetAxis("Mouse Y");
            //Debug.Log("Y " + Input.GetAxis("Mouse Y"));
            cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
            Camera.main.transform.forward = transform.forward; // reset the camera view
            Camera.main.transform.Rotate(cameraRotX, 90f, 0f);
        }

        if (Input.GetMouseButtonDown(2))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 2))
            {
              
                agent.destination = hit.point;
                Debug.Log("Hit target " + hit.collider.gameObject.name);
            }
        }*/

        base.Update();
    }

    private void MoveTowards(Transform target)
    {
        agent.SetDestination(target.position);
    }

    private void RotateTowards(Transform target)
    {
        //Debug.Log("vRotation before " + vRotation);
        //Debug.Log("transform before " + transform.rotation.eulerAngles);
        //transform.Rotate(0f, -vRotation.y, 0f);
        //Debug.Log("transform after " + transform.rotation.eulerAngles);
        qRotation = target.transform.rotation;
        vRotation = qRotation.eulerAngles;
        Debug.Log("vRotation after " + vRotation);
        //transform.Rotate(0f, 0f, 0f);
        
        //transform.Rotate(0f, vRotation.y, 0f);
        Camera.main.transform.forward = transform.forward; // reset the camera view
        Camera.main.transform.Rotate(vRotation.x, 90f + vRotation.y, 0f);

        /*float ang;
        Vector3 rot;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);*/
    }

    /*void FixedUpdate()
    {
        direction = Vector3.zero;
        //get the vector from your position to current waypoint
        direction = checkpoints[counter].transform.position - transform.position;
        //check our distance to the current waypoint, Are we near enough?
        if (direction.magnitude < distance)
        {
            if (counter < checkpoints.Length - 1) //switch to the next waypoint if exists
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
        //GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * actualSpeed, direction.y * actualSpeed);
    }*/
}