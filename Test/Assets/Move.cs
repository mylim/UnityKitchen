using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public float speed = 2.0F;
    public float rotationSpeed = 2.0F;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Vertical"))
        {
            float vertical = Input.GetAxis("Vertical") * speed;
            vertical *= Time.deltaTime;
            transform.Translate(0, 0, vertical);
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            float horizontal = Input.GetAxis("Horizontal") * speed;
            horizontal *= Time.deltaTime;
            transform.Translate(horizontal, 0, 0);
            //transform.Rotate(0.0f, -Input.GetAxis("Horizontal") * speed, 0.0f);
            // code for the movement of Cube1' forward
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.cube1.transform.Translate(Vector3.forward * Time.deltaTime);
            }
            // code for the movement of Cube1' backward
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.cube1.transform.Translate(Vector3.back * Time.deltaTime);
            }
            // code for the movement of Cube1' left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.cube1.transform.Translate(Vector3.left * Time.deltaTime);
            }
            // code for the movement of Cube1' right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.cube1.transform.Translate(Vector3.right * Time.deltaTime);
            }
        }

    }
}
