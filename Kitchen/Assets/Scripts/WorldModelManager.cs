using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldModelManager : MonoBehaviour {
    private List<PrimitiveAction> actions;
    private int actionIndex;
    private Dictionary<string, List<string>> objects;

    // Use this for initialization
    public WorldModelManager () {
        actionIndex = 0;
        actions = new List<PrimitiveAction>();
	}
	
    public void updateWorldModel(string pAction, GameObject elementOne, GameObject elementTwo)
    {
        Debug.Log("action " + pAction);
        Debug.Log("elementOne " + elementOne.tag);
        Debug.Log("elementTwo " + elementTwo.tag);

        //PrimitiveAction action = new PrimitiveAction(pAction, elementOne, elementTwo);
        actions.Add(new PrimitiveAction(pAction, elementOne, elementTwo));
        printActions();
    }

    private void printActions()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            Debug.Log("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.tag + " " + actions[i].ElementTwo.tag);
            if (actions[i].ElementOne.transform.parent != null && actions[i].ElementOne.transform.parent.GetComponent<SemanticCategory>())
            {
                if (actions[i].ElementOne.GetComponent<CorrectItem>())
                    Debug.Log("correct " + actions[i].ElementOne.tag);
            }
            if (actions[i].ElementTwo.transform.parent != null && actions[i].ElementTwo.transform.parent.GetComponent<SemanticCategory>())
            {
                if(actions[i].ElementTwo.GetComponent<CorrectItem>())
                   Debug.Log("correct " + actions[i].ElementTwo.tag);
            }
        }
    }
}
