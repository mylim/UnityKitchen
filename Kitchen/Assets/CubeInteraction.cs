using UnityEngine;
using System.Collections;

public class CubeInteraction : MonoBehaviour {

    public Color c;
    public static Color selectedColor;
    public bool selectable = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision occurred");
        //Debug.Log("Collider " + other.gameObject.transform.parent.name);
        if (other.gameObject.transform.parent.name.Equals("index"))
        //if (other.gameObject.name.Equals("Player"))
        {
            Debug.Log("Collider " + other.gameObject.name);
            if (this.selectable)
            {
                CubeInteraction.selectedColor = this.c;
                this.transform.Rotate(Vector3.up, 33);
                return;
            }

            transform.gameObject.GetComponent<Renderer>().material.color = CubeInteraction.selectedColor;
        }
    }
}
