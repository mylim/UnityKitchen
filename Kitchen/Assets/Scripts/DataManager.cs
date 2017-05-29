using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        this.InterferenceVersion = 1;
    }

    public void setVRAISVersion(int VRAISVersion)
    {
        this.VRAISVersion = VRAISVersion;
        Debug.Log("VRAISVersion " + this.VRAISVersion);
    }

    public void setInterferenceVersion(int interferenceVersion)
    {
        this.InterferenceVersion = interferenceVersion;
        Debug.Log("InterferenceVersion " + this.InterferenceVersion);
    }

    public void LoadScene()
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(VRAISVersion);
    }
}
