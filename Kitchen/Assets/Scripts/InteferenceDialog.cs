using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteferenceDialog : MonoBehaviour {
    public GameObject dialog;
    public Text answerText;
    private string answer;
    private AudioSource source;
    public AudioClip sound;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ShowDialog()
    {
        dialog.SetActive(true);
        source.PlayOneShot(sound);
    }

    public void ShowAnswer()
    {
        answer = answerText.text.ToString();
        Debug.Log("Answer " + answer);
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);
    }
}
