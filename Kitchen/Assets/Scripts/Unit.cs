using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

[RequireComponent(typeof(CharacterController))]

public class Unit : MonoBehaviour
{
    protected CharacterController charControl;
    protected Vector3 move = Vector3.zero;
    public float moveSpeed = 1.0f;
    public float turnSpeed = 25.0f;
    public float cameraPitchMax = 30.0f;
    public float gravity = 20.0f;
    public float cameraRotX = 0.0f;

    /*public float samplingRate = 1f; // sample rate in Hz
    private System.IO.StreamWriter trajectoryFile;
    private string fileName;
    private string trajectory;*/

    // Use this for initialization
    public virtual void Start()
    {
        charControl = GetComponent<CharacterController>();

        if (!charControl)
        {
            Debug.LogError("Unit.Start(): " + name + " has no CharacterController");
            enabled = false;
        }
        if (charControl.isGrounded)
            Debug.Log("Character is grounded");

        /*fileName = System.DateTime.Today.ToString("yy-MM-dd") + "_" + WorldModelManager.VRAISVersion[DataManager.Instance.VRAISVersion]
            + "_" + WorldModelManager.InterferenceVersion[DataManager.Instance.InterferenceVersion] + "_P" + DataManager.Instance.ParticipantID + "_A" + DataManager.Instance.AssessmentNo;
        trajectory = "\nStart Position " + charControl.transform.position + " Start Direction " + charControl.transform.forward + "\n";
        InvokeRepeating("SampleNow", 0, 2.0f);*/
    }

    // Update is called once per frame
    public virtual void Update()
    {
        charControl.Move(move * moveSpeed);
    }

    /*public void SampleNow()
    {
        // cancelling out the initial camera rotation of 90 degrees
        trajectory = trajectory + System.DateTime.Now.ToLongTimeString() + " x " + -charControl.transform.position.x + " z " + charControl.transform.position.z + " fx " + charControl.transform.forward.z + " fz " + charControl.transform.forward.x +  "\n";
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
        //trajectoryFile.Write(SerializeObject(trajectory));
        //trajectoryFile.Close();       
    }

    void LoadXML()
    {
        StreamReader r = File.OpenText(fileName);
        string info = r.ReadToEnd();
        r.Close();
        trajectory = info;
        Debug.Log("File Read");
    }


    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    private string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(string));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    private object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(string));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }*/
}


