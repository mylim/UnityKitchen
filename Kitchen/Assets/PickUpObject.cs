using UnityEngine;
using System.Collections;

/** 
Handles objects pickup and dropping
*/

public class PickUpObject : MonoBehaviour {

    GameObject mainCamera;
    bool carrying;
    GameObject carriedObject;

    public float distance;
    public float smooth;
    Animator animator;
    bool open = false;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        carrying = false;
    }

    void Update()
    {      
        if (carrying)
        {
            Carry(carriedObject);
            CheckDrop();
        }
        else
        {
            Pickup();
        }      
    }

    void Carry(GameObject cObject)
    {        
        cObject.transform.position = Vector3.Lerp(cObject.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        //cObject.transform.rotation = Quaternion.identity;
    }

    void Pickup()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //int x = Screen.width / 2;
            //int y = Screen.height / 2;
            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastHit;
            // Debug ray
            Debug.DrawRay(ray.origin, ray.direction * 2, Color.green, 2f);
            Debug.Log("Ray direction " + ray.direction.ToString());

            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, 2f))
            {
                Pickupable p = rayCastHit.collider.GetComponent<Pickupable>();
                if (p != null)
                {
                    Debug.Log("pickupable " + p.gameObject);
                    carrying = true;
                    carriedObject = p.gameObject;
                    //cObject.GetComponent<Rigidbody>().isKinematic = true;
                    carriedObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }    
    }

    void CheckDrop()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DropObject();
        }
    }

    void DropObject()
    {
        carrying = false;
        //carriedObject.GetComponent<Rigidbody>().AddForce(-transform.up * 20f);
        //carriedObject.GetComponent<Rigidbody>().AddTorque(transform.forward);
        //carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}
