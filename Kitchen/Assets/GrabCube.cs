using UnityEngine;
using System.Collections;
using Leap;
using System;

public class GrabCube : MonoBehaviour {
    

    Controller controller;
    SampleListener listener;

    // Use this for initialization
    void Start () {
        controller = new Controller();
        
        /*listener = new SampleListener();
        controller.Connect += listener.OnServiceConnect;
        controller.Device += listener.OnConnect;
        controller.FrameReady += listener.OnFrame;*/
    }

    GameObject GetMouseHoverObject(float range)
    {
        Vector3 position = gameObject.transform.position;
        RaycastHit rayCastHit;
        Vector3 target = position + Camera.main.transform.forward;
        if (Physics.Linecast(position, target, out rayCastHit))
            return rayCastHit.collider.gameObject;
        return null;
    }

    void Update()
    {
        Debug.Log(GetMouseHoverObject(5));
    }

    /*void UpdatePinch(Frame frame)
    {
        bool trigger_pinch = false;
        Hand hand = frame.Hands[handIndex];
	    // Thumb tip is the pinch position.
        Vector3 thumb_tip = hand.Fingers[0].TipPosition.ToUnityScaled();
     
        // Check thumb tip distance to joints on all other fingers.
        // If it's close enough, start pinching.

        for (int i = 1; i < NUM_FINGERS && !trigger_pinch; ++i)
        {
            Finger finger = hand.Fingers[i];

            for (int j = 0; j < NUM_JOINTS && !trigger_pinch; ++j)
            {
                Vector3 joint_position = finger.JointPosition((Finger.FingerJoint)(j)).ToUnityScaled();

                Vector3 distance = thumb_tip - joint_position;

                if (distance.magnitude < THUMB_TRIGGER_DISTANCE)

                    trigger_pinch = true;
            }           
        }

	    // Only change state if it's different.

        if (trigger_pinch && !pinching_)
          OnPinch(pinch_position);
    }

    void OnPinch(Vector3 pinch_position)
    {
        // ...
        // Check if we pinched a movable object and grab the closest one.
        Collider[] close_things = Physics.OverlapSphere(pinch_position, PINCH_DISTANCE, layer_mask_);
        Vector3 distance = new Vector3(PINCH_DISTANCE, 0.0f, 0.0f);
        for (int j = 0; j < close_things.Length; ++j)
        {
            Vector3 new_distance = pinch_position - close_things[j].transform.position;
            if (close_things[j].rigidbody != null && new_distance.magnitude < distance.magnitude)
            {
                grabbed_object_ = close_things[j];
                distance = new_distance;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        // Accelerate what we are grabbing toward the pinch.
        if (grabbed_ != null)
        {
            Vector3 distance = pinch_position - grabbed_.transform.position;
            grabbed_object_.rigidbody.AddForce(SPRING_CONSTANT * distance);
        }
    }*/

    void Stop ()
    {
        controller.Dispose();
    }
}

class SampleListener
{
    public void OnServiceConnect(object sender, ConnectionEventArgs args)
    {
        Debug.Log("Service Connected");
    }

    public void OnConnect(object sender, DeviceEventArgs args)
    {
        Debug.Log("Connected");
    }

    public void OnFrame(object sender, FrameEventArgs args)
    {
        // Get the most recent frame and report some basic information
        Frame frame = args.frame;

        Debug.Log("Frame id: {0}, timestamp: {1}, hands: {2} " + frame.Id + " " + frame.Timestamp + " " + frame.Hands.Count);
        foreach (Hand hand in frame.Hands)
        {
            Debug.Log("\nHand id: {0}, palm position: {1}, fingers: {2}" + " " +  hand.Id + " " + hand.PalmPosition + " " + hand.Fingers.Count);
            // Get the hand's normal vector and direction
            Vector normal = hand.PalmNormal;
            Vector direction = hand.Direction;

            // Calculate the hand's pitch, roll, and yaw angles
            Debug.Log("\nHand pitch: {0} degrees, roll: {1} degrees, yaw: {2} degrees" + " " + 
                direction.Pitch * 180.0f / (float)Math.PI + " " + normal.Roll * 180.0f / (float)Math.PI + " " + direction.Yaw * 180.0f / (float)Math.PI);
        }
    }
}