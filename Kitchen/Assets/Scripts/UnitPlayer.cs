using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;

public class UnitPlayer : Unit {

    float cameraRotX = 0.0f;
    public float cameraPitchMax = 5.0f;

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

                /*Debug.Log("\nHand id: {0}, palm position: {1}, fingers: {2}" + " " + hand.Id + " " + hand.PalmPosition + " " + hand.Fingers.Count);
                // Get the hand's normal vector and direction
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                move = new Vector3(direction.x, 0f, direction.z);
                move.Normalize();
                move = transform.TransformDirection(move);
                // Calculate the hand's pitch, roll, and yaw angles
                Debug.Log("\nHand pitch: {0} degrees, roll: {1} degrees, yaw: {2} degrees" + " " +
                    direction.Pitch + " " + normal.Roll + " " + direction.Yaw);*/

                //List<Finger> fingers = hand.Fingers;
                //foreach (Finger finger in fingers)
                {
                    //if ((finger.Type == Finger.FingerType.TYPE_THUMB) && finger.IsExtended)
                    {

                    }
                    /*else if ((finger.Type == Finger.FingerType.TYPE_INDEX) && finger.IsExtended)
                    {
                        Debug.Log(finger.Direction);                     

                        // movement
                        move = new Vector3(finger.Direction.Pitch, 0f, finger.Direction.Yaw);
                        move.Normalize();
                        // transform the movement to the character's local orientation
                        move = transform.TransformDirection(move);
                    }*/
                } 
            }
            else
            {
                Debug.Log("Right hand");
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                move = new Vector3(hand.Direction.x, 0f, hand.Direction.y);
                //move.Normalize();
                move = transform.TransformDirection(move);
                Debug.Log(move.ToString());
                // Calculate the hand's pitch, roll, and yaw angles
                Debug.Log("\nHand pitch: {0} degrees, roll: {1} degrees, yaw: {2} degrees" + " " +
                    direction.x + " " + direction.y + " " + direction.z + " " +
                    normal.x + " " + normal.y + " " + normal.z + " " + hand.PalmVelocity);
            }
        }

       /* // rotate only when left mouse button is down
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

        // movement
        move = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        Debug.Log("Vertical " + Input.GetAxis("Vertical") + " Horizontal " + Input.GetAxis("Horizontal"));
        move.Normalize(); 
        // transform the movement to the character's local orientation
        move = transform.TransformDirection(move);*/

        base.Update();
	}
}