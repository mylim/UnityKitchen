using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterferenceDialog : MonoBehaviour {
    public GameObject dialog;
    private string answer;
    private bool interfering;
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
        answer = "";
        interfering = true;
        //Debug.Log("Interference dialog " + dialog.name + " set to " + interfering);
        dialog.SetActive(true);
        /*if (dialog.GetComponentInChildren<InputField>())
        {
            dialog.GetComponentInChildren<InputField>().ActivateInputField();
            dialog.GetComponentInChildren<InputField>().Select();
            dialog.GetComponentInChildren<InputField>().IsInteractable();

            Debug.Log("Answer field available");
        }*/
    }

    public void ShowAnswer()
    {
        answer = dialog.GetComponentInChildren<InputField>().text.ToString();
        Debug.Log("Answer " + answer);
    }

    public void CloseDialog()
    {
        interfering = false;
        dialog.SetActive(false);
        dialogClosed = true;
    }

    public bool DialogClosed()
    {
        return dialogClosed;
    }

    public string GetAnswer()
    {
        return answer;
    }

    public bool GetInterference()
    {
        return interfering;
    }
}
