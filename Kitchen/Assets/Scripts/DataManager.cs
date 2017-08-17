using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class DataManager : MonoBehaviour {

    /// Static reference to the instance of our DataManager
    public static DataManager Instance;
    public GameObject loadingImage;
    public InputDialog inputDialog;

    /// <summary>
    /// The VRAIS version number
    /// </summary>
    public int VRAISVersion
    {
        get;
        set;
    }

    /// <summary>
    /// The interference version
    /// </summary>
    public int InterferenceVersion
    {
        get;
        set;
    }

    /// <summary>
    /// The participant ID
    /// </summary>
    public string ParticipantID
    {
        get;
        set;
    }

    /// <summary>
    /// The Assessment Number
    /// </summary>
    public string AssessmentNo
    {
        get;
        set;
    }

    /// <summary>
    /// The assessment StartTime
    /// </summary>
    public DateTime StartTime
    {
        get;
        set;
    }

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        this.VRAISVersion = 1;
        this.InterferenceVersion = 0;
        this.ParticipantID = "";
        this.AssessmentNo = "";
        this.StartTime = System.DateTime.Now;
        //Debug.Log("Start time " + this.StartTime);
    }

    public void setVRAISVersion(int VRAISVersion)
    {
        if (VRAISVersion > 0)
        {
            // add 1 to account for the main menu
            this.VRAISVersion = VRAISVersion + 1;
        }
        Debug.Log("VRAISVersion " + this.VRAISVersion);
    }

    public void setInterferenceVersion(int interferenceVersion)
    {
        if (interferenceVersion >= 0)
        {
            this.InterferenceVersion = interferenceVersion;
        }
        Debug.Log("InterferenceVersion " + this.InterferenceVersion);
    }

    public void setParticipantID(string ParticipantID)
    {
        this.ParticipantID = ParticipantID;
        Debug.Log("Participant ID " + this.ParticipantID);
    }

    public void setAssessmentNo(string AssessmentNo)
    {
        this.AssessmentNo = AssessmentNo;
        Debug.Log("Assessment No " + this.AssessmentNo);
    }

    public void LoadScene()
    {
        if (ParticipantID.Equals("") || AssessmentNo.Equals(""))
        {
            Debug.Log("No data");
            inputDialog.ShowDialog();
        }
        else
        {
            loadingImage.SetActive(true);
            SceneManager.LoadScene(VRAISVersion); 
        }
    }
}
