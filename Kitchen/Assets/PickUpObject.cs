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

    // Bin manipulation
    GameObject tiedBag;
    bool lidOn;
    Vector3 lidPosition;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        carrying = false;

        tiedBag = GameObject.FindWithTag("TiedBag");
        if (tiedBag)
        {
            Debug.Log("Tied Bag found");
            tiedBag.SetActive(false);
            lidOn = true;
        }
    }

    void FixedUpdate()
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
            Collider collider = GetMouseHoverObject(2);
            if (collider != null)
            {
                Debug.Log("In Pickup()");
                Pickupable p = collider.GetComponent<Pickupable>();
                if (p != null)
                {
                    if (collider.tag.Equals("BinLid"))
                    {
                        lidPosition = p.gameObject.transform.position;
                        lidOn = false;
                    }
                    Debug.Log("pickupable " + p.gameObject);
                    carrying = true;
                    carriedObject = p.gameObject;
                    //carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                    carriedObject.GetComponent<Rigidbody>().useGravity = false;
                   
                }
                else
                {
                    Debug.Log("Collider " + collider.gameObject.name);
                    if (!lidOn)
                    {
                        if (collider.tag.Equals("BinBag"))
                        {
                            Debug.Log("Bin Bag clicked");
                            collider.gameObject.SetActive(false);

                            if (tiedBag)
                            {
                                Debug.Log("Tied Bag found");
                                tiedBag.SetActive(true);
                            }
                        }
                    }
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
        /*if (carriedObject.tag.Equals("BinLid"))
        {
            carriedObject.transform.position = lidPosition;
        }*/
        carriedObject.GetComponent<Rigidbody>().useGravity = true;        
        carriedObject = null;
    }

    Collider GetMouseHoverObject(float range)
    {
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit rayCastHit;

        // Debug ray
        //Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 2f);
        //Debug.Log("Ray direction " + ray.direction.ToString());

        if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, range))
        {
            return rayCastHit.collider;
        }
        return null;

    }
}
