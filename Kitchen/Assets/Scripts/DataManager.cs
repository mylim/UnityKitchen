using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour {

    /// Static reference to the instance of our DataManager
    public static DataManager Instance;

    private string VRAISVersion;
    private int interferenceVersion;
    public GameObject loadingImage;

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
    }

    public void setVRAISVersion(string VRAISVersion)
    {
        this.VRAISVersion = VRAISVersion;
    }

    public void setInterferenceVersion(int interferenceVersion)
    {
        this.interferenceVersion = interferenceVersion;
    }

    public void LoadScene()
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(VRAISVersion);
    }
}
