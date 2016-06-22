using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;

public class UnitPlayerLeap: Unit {

    Controller controller;  

    // Use this for initialization
    public override void Start () {
        base.Start();
        controller = new Controller();        
	}

    // Update is called once per frame
    public override void Update() {

        Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                Debug.Log("Left hand");
                Debug.Log(hand.Direction);
                // character rotation
                transform.Rotate(0f, hand.Direction.x * turnSpeed * Time.deltaTime, 0f);

                // camera rotation        
                cameraRotX -= hand.Direction.y;
                cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
                Camera.main.transform.forward = transform.forward; // reset the camera view
                Camera.main.transform.Rotate(cameraRotX, 90f, 0f);
            }
            else
            {
                Debug.Log("Right hand");
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                move = new Vector3(hand.Direction.y * 0.01f, normal.y * 0.01f, -hand.Direction.x * 0.01f);
                move.y -= gravity * Time.deltaTime;
                move.Normalize();
                move = transform.TransformDirection(move);
                //Debug.Log(move.ToString());
                // Calculate the hand's pitch, roll, and yaw angles
                //Debug.Log("\nHand pitch: {0} degrees, roll: {1} degrees, yaw: {2} degrees" + " " +
                //    direction.x + " " + direction.y + " " + direction.z + " " +
                //    normal.x + " " + normal.y + " " + normal.z + " " + hand.PalmVelocity);
            }
        }

        /*// rotate only when left mouse button is down
        if (Input.GetMouseButton(0))
        {
            // character rotation
            transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f);

            // camera rotation        
            cameraRotX -= Input.GetAxis("Mouse Y");
            cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
            Camera.main.transform.forward = transform.forward; // reset the camera view
            Camera.main.transform.Rotate(cameraRotX, 90f, 0f);
        }

        // movement using mouse
        move = new Vector3(Input.GetAxis("Mouse ScrollWheel"), 0f, 0f);
        move.y -= gravity * Time.deltaTime;
        Debug.Log("Vertical " + Input.GetAxis("Mouse ScrollWheel"));
        Debug.Log(move.ToString());
        move.Normalize(); 
        // transform the movement to the character's local orientation
        move = transform.TransformDirection(move);*/

        /*// movement
        move = new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal"));
        move.y -= gravity * Time.deltaTime;
        Debug.Log("Vertical " + Input.GetAxis("Vertical") + " Horizontal " + Input.GetAxis("Horizontal"));
        Debug.Log(move.ToString());
        move.Normalize();
        // transform the movement to the character's local orientation
        move = transform.TransformDirection(move);*/

        base.Update();
	}
}