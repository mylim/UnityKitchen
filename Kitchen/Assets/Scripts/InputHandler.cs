using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {
    Animator animator;
    bool open = false;
    /*public bool openable;
    /Controller controller;

    // Use this for initialization
    void Start()
    {
        controller = new Controller();
    }*/

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastHit;

            // Debug ray
            // Debug.DrawRay(ray.origin, ray.direction, Color.green);

            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, 1.5f) && rayCastHit.collider.isTrigger)
            {
                //Debug.Log("Right mouse button click!");
                Debug.Log(rayCastHit.collider.gameObject.name);
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

        /*Frame frame = controller.Frame();
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                List<Finger> fingers = hand.Fingers;
                foreach (Finger finger in fingers)
                {
                    Finger.FingerType fingerType = finger.Type;
                    if (fingerType.Equals(Finger.FingerType.TYPE_INDEX))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayCastHit;

                        // Debug ray
                        // Debug.DrawRay(ray.origin, ray.direction, Color.green);

                        if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, 1.5f))
                        {
                            //Debug.Log("Right mouse button click!");
                            Debug.Log(rayCastHit.collider.gameObject.name);
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
        }*/

    }
}
