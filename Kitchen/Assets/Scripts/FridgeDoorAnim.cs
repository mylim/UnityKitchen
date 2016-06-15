using UnityEngine;
using System.Collections;

public class FridgeDoorAnim : MonoBehaviour {
    /*private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Open", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Open", false);
        }
    }*/
    //Animator fridgeAnimator;
    //bool doorOpen;

    /*void Start()
    {
        doorOpen = false;
        fridgeAnimator.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider fridgeCollider)
    {
        //if (fridgeCollider.gameObject.tag == "Player")
        if (!doorOpen)
        {
            doorOpen = true;
            DoorHandler("Open");
        }
    }

    void onTriggerExit(Collider fridgeCollider)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorHandler("Close");
        }
    }

    void DoorHandler(string state)
    {
        fridgeAnimator.SetTrigger(state);
    }*/

    /*Animator animator;
    bool doorOpen;

    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider fridgeCollider)
    {
        //if (fridgeCollider.gameObject.tag == "Player")
        if(Input.GetMouseButton(1) && !doorOpen)
        {
            doorOpen = true;
            DoorHandler(doorOpen);
        }
    }

    void onTriggerExit(Collider fridgeCollider)
    {
        if (Input.GetMouseButton(1) && doorOpen)
        {
            doorOpen = false;
            DoorHandler(doorOpen);
        }
    }

    void DoorHandler(bool state)
    {
        animator.SetBool("Open", state);
    }*/

    Animator animator;
    bool open = false;

    void Update () {
       
        animator = GetComponent<Animator>();
        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Open", !open);
            open = !open;
        }
	}

    /*bool doorOpen;

    public void PlayDoorAnim()
    {

        if (!GetComponent<Animation>().isPlaying)
        {
            if (!doorOpen)
            {
                GetComponent<Animation>().Play("FridgeRightDoorOpen");
                GetComponent<Animation>().Play("FridgeLeftDoorOpen");
                //Debug.Log("Right door animation!");
            }
            else
            {
                GetComponent<Animation>().Play("FridgeRightDoorClose");
                GetComponent<Animation>().Play("FridgeLeftDoorClose");
            }
            doorOpen = !doorOpen;
        }
    }*/

    }
