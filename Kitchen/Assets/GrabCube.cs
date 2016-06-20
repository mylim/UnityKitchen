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
        listener = new SampleListener();
        controller.Connect += listener.OnServiceConnect;
        controller.Device += listener.OnConnect;
        controller.FrameReady += listener.OnFrame;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

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