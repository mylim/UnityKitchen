using UnityEngine;
using System.Collections;

public class InstantiateBreadSlice : MonoBehaviour {
    public GameObject breadSlice;
    private static int NUM_BREAD = 2;

	// Use this for initialization
	public void InstantiateSlice (Vector3 position) {
        for (int i = 0; i < NUM_BREAD; i++)
        {
            Instantiate(breadSlice, position, breadSlice.transform.rotation);
        }
    }
}
