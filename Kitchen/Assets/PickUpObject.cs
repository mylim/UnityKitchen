using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/** 
Handles objects pickup and dropping
*/

public class PickUpObject : MonoBehaviour
{
    private GameObject mainCamera;
    private RaycastHit rayCastHit;

    // Object being carried
    private bool carrying;
    private GameObject carriedObject;
    public float distance;
    public float smooth;

    // Bin manipulation
    private GameObject binLid;
    private GameObject bin;
    private GameObject singleItem;
    private GameObject tiedBag;
    private GameObject binBag;
    private bool lidOn;
    private bool binEmpty;
    private bool newBag;
    private Vector3 lidPosition;
    private Vector3 binPosition;
    private Vector3 binBagPosition;
    private Vector3 tiedBagPosition;

    // Sound
    public AudioClip dropSound;
    private AudioSource source;

    private GameObject kettle;
    private GameObject singleTeaBag;
    private bool teaBagIn;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        carrying = false;

        if (GameObject.FindWithTag("BinLid"))
        {
            binLid = GameObject.FindWithTag("BinLid");
            lidOn = true;
        }

        if (GameObject.FindWithTag("Bin"))
        {
            bin = GameObject.FindWithTag("Bin");
        }

        if (GameObject.FindWithTag("SingleItem"))
        {
            singleItem = GameObject.FindWithTag("SingleItem");
            singleItem.SetActive(false);
        }

        if (GameObject.FindWithTag("SingleTeaBag"))
        {
            singleTeaBag = GameObject.FindWithTag("SingleTeaBag");
            singleTeaBag.SetActive(false);
            teaBagIn = false;
        }

        if (GameObject.FindWithTag("BinBag"))
        {
            binBag = GameObject.FindWithTag("BinBag");
            newBag = false;
        }

        if (GameObject.FindWithTag("TiedBag"))
        {
            Debug.Log("Tied Bag found");
            tiedBag = GameObject.FindWithTag("TiedBag");     
        }

        if (GameObject.FindWithTag("Kettle"))
        {
            Debug.Log("Kettle");
            kettle = GameObject.FindWithTag("Kettle");
        }


        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (carrying)
        {
            Debug.Log("Carrying");
            Carry(carriedObject);
            CheckDrop();
        }
        else
        {
            Debug.Log("Pickup");
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
            Collider collider = GetComponent<MouseHoverObject>().GetMouseHoverObject(2);
            if (collider != null)
            {
                Debug.Log("In Pickup()");

                if (collider.GetComponent<Pickupable>())
                {
                    Pickupable p = collider.GetComponent<Pickupable>();
                    if (collider.tag.Equals("BinLid") && lidOn)
                    {
                        lidOn = false;                       
                        Debug.Log("Lid off");
                    }
                    else if (collider.tag.Equals("TiedBag"))
                    {
                        if (!lidOn)
                        {
                            binEmpty = true;
                            Debug.Log("Bin empty");
                        }
                    }
                    Debug.Log("pickupable " + p.gameObject);
                    carriedObject = p.gameObject;
                    carrying = true;
                    carriedObject.GetComponentInParent<Rigidbody>().useGravity = false;

                }
                else if (collider.GetComponent<PickupableSingle>())
                {
                    PickupableSingle pickSingle = collider.GetComponent<PickupableSingle>();
                    //Instantiate(spawnItem, new Vector3(0, 0, 0), Quaternion.identity);
                    singleItem.GetComponent<Renderer>().material.SetColor("_Color", pickSingle.itemColor);
                    singleItem.SetActive(true);
                    carriedObject = singleItem;
                    carrying = true;
                    carriedObject.GetComponent<Rigidbody>().useGravity = false;
                }
                else if (collider.GetComponent<PickupableTeaBag>())
                {
                    singleTeaBag.SetActive(true);
                    carriedObject = singleTeaBag;
                    carrying = true;
                    carriedObject.GetComponent<Rigidbody>().useGravity = false;
                }
                else
                {
                    Debug.Log("Collider " + collider.gameObject.name);
                    //if (!lidOn && !newBag)
                    if (!lidOn)
                    {
                        if (collider.tag.Equals("BinBag"))
                        {
                            Debug.Log("Bin Bag clicked");
                            binBag.SetActive(false);
                            //GameObject spawnItem = binBag.GetComponent<SpawnBag>().gameObject;
                            //spawnPosition = binBag.GetComponent<SpawnBag>().transform;

                            if (!newBag && tiedBag)
                            {
                                Debug.Log("Tied Bag found");                               
                                tiedBag.SetActive(true);
                                //tiedBag.GetComponent<Rigidbody>().isKinematic = true;
                            }
                            //Instantiate(spawnItem, spawnPosition.position, spawnPosition.rotation);
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
        Collider collider = GetComponent<MouseHoverObject>().GetMouseHoverObject(2);
        if (collider != null)
        {          
            if (collider.tag.Equals("Tap"))
            {
                if (carriedObject == kettle)
                    Debug.Log("Filling water ");
            }
            else if (collider.tag.Equals("Mug"))
            {
                if (carriedObject == kettle)
                    Debug.Log("Pouring water ");            
            }
            else
            {
                Vector3 originalPosition = carriedObject.GetComponent<Pickupable>().getOriginalPosition();
                float distance = Vector3.Distance(originalPosition, collider.transform.position);
                if (distance < 0.2f)
                {
                    Debug.Log(carriedObject.name + " Distance " + distance);
                    if (carriedObject == binLid)
                    {
                        Debug.Log("Lid distance " + distance);
                        lidOn = true;
                        Debug.Log("Lid on");
                    }
                    else if (carriedObject == tiedBag)
                    {
                        Debug.Log("Bag distance " + distance);
                        if (!lidOn && !newBag)
                        {
                            binEmpty = false;
                            Debug.Log("Bin is not empty");
                        }
                    }
                    carriedObject.transform.position = originalPosition;
                }
                else
                {
                    carriedObject.transform.position = GetComponent<MouseHoverObject>().GetHitPoint();
                }

                if (collider.tag.Equals("Bin"))
                {
                    if ((carriedObject == singleItem) && (binEmpty) && (!lidOn))
                    {
                        binBag.SetActive(true);
                        singleItem.SetActive(false);
                        Debug.Log("Bin bag default color " + binBag.GetComponent<Renderer>().material.GetColor("_Color"));
                        binBag.GetComponent<Renderer>().material.SetColor("_Color", singleItem.GetComponent<Renderer>().material.GetColor("_Color"));
                        newBag = true;
                    }
                }
                else if (collider.tag.Equals("Mug"))
                {
                    if (carriedObject == singleTeaBag)
                    {
                        teaBagIn = true;
                        singleTeaBag.SetActive(false);
                        Debug.Log("Tea bag is in");
                    }
                }

                carrying = false;
                Debug.Log("carrying is false");
                //carriedObject.GetComponent<Rigidbody>().AddForce(-transform.up * 20f);
                //carriedObject.GetComponent<Rigidbody>().AddTorque(transform.forward);
                /*if (carriedObject.GetComponent<IsKinematic>())
                {
                    carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                }*/
                carriedObject.GetComponent<Rigidbody>().useGravity = true;
                source.PlayOneShot(dropSound);
                carriedObject = null;
            }
        }
    }
}
