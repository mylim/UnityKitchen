using UnityEngine;
using System.Collections;
using Leap;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Xml;

public class UnitPlayerMouse: Unit {

    public float samplingRate = 1f; // sample rate in Hz
    private System.IO.StreamWriter trajectoryFile;
    private string fileName;
    private string trajectory;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        fileName = System.DateTime.Today.ToString("yy-MM-dd") + "_" + WorldModelManager.VRAISVersion[DataManager.Instance.VRAISVersion]
               + "_" + WorldModelManager.InterferenceVersion[DataManager.Instance.InterferenceVersion] + "_P" + DataManager.Instance.ParticipantID + "_A" + DataManager.Instance.AssessmentNo;
        // cancelling out the initial camera rotation of 90 degrees and -mouse z movement
        trajectory = "\n" + System.DateTime.Now.ToLongTimeString() + " Start Position x " + charControl.transform.position.x + " z " + -charControl.transform.position.z + " fx " + charControl.transform.forward.z + " fz " + charControl.transform.forward.x + "\n";
        InvokeRepeating("SampleNow", 0, 2.0f);
    }
    

    // Update is called once per frame
    public override void Update() {

        // rotate only when left mouse button is down
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
        move = new Vector3(Input.GetAxis("Mouse ScrollWheel"), 0f, -Input.GetAxis("Horizontal"));
        move.y -= gravity * Time.deltaTime;
        //Debug.Log("Vertical " + Input.GetAxis("Mouse ScrollWheel"));
        //Debug.Log(move.ToString());
        move.Normalize(); 
        // transform the movement to the character's local orientation
        move = transform.TransformDirection(move);

        base.Update();
	}

    public void SampleNow()
    {
        /*trajectoryFile.WriteLine("t {0} x {1} z {2} fx {3} fz {4}",
          System.DateTime.Now.ToLongTimeString(), charControl.transform.position.x, charControl.transform.position.z, charControl.transform.forward.x, charControl.transform.forward.z);         */
        // cancelling out the initial camera rotation of 90 degrees and -mouse z movement
        trajectory = trajectory + System.DateTime.Now.ToLongTimeString() + " x " + charControl.transform.position.x + " z " + -charControl.transform.position.z + " fx " + charControl.transform.forward.z + " fz " + charControl.transform.forward.x + "\n";
    }

    public virtual void OnApplicationQuit()
    {
        CancelInvoke();
        CreateXML();
    }

    private void CreateXML()
    {
        trajectoryFile = System.IO.File.AppendText(@".\Logs\" + fileName + "_trajectory.txt");
        XmlSerializer xs = new XmlSerializer(typeof(string));
        xs.Serialize(trajectoryFile, trajectory);
        trajectoryFile.Close();    
    }

    void LoadXML()
    {
        StreamReader r = File.OpenText(fileName);
        string info = r.ReadToEnd();
        r.Close();
        trajectory = info;
        Debug.Log("File Read");
    }
}