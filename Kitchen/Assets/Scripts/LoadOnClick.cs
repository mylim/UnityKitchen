using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    public GameObject loadingImage;

	public void LoadScene(int scene)
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(scene);
    }
}
