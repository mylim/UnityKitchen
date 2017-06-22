using UnityEngine;
using System.Collections;

public class SaveDialog : MonoBehaviour {

    public GameObject dialog;

    public void ShowDialog()
    {       
        dialog.SetActive(true);
    }
    
    public void CloseDialog()
    {       
        dialog.SetActive(false);      
    }

    public void ExitApplication()
    {
        dialog.SetActive(false);
        Application.Quit();
    }
}
