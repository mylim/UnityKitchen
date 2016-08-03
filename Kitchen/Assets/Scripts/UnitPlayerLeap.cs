using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using Leap.Unity;

public class UnitPlayerLeap: Unit {

    LeapProvider provider;
    float leapUnityFactor = 0.01f;

    //Controller controller;  

    // Use this for initialization
    public override void Start () {
        base.Start();
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        //controller = new Controller();        
    }
    float cameraRotY = 0f;
    float dirZ = 0f;
    // Update is called once per frame
    public override void Update() {
        
        //Frame frame = controller.Frame();
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                Debug.Log("Right hand");
                Debug.Log(hand.Direction);
                Debug.Log("Z " + hand.Direction.z);
                transform.Rotate(0f, -hand.Direction.z * turnSpeed * Time.deltaTime, 0f);
            
                // camera rotation        
                cameraRotX -= hand.Direction.y;
                //Debug.Log("X " + cameraRotX);
                cameraRotX = Mathf.Clamp(cameraRotX, -cameraPitchMax, cameraPitchMax); // limit the angle of camera rotation
                Camera.main.transform.forward = transform.forward; // reset the camera view
                Camera.main.transform.Rotate(cameraRotX, 90f, 0f);
            }
            else
            {
                Debug.Log("Left hand");
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                move = new Vector3(hand.Direction.y * leapUnityFactor, normal.y * leapUnityFactor, hand.Direction.z * leapUnityFactor);
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

        base.Update();
	}
}