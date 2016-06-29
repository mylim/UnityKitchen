using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

public class InputHandler : MonoBehaviour {
    Animator animator;
    bool open = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastHit;

            // Debug ray
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 2f);
            Debug.Log("Ray direction " + ray.direction.ToString());

            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, 1.5f) && rayCastHit.collider.isTrigger)
            {
                //Debug.Log(rayCastHit.collider.gameObject.name);
                //if (rayCastHit.collider.tag == "FridgeRightDoor")
                {
                    //FridgeDoorAnim fridgeDoorAnim = rayCastHit.transform.GetComponent<FridgeDoorAnim>();
                    animator = rayCastHit.transform.GetComponent<Animator>();
                    if (animator)
                    {
                        Debug.Log("Script found");
                        animator.SetBool("isOpened", !open);
                        open = !open;
                        //fridgeDoorAnim.PlayDoorAnim();
                    }
                    else
                    {
                        Debug.Log("Script not found");
                    }
                }
            }
        }
    }
}
