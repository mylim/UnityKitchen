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
    private GameObject objectsHandler;
    private RaycastHit rayCastHit;

    // Object being carried
    private bool carrying;
    private GameObject carriedObject;
    public float distance;
    public float smooth;

    // Bin manipulation
    private GameObject binLid;
    private GameObject bin;
    //private GameObject singleItem;
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
    //private GameObject singleTeaBag;
    private bool teaBagIn;

    private static int NUM_BREAD = 2;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        objectsHandler = GameObject.FindWithTag("ObjectsHandler");
        carrying = false;

        // Garbage removal
        bin = GameObject.FindWithTag("Bin");
        binLid = GameObject.FindWithTag("BinLid");
        binBag = GameObject.FindWithTag("BinBag");
        tiedBag = GameObject.FindWithTag("TiedBag");

        /*if (GameObject.FindWithTag("Bin"))
        {
            bin = GameObject.FindWithTag("Bin");
        }

        if (GameObject.FindWithTag("BinLid"))
        {
            binLid = GameObject.FindWithTag("BinLid");           
        }       
      
        if (GameObject.FindWithTag("BinBag"))
        {
            binBag = GameObject.FindWithTag("BinBag");
        }

        if (GameObject.FindWithTag("TiedBag"))
        {
            Debug.Log("Tied Bag found");
            tiedBag = GameObject.FindWithTag("TiedBag");
        }*/

        // Tea making
        kettle = GameObject.FindWithTag("Kettle");
        /*if (GameObject.FindWithTag("Kettle"))
        {
            Debug.Log("Kettle");
            kettle = GameObject.FindWithTag("Kettle");
        }*/
        /*if (GameObject.FindWithTag("SingleTeaBag"))
        {
            singleTeaBag = GameObject.FindWithTag("SingleTeaBag");
            singleTeaBag.SetActive(false);
            teaBagIn = false;
        }*/
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
                    // Kettle is clicked 
                    if (p.gameObject.tag.Equals("Kettle") && (kettle.GetComponent<FilledWithWater>().filledWithWater) && (!kettle.GetComponent<BoiledWater>().boiledWater))
                    {
                        // The kettle has been filled with water
                        Debug.Log("Boiling water");
                        kettle.GetComponent<BoiledWater>().boiledWater = true;                       
                    }                    
                    else
                    {
                        CarriedObject(p.gameObject);

                        // Bin lid has been removed
                        if (p.gameObject.tag.Equals("BinLid"))
                        {
                            if (bin.GetComponent<LidOn>().lidOn)
                            {
                                bin.GetComponent<LidOn>().lidOn = false;
                            }
                        }
                        // Tied bag has been removed
                        else if (p.gameObject.tag.Equals("TiedBag"))
                        {
                            if (!bin.GetComponent<LidOn>().lidOn)
                            {
                                bin.GetComponent<BinEmpty>().binEmpty = true;
                            }
                        }
                    }

                    objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(p.gameObject.tag, p.gameObject);

                    // Picked the correct item
                    if (p.gameObject.GetComponent<CorrectItem>())
                    {
                        Debug.Log("Correct " + p.gameObject.tag + " picked");
                    }
                }
                // Picking up a single item of a collection, eg. bin bag, napkin
                else if (collider.GetComponent<PickupableSingle>())
                {
                    PickupableSingle pickSingle = collider.GetComponent<PickupableSingle>();
                    GameObject singleItem = Instantiate(pickSingle.singleItem);
                    singleItem.GetComponent<Renderer>().material.SetColor("_Color", pickSingle.itemColor);
                    CarriedObject(singleItem);

                    objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(pickSingle.gameObject.tag, pickSingle.gameObject);

                    // Picked the correct item
                    if (pickSingle.gameObject.GetComponent<CorrectItem>())
                    {
                        Debug.Log("Correct " + pickSingle.gameObject.tag + " picked");
                    }

                    /*singleItem.GetComponent<Renderer>().material.SetColor("_Color", pickSingle.itemColor);
                    singleItem.SetActive(true);
                    CarriedObject(singleItem);*/
                }
                // Picking up a single tea bag
                else if (collider.GetComponent<PickupableTeaBag>())
                {
                    PickupableTeaBag pickTeaBag = collider.GetComponent<PickupableTeaBag>();
                    GameObject singleTeaBag = Instantiate(pickTeaBag.singleTeaBag);
                    CarriedObject(singleTeaBag);

                    objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(pickTeaBag.gameObject.tag, pickTeaBag.gameObject);

                    // Picked the correct item
                    if (pickTeaBag.gameObject.GetComponent<CorrectItem>())
                    {
                        Debug.Log("Correct " + pickTeaBag.gameObject.tag + " picked");
                    }
                    //singleTeaBag.SetActive(true);
                    //CarriedObject(singleTeaBag);
                }
                else
                {
                    // Bin bag clicked
                    Debug.Log("Collider " + collider.gameObject.name);
                    if (collider.tag.Equals("BinBag"))
                    {
                        if (!bin.GetComponent<LidOn>().lidOn)
                        {
                            //Make the bin bag invisible
                            Debug.Log("Bin Bag clicked");
                            binBag.SetActive(false);

                            // Make tied bag visible 
                            if (!bin.GetComponent<NewBag>().newBag && tiedBag)
                            {
                                Debug.Log("Tied Bag found");
                                tiedBag.SetActive(true);
                            }
                            else if (bin.GetComponent<NewBag>().newBag)
                            {
                                bin.GetComponent<NewBag>().newBag = false;
                            }
                        }
                    }
                }
            }
        }
    }

    void CarriedObject(GameObject gameObject)
    {
        Debug.Log("pickupable " + gameObject);
        Debug.Log("carried object" + gameObject.tag);
        carriedObject = gameObject;
        carrying = true;
        carriedObject.GetComponentInParent<Rigidbody>().useGravity = false;
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
            Debug.Log("Collider for drop object " + collider.gameObject.tag);
            // Tap is clicked while carrying the kettle = filling water
            if (collider.tag.Equals("Tap"))
            {
                if (carriedObject.tag.Equals("Kettle"))
                {
                    Debug.Log("Filling water ");
                    kettle.GetComponent<FilledWithWater>().filledWithWater = true;
                }
            }
            else if (collider.tag.Equals("Mug") || collider.tag.Equals("Cup") || collider.tag.Equals("Jar") || collider.tag.Equals("CoffeeMug"))
            {
                if (carriedObject.tag.Equals("Kettle"))
                {
                    Debug.Log("Pouring water ");
                    kettle.GetComponent<PouredWater>().pouredWater = true;
                    collider.GetComponent<HasContent>().hasWater = true;
                }
                else if (carriedObject.tag.Equals("SingleTeaBag"))
                {
                    carriedObject.SetActive(false);
                    Debug.Log("Tea bag is in");
                    /*singleTeaBag.GetComponent<TeaBagIn>().teaBagIn = true;
                    singleTeaBag.SetActive(false);                   */
                }
            }
            else if (collider.tag.Equals("BinBody"))
            {
                if ((carriedObject.tag.Equals("SingleItem")) && (bin.GetComponent<BinEmpty>().binEmpty) && (!bin.GetComponent<LidOn>().lidOn))
                {
                    binBag.SetActive(true);
                    //singleItem.SetActive(false);                   
                    Debug.Log("Bin bag default color " + binBag.GetComponent<Renderer>().material.GetColor("_Color"));
                    binBag.GetComponent<Renderer>().material.SetColor("_Color", carriedObject.GetComponent<Renderer>().material.GetColor("_Color"));
                    bin.GetComponent<NewBag>().newBag = true;
                    carriedObject.SetActive(false);
                }
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
                        bin.GetComponent<LidOn>().lidOn = true;
                        Debug.Log("Lid on");
                    }
                    else if (carriedObject == tiedBag)
                    {
                        Debug.Log("Bag distance " + distance);
                        if (!bin.GetComponent<LidOn>().lidOn && !bin.GetComponent<NewBag>().newBag)
                        {
                            bin.GetComponent<BinEmpty>().binEmpty = false;
                            Debug.Log("Bin is not empty");
                        }
                    }
                    carriedObject.transform.position = originalPosition;
                }
                else
                {
                    carriedObject.transform.position = GetComponent<MouseHoverObject>().GetHitPoint();
                    if (carriedObject.tag.Equals("Bread") || carriedObject.tag.Equals("Breadroll") || carriedObject.tag.Equals("Pitta") || 
                        carriedObject.tag.Equals("Wrap") || carriedObject.tag.Equals("Ham") || carriedObject.tag.Equals("Cheese"))
                    {
                        carriedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        //Vector3 movement = carriedObject.transform.position + GetComponent<MouseHoverObject>().GetHitPoint();
                        Debug.Log("carried object tag" + carriedObject.tag);

                        if (collider.tag.Equals("KitchenTop") || collider.tag.Equals("Table"))
                        {
                            Debug.Log("collider tag" + collider.tag);
                            // Instantiate items only if the object is placed on the table                            
                            if (carriedObject.GetComponent<InstantiateItem>())
                            {
                                carriedObject.GetComponent<OnTable>().onTable = true;
                                carriedObject.GetComponentInParent<InstantiateItem>().Instantiate((carriedObject.transform.position + (transform.up * (carriedObject.GetComponent<Collider>().bounds.size.y / 2))), 2);
                            }
                        }
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

                //source.PlayOneShot(dropSound);
                carriedObject = null;
            }
        }
    }
}
