using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterferenceDialog : MonoBehaviour {
    public GameObject dialog;
    private string answer;
    public bool interfering;
    private int index;
    private bool dialogClosed;

    void Start()
    {
        index = 0;
        interfering = false;
        dialogClosed = false;
    }

    public void ShowDialog()
    {
        SetInterference();       
        //Debug.Log("Interference dialog " + dialog.name + " set to " + interfering);
        dialog.SetActive(true);                    
    }

    public void ShowAnswer()
    {
        answer = dialog.GetComponent<InputField>().text.ToString();
        Debug.Log("Answer " + answer);
    }

    public void CloseDialog()
    {
        ResetInterference();
        dialog.SetActive(false);
        dialogClosed = true;
    }

    public bool DialogClosed()
    {
        return dialogClosed;
    }

    public void SetInterference()
    {
        interfering = true;
        //Debug.Log("Interfering set to true dialog " + dialog.name);
    }

    public bool GetInterference()
    {
        return interfering;
    }

    public void ResetInterference()
    {
        // Debug.Log("Interfering set to false dialog " + dialog.name);
        interfering = false;
    }

}
