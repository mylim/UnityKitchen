﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldModelManager : MonoBehaviour {
    public int interferenceInterval;
    public InterferenceDialog[] dialogs;
    private int dialogIndex;
    private List<PrimitiveAction> actions;
    private int actionIndex;
    private Dictionary<string, GameObject> objects;
    private bool interfering;

    // Use this for initialization
    void Start() {
        actionIndex = 0;
        dialogIndex = 0;
        actions = new List<PrimitiveAction>();
        objects = new Dictionary<string, GameObject>();       
    }

    void Update() {
        if (dialogs[dialogIndex].DialogClosed())
        {
            UpdateInterference();
            dialogIndex++;
        }
    }

    public void updateWorldModel(string pAction, GameObject elementOne, GameObject elementTwo)
    {
        Debug.Log("action " + pAction);
        Debug.Log("elementOne " + elementOne.tag);
        Debug.Log("elementTwo " + elementTwo.tag);

        //PrimitiveAction action = new PrimitiveAction(pAction, elementOne, elementTwo);
        actions.Add(new PrimitiveAction(pAction, elementOne, elementTwo));
        if (elementOne.transform.parent != null && elementOne.transform.parent.GetComponent<SemanticCategory>())
        {
            if (!objects.ContainsKey(elementOne.transform.parent.tag))
                objects.Add(elementOne.transform.parent.tag, elementOne);
            else
            {
                objects[elementOne.transform.parent.tag] = elementOne;
            }
        }
        if (elementTwo.transform.parent != null && elementTwo.transform.parent.GetComponent<SemanticCategory>())
        {
            if (!objects.ContainsKey(elementTwo.transform.parent.tag))
                objects.Add(elementTwo.transform.parent.tag, elementTwo);
            else
            {
                objects[elementTwo.transform.parent.tag] = elementTwo;
            }
        }
        Debug.Log("action count " + actions.Count);
        //+ "mod count " + actions.Count%10);
        //if ((actions.Count > 0) && ((actions.Count % interferenceInterval) == 0))
        //if (((actions.Count % interferenceInterval) == 0))
        if ((actions.Count > 0) && (actions.Count % interferenceInterval == 0) && (!interfering))
        {
            Debug.Log("In update of world model ");
            if ((dialogIndex < dialogs.Length))
            {
                //Debug.Log("Dialog index to show" + dialogIndex);
                dialogs[dialogIndex].ShowDialog();
                UpdateInterference();
                //Debug.Log("In show Dialog index " + dialogIndex + " interference " + interfering);
                //GetInterference();                
                //dialogs[dialogIndex].GetComponent<InterferenceDialog>().SetInterference();
            }
        }
        printActions();
    }

    private void UpdateInterference()
    {
        interfering = dialogs[dialogIndex].GetInterference();
        //Debug.Log("In update Inteference dialog index " + dialogIndex + " interference " + interfering);
    }

    public bool GetInterference()
    {
        //Debug.Log("Dialog index " + dialogIndex + " interference " + interfering);
        return interfering;
    }

    private void printActions()
    {
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_log.txt", true))
        {
            for (int i = 0; i < actions.Count; i++)
            {
                Debug.Log("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.tag + " " + actions[i].ElementTwo.tag);
                logFile.WriteLine("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.tag + " " + actions[i].ElementTwo.tag);
                /*if (actions[i].ElementOne.transform.parent != null && actions[i].ElementOne.transform.parent.GetComponent<SemanticCategory>())
                {
                    if (actions[i].ElementOne.GetComponent<CorrectItem>())
                        Debug.Log("correct " + actions[i].ElementOne.tag);
                }
                if (actions[i].ElementTwo.transform.parent != null && actions[i].ElementTwo.transform.parent.GetComponent<SemanticCategory>())
                {
                    if(actions[i].ElementTwo.GetComponent<CorrectItem>())
                        Debug.Log("correct " + actions[i].ElementTwo.tag);
                }*/
            }

            if (objects != null)
            {
                foreach (KeyValuePair<string, GameObject> item in objects)
                {
                    Debug.Log("Item " + item.Key + ", value " + item.Value.name);
                    logFile.WriteLine("Item " + item.Key + ", value " + item.Value.name);
                }
            }
        }
    }
}
