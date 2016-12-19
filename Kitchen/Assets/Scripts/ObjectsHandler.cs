using UnityEngine;
using System.Collections.Generic;

public class ObjectsHandler : MonoBehaviour {    
    public List<GameObject> correctObjectsList;
    private Dictionary<string, GameObject> pickedObjectsDictionary;

    // Use this for initialization
    void Start () {
        correctObjectsList = new List<GameObject>();
        pickedObjectsDictionary = new Dictionary<string, GameObject>();
    }

    public void addPickedObject(string tag, GameObject gameObject)
    {
        Debug.Log("Adding picked object " + tag);
        if (!pickedObjectsDictionary.ContainsKey(tag))
        {
            pickedObjectsDictionary.Add(tag, gameObject);
            foreach (KeyValuePair<string, GameObject> pair in pickedObjectsDictionary)
            {
                Debug.Log(pair.Key + " " + pair.Value);
            }
        }
        
    }
}
