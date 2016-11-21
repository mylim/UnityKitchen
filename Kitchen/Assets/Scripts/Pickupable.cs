﻿using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour {

    public GameObject gameObject;
    private Color defaultColor;
   
    // Use this for initialization
    void Start()
    {
        defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
    }

	// Update is called once per frame
	void Update () {
	
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

    public void OnMouseUp()
    {
        Debug.Log("Up");
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
