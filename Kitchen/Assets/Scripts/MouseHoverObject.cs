using UnityEngine;
using System.Collections;

public class MouseHoverObject : MonoBehaviour {
    private GameObject mainCamera;
    private RaycastHit rayCastHit;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");

    }


    public Collider GetMouseHoverObject(float range)
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
