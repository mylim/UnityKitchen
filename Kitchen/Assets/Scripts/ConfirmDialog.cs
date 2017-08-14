using UnityEngine;
using System.Collections;

public class ConfirmDialog : MonoBehaviour {
    public GameObject dialog;

    public void ShowDialog()
    {
        dialog.SetActive(true);       
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);       
    }
}
