using UnityEngine;
using System.Collections;

public class alive : MonoBehaviour {
    public float speed = 6.0F;
    private Vector3 moveDirection = Vector3.zero;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame    
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;           
        }
            controller.Move(moveDirection * Time.deltaTime);
    }
}
