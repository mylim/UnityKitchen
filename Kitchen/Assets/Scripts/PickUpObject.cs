using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/** 
Handles objects pickup and dropping
*/

public class PickUpObject : MonoBehaviour
{
    private GameObject player;
    private GameObject mainCamera;
    private GameObject worldHandler;
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
    private GameObject water;
    public GameObject sandwich;
    private Object sandwichParent;
    private float sandwichHeight = 0;
    private bool makingSandwich = false;

    private bool interfering;
    //private GameObject singleTeaBag;
    //private bool teaBagIn;

    private static int NUM_BREAD = 2;

    private GameObject dialog = null;

   //private ObjectsHandler objectHandler;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        worldHandler = GameObject.FindWithTag("WorldHandler");
        objectsHandler = GameObject.FindWithTag("ObjectHandler");
        //manager = new WorldModelManager();
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
        water = GameObject.FindWithTag("Water");
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
        interfering = false;
    }

    void Update()
    {
        //interfering = worldHandler.GetComponent<WorldModelManager>().GetInterference();
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
                /*if (collider.GetComponent<Interfere>())
                {
                    //collider.GetComponent<Interfere>().ShowID();
                    collider.GetComponent<Interfere>().dialog.GetComponent<InterferenceDialog>().ShowDialog();
                    dialog = collider.GetComponent<Interfere>().dialog;
                    //interfering = true;
                    //collider.GetComponent<Interfere>().dialog.GetComponent<InterferenceDialog>().SetInterference();
                }*/

                // interference on 
                /*if (collider.tag.Equals("Bleach") && interfering)
                {
                    Debug.Log("Interference bleach " + interfering);
                    //if (collider.tag.Equals("Bleach"))
                        Debug.Log("Bleach clicked");

                }
                else*/
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

                            // update world model
                            worldHandler.GetComponent<WorldModelManager>().updateWorldModel("boiled", kettle, water);
                        }
                        else if (collider.tag.Equals("Bleach") && worldHandler.GetComponent<WorldModelManager>().GetInterference())
                        {
                            //Debug.Log("Interference bleach " + worldHandler.GetComponent<WorldModelManager>().GetInterference());
                            //if (collider.tag.Equals("Bleach"))
                            Debug.Log("Bleach clicked");

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

                            // update world model
                            //manager.updateWorldModel("pickedUp", player, p.gameObject);
                            if (carriedObject == kettle || (carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("WashingLiquids") || carriedObject.transform.parent.tag.Equals("Towels") || carriedObject.transform.parent.tag.Equals("BeverageContainers"))))
                            {
                                // update world model
                                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("pickedUp", player, carriedObject);
                            }

                        }

                        if (p.gameObject.transform.parent != null)
                        {
                            objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(p.gameObject.tag, p.gameObject.transform.parent.tag, p.gameObject);
                        }
                        else
                        {
                            objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(p.gameObject.tag, p.gameObject.tag, p.gameObject);
                        }

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
                        //singleItem.GetComponent<Renderer>().material.SetTexture("_MainTex", pickSingle.itemTexture);
                        //singleItem.GetComponent<Renderer>().material.mainTexture = pickSingle.itemTexture;
                        CarriedObject(singleItem);
                        // update world model
                        if (!collider.tag.Equals("Napkin"))
                        {
                            worldHandler.GetComponent<WorldModelManager>().updateWorldModel("pickedUp", player, collider.gameObject);
                        }

                        objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(pickSingle.gameObject.tag, pickSingle.gameObject.gameObject.tag, pickSingle.gameObject);

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
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().updateWorldModel("pickedUp", player, collider.gameObject);

                        objectsHandler.GetComponent<ObjectsHandler>().addPickedObject(pickTeaBag.gameObject.tag, pickTeaBag.gameObject.tag, pickTeaBag.gameObject);

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
                                    // update world model
                                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("tied", player, collider.gameObject);
                                }
                                // Player take out the new bin bag
                                else if (bin.GetComponent<NewBag>().newBag)
                                {
                                    bin.GetComponent<NewBag>().newBag = false;
                                    // update world model
                                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("tookOut", player, collider.gameObject);
                                }
                            }
                        }
                        /*else if (collider.tag.Equals("SingleNapkin") || collider.tag.Equals("HamSlice") ||
                         collider.tag.Equals("CheeseSlice") || collider.tag.Equals("SingleSlice")
                         collider.tag.Equals("CheeseSlice") || collider.tag.Equals("SingleSlice")))*/
                        else if ((collider.transform.parent != null) && collider.transform.parent.tag.Equals("Sandwich"))
                        {
                            {
                                Debug.Log("Sandwich exists");
                                CarriedObject(sandwich);
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
        if (carriedObject.GetComponent<Rigidbody>())
        {
            carriedObject.GetComponent<Rigidbody>().useGravity = false;
        }
        //carriedObject.GetComponentInParent<Rigidbody>().useGravity = false;
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
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("filled", kettle, water);
                }
                //else if (carriedObject.tag.Equals("Bowl") || carriedObject.tag.Equals("PicnicPlate") || carriedObject.tag.Equals("SmallPlate") || carriedObject.tag.Equals("Saucer"))
                else if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Dishes"))
                {
                    Debug.Log("Rinsing dirty bowl ");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("rinsed", carriedObject, water);
                }
                else if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Towels"))
                {
                    Debug.Log("Wetting towel ");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", water, carriedObject);
                }
            }
            //else if (collider.tag.Equals("Mug") || collider.tag.Equals("Cup") || collider.tag.Equals("Jar") || collider.tag.Equals("CoffeeMug"))
            else if (collider.transform.parent != null && collider.transform.parent.tag.Equals("BeverageContainers"))
            {
                if (carriedObject.tag.Equals("Kettle"))
                {
                    Debug.Log("Pouring water ");
                    kettle.GetComponent<PouredWater>().pouredWater = true;
                    collider.GetComponent<HasContent>().hasWater = true;

                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("in", water, collider.gameObject);
                }
                else if (carriedObject.tag.Equals("SingleTeaBag"))
                {
                    carriedObject.SetActive(false);                  
                    Debug.Log("Tea bag is in");

                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("in", carriedObject, collider.gameObject);
                    carrying = false;
                    carriedObject = null;
                }
            }
            //else if (collider.tag.Equals("Bowl") || collider.tag.Equals("PicnicPlate") || collider.tag.Equals("SmallPlate") || collider.tag.Equals("Saucer"))
            else if (collider.transform.parent != null && collider.transform.parent.tag.Equals("Dishes"))
            {
                if (carriedObject.tag.Equals("Cereal") || carriedObject.tag.Equals("Milk") || carriedObject.tag.Equals("Honey"))
                {
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("in", carriedObject, collider.gameObject);
                }
                /*if (carriedObject.tag.Equals("Cereal"))
                {
                    Debug.Log("Pouring Cereal");
                }
                else if (carriedObject.tag.Equals("Milk"))
                {
                    Debug.Log("Pouring Milk");
                }
                else if (carriedObject.tag.Equals("Honey"))
                {
                    Debug.Log("Pouring Honey ");
                }*/
                else if (carriedObject.tag.Equals("Sponge"))
                {
                    Debug.Log("Washing dirty bowl ");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("washed", carriedObject, collider.gameObject);
                }
                else
                {
                    Drop(collider);
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

                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("in", carriedObject, collider.gameObject);
                    carrying = false;
                    carriedObject = null;
                }
            }
            else if (collider.tag.Equals("BinLid"))
            {
                //if (carriedObject.tag.Equals("Bowl") || carriedObject.tag.Equals("PicnicPlate") || carriedObject.tag.Equals("SmallPlate") || carriedObject.tag.Equals("Saucer"))
                if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Dishes"))
                {
                    Debug.Log("Disposing Cereal");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("disposed", carriedObject, collider.gameObject);
                }
            }
            else if (collider.tag.Equals("Sponge"))
            {
                if (carriedObject.tag.Equals("DishwashingLiquid"))
                {
                    Debug.Log("Putting dishwashing liquid on sponge");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", carriedObject, collider.gameObject);
                }
            }
            else if ((collider.tag.Equals("Table") || collider.tag.Equals("KitchenTop")) && (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Towels")))
            {
                Debug.Log("Wiping table ");
                // update world model
                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("wiped", carriedObject, collider.gameObject);
            }
            // interference on 
            else if (collider.tag.Equals("Bleach") && worldHandler.GetComponent<WorldModelManager>().GetInterference())
            {
                //Debug.Log("Dialog interference " + worldHandler.GetComponent<WorldModelManager>().GetInterference());
                Debug.Log("Bleach clicked");
            }
            else
            {
                Drop(collider);
            }
        }
    }

    void Drop(Collider collider)
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
                // update world model
                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", binLid, collider.gameObject);
                Debug.Log("Lid on");
            }
            else if (carriedObject == tiedBag)
            {
                Debug.Log("Bag distance " + distance);
                if (!bin.GetComponent<LidOn>().lidOn && !bin.GetComponent<NewBag>().newBag)
                {
                    bin.GetComponent<BinEmpty>().binEmpty = false;
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().updateWorldModel("in", tiedBag, collider.gameObject);
                    Debug.Log("Bin is not empty");
                }
            }
            else if (carriedObject == kettle)
            {
                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", kettle, collider.gameObject);
            }
            carriedObject.transform.position = originalPosition;
        }
        else
        {
            carriedObject.transform.position = GetComponent<MouseHoverObject>().GetHitPoint();

            // update world model with the new position of the object
            // manager.updateWorldModel("on", carriedObject, collider.gameObject);

            if (carriedObject.tag.Equals("SingleNapkin") || carriedObject.tag.Equals("SingleSlice") ||
                carriedObject.tag.Equals("SingleWrap") || carriedObject.tag.Equals("SinglePitta") || carriedObject.tag.Equals("SingleRoll") ||
                carriedObject.tag.Equals("HamSlice") || carriedObject.tag.Equals("CheeseSlice"))
            {
                carriedObject.transform.position = sandwich.transform.position + (transform.up * sandwichHeight);
                carriedObject.transform.parent = sandwich.transform;
                sandwichHeight += carriedObject.GetComponent<Collider>().bounds.size.y;
     
                // the object does not have independent properties anymore once it has been attached to sandwich object
                Destroy(carriedObject.GetComponent<Rigidbody>());
                Destroy(carriedObject.GetComponent<Pickupable>());
                // update world model with the new position of the object
                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", carriedObject, sandwich);
            }
            else
            {
                if ((carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("Dough") || carriedObject.transform.parent.tag.Equals("Cheeses"))) || carriedObject.tag.Equals("Ham"))
                {
                    //RigidbodyConstraints constraints = carriedObject.GetComponent<Rigidbody>().constraints;
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
                            carriedObject.GetComponent<InstantiateItem>().Instantiate((carriedObject.transform.position + (transform.up * (carriedObject.GetComponent<Collider>().bounds.size.y))), 2);
                        }
                    }                   
                }
                else if ((carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("Cutlery") || carriedObject.transform.parent.tag.Equals("BeverageContainers")) || carriedObject == sandwich))
                {
                    //if (carriedObject.transform.parent.tag.Equals("Cutlery") || carriedObject.transform.parent.tag.Equals("BeverageContainers"))                        
                        //|| (carriedObject == sandwich)))// || carriedObject.transform.parent.tag.Equals("Dishes"))
                    {
                        if (collider.tag.Equals("Table") || collider.tag.Equals("Placemat"))
                        {
                            Collider[] hitColliders = Physics.OverlapSphere(carriedObject.transform.position, 0.1f);
                            foreach (Collider hitCollider in hitColliders)
                            {
                                if (hitCollider.transform.parent != null)
                                {
                                    //if (!hitCollider.transform.parent.tag.Equals(carriedObject.transform.parent.tag))
                                    if (hitCollider.transform.parent.tag.Equals("Dishes"))
                                    {
                                        //if (hitCollider.tag.Equals("Bowl") || hitCollider.tag.Equals("PicnicPlate") || hitCollider.tag.Equals("SmallPlate") || hitCollider.tag.Equals("Saucer"))
                                        //if (hitCollider.transform.parent.tag.Equals("Cutlery") || hitCollider.transform.parent.tag.Equals("BeverageContainers") || hitCollider.transform.parent.tag.Equals("Sandwich") || hitCollider.transform.parent.tag.Equals("Dishes"))
                                        {
                                            // direction from the cutlery to the dish
                                            /*Vector3 dir = (hitCollider.transform.position - carriedObject.transform.position);
                                            float angle = Vector3.Angle(dir, mainCamera.transform.forward);


                                            Debug.DrawLine(carriedObject.transform.position, mainCamera.transform.forward, Color.red, 2f);
                                            Debug.DrawLine(hitCollider.transform.position, carriedObject.transform.position, Color.green, 2f);

                                            Debug.Log("Angle + collider " + angle + hitCollider.tag);
                                            float angleDir = AngleDir(mainCamera.transform.forward, dir, mainCamera.transform.up);
                                            Debug.Log("AngleDir " + angleDir);
                                            if (angleDir < 0f)
                                            {
                                                Debug.Log("Cutlery is at the right of dishes " + hitCollider.tag);
                                            }
                                            else if (angleDir > 0f)
                                            {
                                                Debug.Log("Cutlery is at the wrong side of " + hitCollider.tag);
                                            }
                                            else
                                            {
                                                Debug.Log("Cutlery is at the front or back of " + hitCollider.tag);
                                            }*/

                                            // direction from the dish to the cutlery
                                            Vector3 dir = (carriedObject.transform.position - hitCollider.transform.position);
                                            float angle = Vector3.Angle(dir, mainCamera.transform.forward);

                                            //Debug.DrawLine(hitCollider.transform.position, mainCamera.transform.forward, Color.red, 2f);
                                            //Debug.DrawLine(carriedObject.transform.position, hitCollider.transform.position, Color.green, 2f);
                                            Debug.Log("Angle + collider " + angle + hitCollider.tag);
                                            float angleDir = AngleDir(mainCamera.transform.forward, dir, mainCamera.transform.up);
                                            Debug.Log("AngleDir " + angleDir);
                                            if (angleDir > 0.0f && (angle > 45f && angle < 135f))
                                            {
                                                //Debug.Log(carriedObject.transform.parent.tag + " is at the right of " + hitCollider.tag);
                                                // update world model
                                                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("right", carriedObject, hitCollider.gameObject);
                                            }
                                            else if (angleDir < 0.0f)
                                            {
                                                //Debug.Log(carriedObject.transform.parent.tag + "  is at the left side of " + hitCollider.tag);
                                                // update world model
                                                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("left", carriedObject, hitCollider.gameObject);
                                            }
                                            else
                                            {
                                                //Debug.Log(carriedObject.transform.parent.tag + " is at the front or back of " + hitCollider.tag);
                                                // update world model
                                                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("front", carriedObject, hitCollider.gameObject);
                                            }
                                        }
                                    }
                                }
                            }
                        } 
                    }
                }
                // update world model with the new position of the object
                worldHandler.GetComponent<WorldModelManager>().updateWorldModel("on", carriedObject, collider.gameObject);
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
        if (carriedObject.GetComponent<Rigidbody>())
        {
            carriedObject.GetComponent<Rigidbody>().useGravity = true;
        }
        //source.PlayOneShot(dropSound);
        carriedObject = null;
    }

    private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        //returns negative when to the left, positive to the right, and 0 for forward/backward
        Vector3 right = Vector3.Cross(up, fwd);        // right vector
        float dir = Vector3.Dot(right, targetDir);
        Debug.Log("AngleDir " + dir);
        return dir;

        /*returns -1 when to the left, 1 to the right, and 0 for forward/backward
        if (dir > 0.0f)
        {
            Debug.Log("Returning 1");
            return 1f;
        }
        else if (dir < 0.0f)
        {
            Debug.Log("Returning -1");
            return -1f;
        }
        else
        {
            Debug.Log("Returning 0");
            return 0f;
        }*/
    }

}
