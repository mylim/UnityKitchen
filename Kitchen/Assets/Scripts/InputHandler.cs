using UnityEngine;
using System.Collections;

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
            // Debug.DrawRay(ray.origin, ray.direction, Color.green);

            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit, Mathf.Infinity))
            {
                //Debug.Log("Right mouse button click!");
                Debug.Log( rayCastHit.collider.gameObject.name );
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
