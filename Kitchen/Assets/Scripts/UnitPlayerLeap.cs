using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using Leap.Unity;

public class UnitPlayerLeap: Unit {

    LeapProvider provider;
    //Controller controller;  

    // Use this for initialization
    public override void Start () {
        base.Start();
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        //controller = new Controller();        
    }
    float cameraRotY = 0f;
    // Update is called once per frame
    public override void Update() {
        
        //Frame frame = controller.Frame();
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                Debug.Log("Left hand");
                Debug.Log(hand.Direction);
                Debug.Log("Z " + hand.Direction.z);
                //Debug.Log("transform rotation " + transform.rotation.y);
                // character rotation
              
                //cameraRotY -= hand.Direction.z * turnSpeed * Time.deltaTime;
                //Debug.Log("Z " + cameraRotY);
                //cameraRotY = Mathf.Clamp(cameraRotY, -0.5f, 0.5f);
                transform.Rotate(0f, -hand.Direction.z * turnSpeed * Time.deltaTime, 0f);
                /*if (transform.rotation.y == 0.5f)
                {
                    transform.Rotate(0f, hand.Direction.z * turnSpeed * Time.deltaTime, 0f);
                } else
                {
                    transform.Rotate(0f, -hand.Direction.z * turnSpeed * Time.deltaTime, 0f);
                }
                    
                Debug.Log("cameraRotY " + cameraRotY);
                Debug.Log("transform rotation " + transform.rotation.y);*/
              

                // camera rotation        
                cameraRotX -= hand.Direction.y;
                //Debug.Log("X " + cameraRotX);
                cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
                Camera.main.transform.forward = transform.forward; // reset the camera view
                Camera.main.transform.Rotate(cameraRotX, 90f, 0f);
            }
            else
            {
                Debug.Log("Right hand");
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                //move = new Vector3(hand.Direction.y * 0.01f, normal.y * 0.01f, -hand.Direction.x * 0.01f);
                move = new Vector3(hand.Direction.y * 0.01f, normal.y * 0.01f, hand.Direction.z * 0.01f);
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