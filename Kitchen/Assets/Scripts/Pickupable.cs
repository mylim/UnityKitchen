using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

    public GameObject gameObject;
    private Color defaultColor;
    private Vector3 originalPosition;
    private bool highlighted;
   
    // Use this for initialization
    void Start()
    {
        originalPosition = gameObject.transform.position;
        highlighted = false;
        if (gameObject == GameObject.FindWithTag("TiedBag"))
        {
            Debug.Log("Tied Bag found");
            gameObject.SetActive(false);         
        }
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

    public Vector3 getOriginalPosition()
    {
        return originalPosition;
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
