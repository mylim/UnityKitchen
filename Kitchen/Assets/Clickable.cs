using UnityEngine;
using System.Collections;

public class Clickable : MonoBehaviour {

    public GameObject gameObject;
    private Color defaultColor;
    private bool highlighted;

    // Use this for initialization
    void Start()
    {
        highlighted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseEnter()
    {
        float distance = Vector3.Distance(GameObject.FindWithTag("MainCamera").transform.position, gameObject.transform.position);
        if (distance <= 2)
        {
            Debug.Log("Enter");
            defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
            Highlight(true);
            highlighted = true;
        }
    }

    public void OnMouseExit()
    {
        if (highlighted)
        {
            Debug.Log("Exit");
            Highlight(false);
            highlighted = false;
        }
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
