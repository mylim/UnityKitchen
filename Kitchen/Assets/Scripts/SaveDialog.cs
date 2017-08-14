using UnityEngine;
using System.Collections;

public class SaveDialog : MonoBehaviour
{
    public GameObject dialog;
    private bool allowQuit;

    void Start()
    {
        allowQuit = false;
    }

    public void ShowDialog()
    {
        dialog.SetActive(true);
        allowQuit = true;
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);
        allowQuit = false;
    }

    public void ExitApplication()
    {
        dialog.SetActive(false);
        allowQuit = true;
        Application.Quit();
    }

    public bool AllowQuit()
    {
        return allowQuit;
    }
}
