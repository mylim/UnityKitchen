using UnityEngine;
using System.Collections;

public class InstantiateItem : MonoBehaviour {
    public GameObject item;
    public bool instantiated;

    void Start()
    {
        instantiated = false;
    }
    // Use this for initialization
    public void Instantiate(Vector3 position, int num)
    {
        if (!instantiated)
        {
            for (int i = 0; i < num; i++)
            {
                Instantiate(item, position, item.transform.rotation);
            }
            instantiated = true;
        }
    }
}
