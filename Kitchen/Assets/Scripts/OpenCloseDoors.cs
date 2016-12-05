using UnityEngine;
using System.Collections;


/** 
Handles the opening and closing doors
*/

public class OpenCloseDoors : MonoBehaviour {
    Animator animator;
    bool open = false;

    //GameObject mainCamera;
    //RaycastHit rayCastHit;
   

    /*void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
       
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            OpenCloseDoor();
        }
    }

    /*Collider GetMouseHoverObject(float range)
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

    }*/

    // Opening and Closing Doors
    void OpenCloseDoor()
    {
        Collider collider = GetComponent<MouseHoverObject>().GetMouseHoverObject(2);

        if (collider != null)
        {
            if (collider.isTrigger)
            {
                animator = collider.GetComponent<Animator>();
                if (animator)
                {
                    Debug.Log("Script found");
                    animator.SetBool("isOpened", !open);
                    open = !open;
                }
                /*else if (collider.tag.Equals("BinBag"))
                {
                    Debug.Log("Bin Bag clicked");
                    collider.gameObject.SetActive(false);

                    if (tiedBag)
                    { 
                        Debug.Log("Tied Bag found");
                        tiedBag.SetActive(true);
                    }
                }*/
            }
        }
    }

}
