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
    private GameObject drink;
    public GameObject sandwich;
    private Object sandwichParent;
    private float sandwichHeight = 0;
    private bool sandwichStarted = false;

    // whether the inteference is currently on
    private bool interfering;

    private static int NUM_BREAD = 2;

    private GameObject dialog = null;

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
        drink = GameObject.FindWithTag("Drink");
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
                if (worldHandler.GetComponent<WorldModelManager>().GetInterference())
                {
                    //Debug.Log("Interference bleach " + worldHandler.GetComponent<WorldModelManager>().GetInterference());
                    //if (collider.tag.Equals("Bleach"))
                    worldHandler.GetComponent<WorldModelManager>().InterfereWorldModel(collider.gameObject);
                    Debug.Log(collider.gameObject.tag + " clicked");
                }
                else
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
                            worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("boiled", kettle, water);
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
                            if (((carriedObject) == kettle && (kettle.GetComponent<BoiledWater>().boiledWater != true)) ||
                                (carriedObject.transform.parent != null &&
                                (carriedObject.transform.parent.tag.Equals("WashingLiquids") || carriedObject.transform.parent.tag.Equals("Towels") ||
                                ((carriedObject.transform.parent.tag.Equals("BeverageContainers")) && (carriedObject.GetComponent<HasContent>().hasWater == false)))))
                            {
                                // update world model
                                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("pickedUp", player, carriedObject);
                            }
                            else if ((carriedObject) == kettle && (kettle.GetComponent<BoiledWater>().boiledWater == true))
                            {
                                // update world model
                                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("pickedUp", player, water);
                            }
                            else if (carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("BeverageContainers")) && (carriedObject.GetComponent<HasContent>().hasWater == true))
                            {
                                // update world model
                                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("pickedUp", player, drink);
                            }
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
                        CarriedObject(singleItem);
                        // update world model
                        if (!collider.tag.Equals("Napkin"))
                        {
                            worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("pickedUp", player, collider.gameObject);
                        }

                        // Picked the correct item
                        if (pickSingle.gameObject.GetComponent<CorrectItem>())
                        {
                            Debug.Log("Correct " + pickSingle.gameObject.tag + " picked");
                        }
                    }
                    // Picking up a single tea bag
                    else if (collider.GetComponent<PickupableTeaBag>())
                    {
                        PickupableTeaBag pickTeaBag = collider.GetComponent<PickupableTeaBag>();
                        GameObject singleTeaBag = Instantiate(pickTeaBag.singleTeaBag);
                        CarriedObject(singleTeaBag);
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("pickedUp", player, collider.gameObject);

                        // Picked the correct item
                        if (pickTeaBag.gameObject.GetComponent<CorrectItem>())
                        {
                            Debug.Log("Correct " + pickTeaBag.gameObject.tag + " picked");
                        }
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
                                //Debug.Log("Bin Bag clicked");
                                binBag.SetActive(false);

                                // Make tied bag visible 
                                if (!bin.GetComponent<NewBag>().newBag && tiedBag)
                                {
                                    //Debug.Log("Tied Bag found");
                                    tiedBag.SetActive(true);
                                    // update world model
                                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("tied", player, collider.gameObject);
                                }
                                // Player take out the new bin bag
                                else if (bin.GetComponent<NewBag>().newBag)
                                {
                                    bin.GetComponent<NewBag>().newBag = false;
                                    // update world model
                                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("tookOut", player, collider.gameObject);
                                }
                            }
                        }
                        // carrying a sandwich
                        else if ((collider.transform.parent != null) && collider.transform.parent.tag.Equals("Sandwich"))
                        {
                            {
                                Debug.Log("Sandwich exists");
                                CarriedObject(sandwich);
                            }
                        }
                        else if (collider.tag.Equals("Tap"))
                        {
                            Debug.Log("Wash hands");
                            // update world model
                            worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("wash", water, player);
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
            if (worldHandler.GetComponent<WorldModelManager>().GetInterference())
            {
                // update world model
                worldHandler.GetComponent<WorldModelManager>().InterfereWorldModel(collider.gameObject);
                Debug.Log(collider.gameObject.tag + " clicked");
            }
            else
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
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("filled", kettle, water);
                    }
                    else if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Dishes"))
                    {
                        Debug.Log("Rinsing dirty bowl ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("rinsed", carriedObject, water);
                    }
                    else if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Towels"))
                    {
                        Debug.Log("Wetting towel ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", water, carriedObject);
                    }                   
                    else
                    {
                        Debug.Log("Add water onto object ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("add", water, carriedObject);
                    }
                }
                // Floor is clicked while carrying the mop
                else if (collider.tag.Equals("Floor"))
                {
                    if (carriedObject.tag.Equals("Mop"))
                    {
                        // Practice trial
                        Debug.Log("Mopping the floor ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("mop", player, collider.gameObject);
                    }
                }
                // Either mug, cup, coffeemug or jar is clicked
                else if (collider.transform.parent != null && collider.transform.parent.tag.Equals("BeverageContainers"))
                {
                    // carrying kettle
                    if (carriedObject.tag.Equals("Kettle"))
                    {
                        Debug.Log("Pouring water ");
                        kettle.GetComponent<PouredWater>().pouredWater = true;
                        collider.GetComponent<HasContent>().hasWater = true;

                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", water, collider.gameObject);
                    }
                    // carrying juice
                    else if (carriedObject.tag.Equals("Juice"))
                    {
                        Debug.Log("Pouring juice ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", carriedObject, collider.gameObject);
                    }
                    // carrying teabag
                    else if (carriedObject.tag.Equals("SingleTeaBag"))
                    {
                        carriedObject.SetActive(false);
                        collider.GetComponent<HasContent>().hasTeaBag = true;
                        Debug.Log("Tea bag is in");

                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", carriedObject, collider.gameObject);
                        carrying = false;
                        carriedObject = null;
                    }
                }
                // either bowl, small plate, saucer or picnic plate is clicked
                else if (collider.transform.parent != null && collider.transform.parent.tag.Equals("Dishes"))
                {
                    // Pouring cereal, milk or honey into dishes
                    if (carriedObject.tag.Equals("Cereal") || carriedObject.tag.Equals("Milk") || carriedObject.tag.Equals("Honey"))
                    {
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", carriedObject, collider.gameObject);
                    }
                    // Washing dishes
                    else if (carriedObject.tag.Equals("Sponge"))
                    {
                        //Debug.Log("Washing dirty bowl ");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("washed", carriedObject, collider.gameObject);
                    }
                    else
                    {
                        Drop(collider);
                    }
                }
                // sandwich is clicked 
                //else if ((collider.transform.parent != null && collider.transform.parent.tag.Equals("Sandwich")) && (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("SandwichFilling")))
                else if ((collider.tag.Equals("Sandwich") || (collider.transform.parent != null && collider.transform.parent.tag.Equals("Sandwich"))) && 
                    (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("SandwichFilling")))
                { 
                    // user is carrying either cheese, spread, jam or pate
                    Debug.Log("Putting sandwich filling on sandwich ");
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, sandwich);
                }
                // Clicking on BinBody
                else if (collider.tag.Equals("BinBody"))
                {
                    // Putting bin bag onto the bin
                    if ((carriedObject.tag.Equals("SingleItem")) && (bin.GetComponent<BinEmpty>().binEmpty) && (!bin.GetComponent<LidOn>().lidOn))
                    {
                        binBag.SetActive(true);
                        //singleItem.SetActive(false);                   
                        Debug.Log("Bin bag default color " + binBag.GetComponent<Renderer>().material.GetColor("_Color"));
                        binBag.GetComponent<Renderer>().material.SetColor("_Color", carriedObject.GetComponent<Renderer>().material.GetColor("_Color"));
                        bin.GetComponent<NewBag>().newBag = true;
                        carriedObject.SetActive(false);

                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", carriedObject, collider.gameObject);
                        carrying = false;
                        carriedObject = null;
                    }
                }
                // Clicking on the BinLid while carrying another item, disposing the object into the bin
                else if (collider.tag.Equals("BinLid"))
                {
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("disposed", carriedObject, collider.gameObject);
                    Debug.Log("Disposing " + carriedObject.tag);

                    /*if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Dishes"))
                    {
                        Debug.Log("Disposing Cereal");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("disposed", carriedObject, collider.gameObject);
                    }*/
                }
                // Clicking on Sponge while carrying washing liquid, putting the liquid on sponge
                else if (collider.tag.Equals("Sponge"))
                {
                    //if (carriedObject.tag.Equals("DishwashingLiquid"))
                    if (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("WashingLiquids"))
                    {
                        Debug.Log("Putting washing liquid on sponge");
                        // update world model
                        worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, collider.gameObject);
                    }
                }
                // if table or kitchentop is clicked while carrying towels - wiping the surface
                else if ((collider.tag.Equals("Table") || collider.tag.Equals("KitchenTop")) && (carriedObject.transform.parent != null && carriedObject.transform.parent.tag.Equals("Towels")))
                {                  
                    //Debug.Log("Wiping table ");
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("wiped", carriedObject, collider.gameObject);                                        
                }
                else
                {
                    Drop(collider);
                }
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
                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", binLid, collider.gameObject);
                Debug.Log("Lid on");
            }
            else if (carriedObject == tiedBag)
            {
                Debug.Log("Bag distance " + distance);
                if (!bin.GetComponent<LidOn>().lidOn && !bin.GetComponent<NewBag>().newBag)
                {
                    bin.GetComponent<BinEmpty>().binEmpty = false;
                    // update world model
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("in", tiedBag, collider.gameObject);
                    Debug.Log("Bin is not empty");
                }
            }
            // putting kettle down
            else if ((carriedObject) == kettle && (kettle.GetComponent<BoiledWater>().boiledWater != true))
            {
                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", kettle, collider.gameObject);
            }
            // putting kettle with boiled water down
            else if ((carriedObject) == kettle && (kettle.GetComponent<BoiledWater>().boiledWater == true))
            {
                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", water, collider.gameObject);
            }
            else
            {
                // update world model - object back in the original position
                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("back", carriedObject, collider.gameObject);
            }
            carriedObject.transform.position = originalPosition;
        }
        else
        {
            carriedObject.transform.position = GetComponent<MouseHoverObject>().GetHitPoint();

            // update world model with the new position of the object
            if (carriedObject.tag.Equals("SingleNapkin") || carriedObject.tag.Equals("SingleSlice") ||
                carriedObject.tag.Equals("SingleWrap") || carriedObject.tag.Equals("SinglePitta") || carriedObject.tag.Equals("SingleRoll") ||
                carriedObject.tag.Equals("HamSlice") || carriedObject.tag.Equals("CheeseSlice"))
            {
                if (!sandwichStarted && (carriedObject.tag.Equals("SingleNapkin") || carriedObject.tag.Equals("SingleSlice") ||
                carriedObject.tag.Equals("SingleWrap") || carriedObject.tag.Equals("SinglePitta") || carriedObject.tag.Equals("SingleRoll")))
                {
                    sandwich.transform.position = GetComponent<MouseHoverObject>().GetHitPoint();
                    sandwichStarted = true;
                }
                distance = Vector3.Distance(sandwich.transform.position, carriedObject.transform.position);
                if (distance < 0.1f)
                {
                    carriedObject.transform.position = sandwich.transform.position + (transform.up * sandwichHeight);
                    carriedObject.transform.parent = sandwich.transform;
                    sandwichHeight += carriedObject.GetComponent<Collider>().bounds.size.y;

                    // the object does not have independent properties anymore once it has been attached to sandwich object
                    Destroy(carriedObject.GetComponent<Rigidbody>());
                    Destroy(carriedObject.GetComponent<Pickupable>());
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, sandwich);
                }
                else
                {
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, collider.gameObject);
                }
            }
            else
            {
                if ((carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("Dough") || carriedObject.transform.parent.tag.Equals("SandwichFilling"))) || carriedObject.tag.Equals("Ham"))
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
                            carriedObject.GetComponent<InstantiateItem>().Instantiate((carriedObject.transform.position + (transform.up * (carriedObject.GetComponent<Collider>().bounds.size.y))), 2);
                        }
                    }                      
                }
                else if ((carriedObject.transform.parent != null && 
                    (carriedObject.transform.parent.tag.Equals("Cutlery") || 
                    (carriedObject.transform.parent.tag.Equals("BeverageContainers")) || 
                    carriedObject == sandwich)))
                {
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
                                            worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("right", carriedObject, hitCollider.gameObject);
                                        }
                                        else if (angleDir < 0.0f)
                                        {
                                            //Debug.Log(carriedObject.transform.parent.tag + "  is at the left side of " + hitCollider.tag);
                                            // update world model
                                            worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("left", carriedObject, hitCollider.gameObject);
                                        }
                                        else
                                        {
                                            //Debug.Log(carriedObject.transform.parent.tag + " is at the front or back of " + hitCollider.tag);
                                            // update world model
                                            if (carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("BeverageContainers")) && (carriedObject.GetComponent<HasContent>().hasWater == true))
                                            {
                                                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("front", drink, hitCollider.gameObject);
                                            }
                                            else
                                            {
                                                worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("front", carriedObject, hitCollider.gameObject);
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }
                    }
                }

                if (carriedObject.transform.parent != null && (carriedObject.transform.parent.tag.Equals("BeverageContainers")) && (carriedObject.GetComponent<HasContent>().hasWater == true))
                {
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", drink, collider.gameObject);
                }
                // for objects other than bread, cheese and ham that are put on the sandwich
                /*else if (collider.transform.parent != null && collider.transform.parent.tag.Equals("Sandwich"))
                {
                    carriedObject.transform.position = sandwich.transform.position + (transform.up * sandwichHeight);
                    carriedObject.transform.parent = sandwich.transform;
                    sandwichHeight += carriedObject.GetComponent<Collider>().bounds.size.y;
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, sandwich);
                }*/
                else
                {
                    // update world model with the new position of the object
                    worldHandler.GetComponent<WorldModelManager>().UpdateWorldModel("on", carriedObject, collider.gameObject);
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
