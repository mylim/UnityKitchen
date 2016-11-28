using UnityEngine;
using System.Collections;

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

    GameObject spawnItem;
    Transform spawnPosition;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        carrying = false;

        if (GameObject.FindWithTag("BinLid"))
        {
            binLid = GameObject.FindWithTag("BinLid");
            lidPosition = binLid.transform.position;
            lidOn = true;
        }

        if (GameObject.FindWithTag("Bin"))
        {
            bin = GameObject.FindWithTag("Bin");
            binPosition = bin.transform.position;
        }

        if (GameObject.FindWithTag("SingleItem"))
        {
            singleItem = GameObject.FindWithTag("SingleItem");
            singleItem.SetActive(false);
        }

        if (GameObject.FindWithTag("BinBag"))
        {
            binBag = GameObject.FindWithTag("BinBag");
            binBagPosition = binBag.transform.position;
            newBag = false;
        }

        if (GameObject.FindWithTag("TiedBag"))
        {
            Debug.Log("Tied Bag found");

            tiedBag = GameObject.FindWithTag("TiedBag");
            tiedBagPosition = tiedBag.transform.position;            
            tiedBag.SetActive(false);            
        }

        source = GetComponent<AudioSource>();
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
            Collider collider = GetMouseHoverObject(2);
            if (collider != null)
            {
                Debug.Log("In Pickup()");

                if (collider.GetComponent<Pickupable>())
                {
                    Pickupable p = collider.GetComponent<Pickupable>();
                    if (collider.tag.Equals("BinLid"))
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
                    //carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                    carriedObject.GetComponent<Rigidbody>().useGravity = false;

                }
                else if (collider.GetComponent<PickupableSingle>())
                {
                    PickupableSingle pickSingle = collider.GetComponent<PickupableSingle>();
                    //Instantiate(spawnItem, new Vector3(0, 0, 0), Quaternion.identity);
                    singleItem.GetComponent<Renderer>().material.SetColor("_Color", pickSingle.itemColor);
                    singleItem.SetActive(true);
                    carriedObject = singleItem;
                    carrying = true;
                    //carriedObject.GetComponent<Rigidbody>().isKinematic = true;
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
                            //spawnItem = binBag.GetComponent<SpawnBag>().gameObject;
                            //spawnPosition = binBag.GetComponent<SpawnBag>().transform;

                            if (tiedBag)
                            {
                                Debug.Log("Tied Bag found");
                                tiedBag.SetActive(true);
                                tiedBag.transform.position = tiedBagPosition;
                            }
                           // Instantiate(spawnItem, spawnPosition.position, spawnPosition.rotation);
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
        Collider collider = GetMouseHoverObject(2);
        if (collider != null)
        {
            if (carriedObject == binLid)
            {
                float distance = Vector3.Distance(lidPosition, collider.transform.position);
                Debug.Log("Lid distance " + distance);
                if (distance < 1.0f)
                {
                    carriedObject.transform.position = lidPosition;
                    lidOn = true;
                    Debug.Log("Lid on");
                }
            }
            else if (carriedObject == tiedBag)
            {
                float distance = Vector3.Distance(tiedBagPosition, collider.transform.position);
                Debug.Log("Bag distance " + distance);
                if (distance < 1.0f && !lidOn && !newBag)
                {
                    carriedObject.transform.position = tiedBagPosition;
                    //GameObject.FindWithTag("Bin").transform.position = binPosition;
                    binEmpty = false;
                    Debug.Log("Bin is not empty");
                }                
            }
            /*else if (carriedObject == spawnItem)
            {
                float distance = Vector3.Distance(spawnPosition.position, collider.transform.position);
                Debug.Log("Bag distance " + distance);
                if (distance < 1.0f && !lidOn)
                {
                    carriedObject.transform.position = spawnPosition.position;
                    GameObject.FindWithTag("Bin").transform.position = binPosition;
                    binEmpty = false;
                    Debug.Log("Bin is not empty");
                }
            }*/
            else if (carriedObject == singleItem)
            {
                if ((collider.tag.Equals("Bin")) && (!lidOn))
                {
                    binBag.SetActive(true);
                    binBag.transform.position = binBagPosition;
                    singleItem.SetActive(false);
                    Debug.Log("Bin bag default color " + binBag.GetComponent<Renderer>().material.GetColor("_Color"));
                    binBag.GetComponent<Renderer>().material.SetColor("_Color", singleItem.GetComponent<Renderer>().material.GetColor("_Color"));
                 
                    //GameObject.FindWithTag("Bin").transform.position = binPosition;
                    newBag = true;
                }
            }

            carrying = false;
            //carriedObject.GetComponent<Rigidbody>().AddForce(-transform.up * 20f);
            //carriedObject.GetComponent<Rigidbody>().AddTorque(transform.forward);
            //carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            /*if (carriedObject.tag.Equals("BinLid"))
            {
                carriedObject.transform.position = lidPosition;
            }*/
            //carriedObject.transform.position = rayCastHit.transform.position;
            carriedObject.GetComponent<Rigidbody>().useGravity = true;
            source.PlayOneShot(dropSound);
            carriedObject = null;
        }
    }

    Collider GetMouseHoverObject(float range)
    {
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        

        // Debug ray
        Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 2f);
        //Debug.Log("Ray direction " + ray.direction.ToString());

        if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, range))
        {
            return rayCastHit.collider;
        }
        return null;

    }
}
