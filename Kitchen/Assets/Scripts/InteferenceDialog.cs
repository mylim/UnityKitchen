using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteferenceDialog : MonoBehaviour {
    public GameObject dialog;
    private string answer;
 
    public void ShowDialog()
    {
        dialog.SetActive(true);
    }

    public void ShowAnswer()
    {
        answer = dialog.GetComponent<InputField>().text.ToString();
        Debug.Log("Answer " + answer);
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);
    }
}
