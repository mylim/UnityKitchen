using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour {

    /// Static reference to the instance of our DataManager
    public static DataManager Instance;
    public GameObject loadingImage;

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

        this.VRAISVersion = 0;
        this.InterferenceVersion = 0;
    }

    public void setVRAISVersion(int VRAISVersion)
    {
        if (VRAISVersion >= 0)
        {
            this.VRAISVersion = VRAISVersion;
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
        loadingImage.SetActive(true);
        SceneManager.LoadScene(VRAISVersion);
    }
}
