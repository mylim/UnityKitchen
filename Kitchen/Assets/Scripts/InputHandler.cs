using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;

public class InputHandler : MonoBehaviour {
    Animator animator;
    bool open = false;
    //public bool openable;
    //Controller controller;

    //HandModel hand_model;
    //Hand leap_hand;

    // Use this for initialization
    void Start()
    {
        //controller = new Controller();

        /*hand_model = GetComponent<HandModel>();
        leap_hand = hand_model.GetLeapHand();
        if (leap_hand == null) Debug.LogError("No leap_hand founded");*/
    }

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
                //Debug.Log("Right mouse button click!");
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
                        Ray ray = Camera.main.ScreenPointToRay(finger.Direction.ToVector3());
                        Debug.Log("Finger position " + finger.TipPosition.ToVector3().ToString());
                        Debug.Log("Finger direction" + finger.Direction.ToVector3().ToString());
                        Debug.Log("Ray direction " + ray.direction.ToString());
                        Debug.Log("Ray origin " + ray.origin.ToString());
                        RaycastHit rayCastHit;

                        // Debug ray
                        Debug.DrawRay(finger.TipPosition.ToVector3(), finger.Direction.ToVector3() * 10f, Color.red, 2f);
                        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.green, 2f);

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
                }
            }
        }*/

        /*for (int i = 0; i < HandModel.NUM_FINGERS; i++)
        {
            FingerModel finger = hand_model.fingers[i];
            Debug.Log("Finger " + finger.name);
            // draw ray from finger tips (enable Gizmos in Game window to see)
            Debug.DrawRay(finger.GetTipPosition(), finger.GetRay().direction, Color.red);
        }*/

    }
}
