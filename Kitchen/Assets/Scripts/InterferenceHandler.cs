using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterferenceHandler : MonoBehaviour {

    void Update()
    {
       Interfere();    
    }

    void Interfere()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Collider collider = GetComponent<MouseHoverObject>().GetMouseHoverObject(2);
            if (collider != null)
            {
                if (collider.tag.Equals("Sink"))
                {                   
                    GetComponent<InteferenceDialog>().ShowDialog();
                }                
            }
        }
    }

  
}
