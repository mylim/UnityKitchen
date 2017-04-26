﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldModelManager : MonoBehaviour {
    public int interferenceInterval;
    public InterferenceDialog[] dialogs;
    private int dialogIndex;
    // List of primitive actions and interferences as they occured
    private List<PrimitiveAction> actions;
    private List<Interference> interferences;
    // Ordered list of execution 
    private List<Execution> executionList;
    private List<XMLSubtask> executionXMLTasks;
    private int actionIndex;
    private Dictionary<string, GameObject> objects;
    private bool interfering;
    private XMLParser xmlParser;
    private List<XMLErrand> xmlErrands;
    private List<XMLInterference> xmlInterferences;

    // Use this for initialization
    void Start() {
        actionIndex = 0;
        dialogIndex = 0;
        actions = new List<PrimitiveAction>();
        interferences = new List<Interference>();
        executionList = new List<Execution>();
        executionXMLTasks = new List<XMLSubtask>();
        objects = new Dictionary<string, GameObject>();
        xmlParser = new XMLParser();
        xmlErrands = xmlParser.ParseXMLErrands();
        xmlInterferences = xmlParser.ParseXMLInterferences();
        /*for (int j = 0; j < xmlErrands.Count; j++)
        //foreach (XMLErrand errand in xmlErrands)
        {
            XMLErrand errand = xmlErrands[j];
            Debug.Log("errand " + errand.Name + " ID " + errand.ID);
            for (int k = 0; k < errand.Subtasks.Count; k++)
            //foreach (XMLSubtask subtask in errand.Subtasks)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                Debug.Log("Subtask number " + k);
                Debug.Log("subtask " + subtask.ID);
                Debug.Log("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement);
            }
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            //foreach (XMLSubtask auxSubtask in errand.AuxSubtasks)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                Debug.Log("auxSubtask " + auxSubtask.ID);
                Debug.Log("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
            }
        }*/
        /*foreach (XMLInterference interference in xmlInterferences)
        {
            Debug.Log("Interference " + interference.Dialog);
            List<string> iObjects = interference.iObjects;
            foreach (string iObject in iObjects)
            {
                Debug.Log("Object " + iObject);
            }
        }*/
    }

    void Update() {
        if ((dialogIndex < dialogs.Length) && dialogs[dialogIndex].DialogClosed())
        {
            //Debug.Log("Answer: " + dialogs[dialogIndex].GetAnswer());
            UpdateInterference();
            dialogIndex++;
        }
    }

    public void UpdateWorldModel(string pAction, GameObject elementOne, GameObject elementTwo)
    {
        Debug.Log("action " + pAction);
        Debug.Log("elementOne " + elementOne.tag);
        Debug.Log("elementTwo " + elementTwo.tag);

        //adding the action performed
        Element eOne = new Element(elementOne);
        Element eTwo = new Element(elementTwo);        
        if (elementOne.transform.parent != null && elementOne.transform.parent.GetComponent<SemanticCategory>())
        {
            eOne.SemanticCategory = true;
            if (!objects.ContainsKey(elementOne.transform.parent.tag))
                objects.Add(elementOne.transform.parent.tag, elementOne);
            else
            {
                objects[elementOne.transform.parent.tag] = elementOne;
            }
        }
        if (elementTwo.transform.parent != null && elementTwo.transform.parent.GetComponent<SemanticCategory>())
        {
            eTwo.SemanticCategory = true;
            if (!objects.ContainsKey(elementTwo.transform.parent.tag))
                objects.Add(elementTwo.transform.parent.tag, elementTwo);
            else
            {
                objects[elementTwo.transform.parent.tag] = elementTwo;
            }
        }
        // adding the primitive action performed to the list of actions
        actions.Add(new PrimitiveAction(pAction, eOne, eTwo));
        Debug.Log("action count " + actions.Count);
        //+ "mod count " + actions.Count%10);
        //if ((actions.Count > 0) && ((actions.Count % interferenceInterval) == 0))
        //if (((actions.Count % interferenceInterval) == 0))
        if ((actions.Count > 0) && (actions.Count % interferenceInterval == 0) && (!interfering))
        {
            //Debug.Log("In update of world model " + dialogIndex);
            if ((dialogIndex < dialogs.Length))
            {
                Debug.Log("Dialog index to show" + dialogIndex);
                dialogs[dialogIndex].ShowDialog();
                UpdateInterference();
                // adding the interference action
                //actions.Add(new PrimitiveAction("Interference: " + dialogs[dialogIndex].name, new Element(), new Element()));
                interferences.Add(new Interference(dialogs[dialogIndex]));
                //Debug.Log("In show Dialog index " + dialogIndex + " interference " + interfering);
                //GetInterference();                
                //dialogs[dialogIndex].GetComponent<InterferenceDialog>().SetInterference();
            }
        }
    }

    public void InterfereWorldModel(GameObject iObject)
    {
        if ((dialogIndex < dialogs.Length))
        {
            interferences[dialogIndex].AddObject(iObject);
        }
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
                //Debug.Log("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementTwo.ObjectElement.tag);
                logFile.WriteLine("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementTwo.ObjectElement.tag);
                if (!actions[i].Name.Equals("Interference click"))
                {
                    if (actions[i].ElementOne.SemanticCategory)
                    {
                        if (actions[i].ElementOne.ObjectElement.GetComponent<CorrectItem>())
                        {
                            //Debug.Log("correct " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                            logFile.WriteLine("correct " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                        }
                        else
                        {
                            logFile.WriteLine("wrong " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                        }
                    }
                    if (actions[i].ElementTwo.SemanticCategory)
                    {
                        if (actions[i].ElementTwo.ObjectElement.GetComponent<CorrectItem>())
                        {
                            //Debug.Log("correct " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                            logFile.WriteLine("correct " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                        }
                        else
                        {
                            logFile.WriteLine("wrong " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                        }
                    }
                }
            }

            for (int j = 0; j < interferences.Count; j++)
            {
                logFile.WriteLine("Interference : " + interferences[j].Dialog.name);
                for (int k = 0; k < interferences[j].iObjects.Count; k++)
                {
                    logFile.WriteLine("Click : " + interferences[j].iObjects[k].tag);
                }
            }

            for (int j = 0; j < xmlErrands.Count; j++)
            //foreach (XMLErrand errand in xmlErrands)
            {
                XMLErrand errand = xmlErrands[j];
                logFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
                for (int k = 0; k < errand.Subtasks.Count; k++)
                //foreach (XMLSubtask subtask in errand.Subtasks)
                {
                    XMLSubtask subtask = errand.Subtasks[k];
                    logFile.WriteLine("Subtask number " + k + "subtask ID " + subtask.ID);
                    logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement);
                }
                for (int l = 0; l < errand.AuxSubtasks.Count; l++)
                //foreach (XMLSubtask auxSubtask in errand.AuxSubtasks)
                {
                    XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                    logFile.WriteLine("Subtask number " + l + "auxSubtask ID " + auxSubtask.ID);
                    logFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
                }
                logFile.WriteLine("\n");
            }
            /*if (objects != null)
            {
                foreach (KeyValuePair<string, GameObject> item in objects)
                {
                    if (item.Value.GetComponent<CorrectItem>())
                    {
                        Debug.Log("Item " + item.Key + ", value " + item.Value.name + ", correct");
                        logFile.WriteLine("Item " + item.Key + ", value " + item.Value.name + ", correct");
                    }
                    else
                    {
                        Debug.Log("Item " + item.Key + ", value " + item.Value.name + ", wrong");
                        logFile.WriteLine("Item " + item.Key + ", value " + item.Value.name + ", wrong");
                    }
                }
            }*/
        }

        /*if (errands != null)
        {
            using (System.IO.StreamWriter errandFile = new System.IO.StreamWriter(@"..\Logs\Errand.txt", true))
            {
                foreach (Errand errand in errands)
                {
                    Debug.Log("errand " + errand.Name);
                    errandFile.WriteLine("errand " + errand.Name);
                    List<Subtask> subtasks = errand.Subtasks;
                    foreach (Subtask subtask in subtasks)
                    {
                        Debug.Log("subtask " + subtask.ID);
                        errandFile.WriteLine("subtask " + subtask.ID);
                    }
                }
            }
        }*/
    }

    public void Score()
    {
        bool taskFound = false;

        printActions();
        for (int i = 0; i < actions.Count; i++)
        {
            PrimitiveAction action = actions[i];
            Debug.Log("Action " + action.Name + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);
            taskFound = CheckSubtasks(action);
            if (!taskFound)
            {
                taskFound = CheckAuxSubtasks(action);
                if (!taskFound)
                {
                    taskFound = AddIntrusion();
                }
                continue;
            }
            /*for (int j = 0; j < xmlErrands.Count; j++)
            {
                XMLErrand errand = xmlErrands[j];
                Debug.Log("Errand " + errand.Name);
                taskFound = CheckSubtasks(errand, action);
                if (!taskFound)
                {
                    taskFound = CheckAuxSubtasks(errand, action);
                    if (!taskFound)
                    {
                        taskFound = AddIntrusion();                        
                    }
                    continue;
                }
            }*/
        }
        for (int i = 0; i < executionList.Count; i++)
        {
            Debug.Log("Errand ID " + executionList[i].ErrandID + " SubtaskNumber " + executionList[i].SubtaskNumber 
                + " TaskType " + executionList[i].TaskType.ToString());
        }
    }
    
    private bool CheckSubtasks(PrimitiveAction action)
    {
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];
            //Debug.Log("Errand name " + errand.Name);
            for (int k = 0; k < errand.Subtasks.Count; k++)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                //Debug.Log("Subtask " + subtask.Action.Name);
                if (subtask.Action.Name.Equals(action.Name) && IsSameObject(subtask.Action, action))
                {
                    Debug.Log("Errand " + j + " " + errand.Name + " Subtask " + k + " and action name are the same");
                    if (!executionXMLTasks.Contains(subtask))
                    {
                        Debug.Log("Errand ID, k and type " + errand.ID + " " + k + " " + Execution.TaskTypes.Subtask);
                        Execution task = new Execution(errand.ID, k+1, Execution.TaskTypes.Subtask);
                        executionXMLTasks.Add(subtask);
                        executionList.Add(task);
                        return true;
                    }
                }
            }
        }
        return false;
    }   
    
    private bool CheckAuxSubtasks(PrimitiveAction action)
    {
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];
            Debug.Log("Errand " + errand.Name);
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                Debug.Log("auxSubtask " + auxSubtask.Action.Name);
                if (auxSubtask.Action.Name.Equals(action.Name) && IsSameObject(auxSubtask.Action, action))
                {
                    Debug.Log("Errand " + j + " " + errand.Name + " auxSubtask " + l + " and action name are the same");
                    // add the auxSubtask to the checklist, auxSubtask will be ignored during scoring
                    if (!executionXMLTasks.Contains(auxSubtask))
                    {
                        Debug.Log("Errand ID, k and type " + errand.ID + " " + l + " " + Execution.TaskTypes.Subtask);
                        Execution task = new Execution(errand.ID, l+1, Execution.TaskTypes.AuxTask);
                        executionXMLTasks.Add(auxSubtask);
                        executionList.Add(task);
                        return true;
                    }
                }
            }
        }
        return false;
    } 

    private bool AddIntrusion()
    {
        Debug.Log("Adding intrusion");
        Execution task = new Execution("I", 0, Execution.TaskTypes.Intrusion);
        //executionXMLTasks.Add(auxSubtask);
        executionList.Add(task);
        return false;
    }

    private bool IsSameObject(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        if (xmlAction.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag) && xmlAction.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag))
            return true;
        return false;
    }
}
