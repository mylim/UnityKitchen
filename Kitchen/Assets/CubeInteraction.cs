using UnityEngine;
using System.Collections;

public class CubeInteraction : MonoBehaviour {

    public Color c;
    public static Color selectedColor;
    public bool selectable = false;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.transform.parent.name.Equals("index"))
        {
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
