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
            //GetComponent<InterferenceDialog>().ShowDialog();
            Collider collider = GetComponent<MouseHoverObject>().GetMouseHoverObject(2);
            if (collider != null)
            {
                if (collider.tag.Equals(GetComponent<Collider>().tag))
                {                   
                    GetComponent<InterferenceDialog>().ShowDialog();
                }                
            }
        }
    }  
}
