using UnityEngine;
using System.Collections.Generic;

public class ObjectsHandler : MonoBehaviour {    
    public List<GameObject> correctObjectsList;
    private Dictionary<string, GameObject> pickedObjectsDictionary; // picked individual objects
    private Dictionary<string, GameObject> pickedItemsDictionary; // picked object for a category

    // Use this for initialization
    void Start () {
        correctObjectsList = new List<GameObject>();
        pickedObjectsDictionary = new Dictionary<string, GameObject>();
        pickedItemsDictionary = new Dictionary<string, GameObject>();
    }

    public void addPickedObject(string tag, string parentTag, GameObject gameObject)
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
        if (parentTag != null)
        {
            if (!pickedItemsDictionary.ContainsKey(parentTag))
            {
                pickedItemsDictionary.Add(parentTag, gameObject);
                foreach (KeyValuePair<string, GameObject> pair in pickedItemsDictionary)
                {
                    Debug.Log(pair.Key + " " + pair.Value);
                }
            }
        }
    }

    public GameObject getPickedObject(string tag)
    {
        GameObject pickedObject;

        if (pickedObjectsDictionary.TryGetValue(tag, out pickedObject))
        {
            return pickedObject;
        }
        else
        {
            return null;
        }
    }

    public GameObject getPickedItem(string tag)
    {
        GameObject pickedItem;

        if (pickedItemsDictionary.TryGetValue(tag, out pickedItem))
        {
            return pickedItem;
        }
        else
        {
            return null;
        }
    }
}
