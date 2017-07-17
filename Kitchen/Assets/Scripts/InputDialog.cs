using UnityEngine;
using System.Collections;

public class InputDialog : MonoBehaviour {

    public GameObject dialog;
    // Use this for initialization
    public void ShowDialog()
    {
        dialog.SetActive(true);       
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);       
    }
}
