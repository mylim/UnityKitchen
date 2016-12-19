using UnityEngine;
using System.Collections;

public class PickupableTeaBag : MonoBehaviour {
    public Color itemColor;
    public GameObject singleTeaBag;
    private Color defaultColor;

    // Use this for initialization
    void Start()
    {
        defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
    }

    public void OnMouseEnter()
    {
        Debug.Log("Enter");
        Highlight(true);
    }

    public void OnMouseExit()
    {
        Debug.Log("Exit");
        Highlight(false);
    }

    private void Highlight(bool glow)
    {
        if (glow)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
        }
    }
}
