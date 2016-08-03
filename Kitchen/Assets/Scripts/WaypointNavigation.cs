using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using Leap.Unity;

public class WaypointNavigation: Unit {

    public GameObject[] checkpoints;
    int counter = 0;
    public float distance = 2.0f; //on which distance you want to switch to the next waypoint
    Vector3 direction;
    public Transform goal;
    NavMeshAgent agent;

    void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position;

    }

    void Update()
    {
        // rotate only when left mouse button is down
        if (Input.GetMouseButton(0))
        {
            // character rotation
            //Debug.Log("X " + Input.GetAxis("Mouse X"));
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
        }

        base.Update();
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