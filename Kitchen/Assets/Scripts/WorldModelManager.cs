using UnityEngine;
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

    // Scoring variables
    private bool taskFound;
    private int taskID;

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
            // element one has a semantic category
            eOne.SemanticCategory = elementOne.transform.parent.tag;
            // element one belongs to the right semantic category
            if(elementOne.transform.parent.GetComponent<CorrectSemanticCategory>())
            {
                eOne.CorrectSemanticCategory = true;
            }
            if (!objects.ContainsKey(elementOne.transform.parent.tag))
                objects.Add(elementOne.transform.parent.tag, elementOne);
            else
            {
                objects[elementOne.transform.parent.tag] = elementOne;
            }
        }
        if (elementTwo.transform.parent != null && elementTwo.transform.parent.GetComponent<SemanticCategory>())
        {
            // element two has a semantic category
            eTwo.SemanticCategory = elementTwo.transform.parent.tag;
            // element two belongs to the right semantic category
            if (elementTwo.transform.parent.GetComponent<CorrectSemanticCategory>())
            {
                eTwo.CorrectSemanticCategory = true;
            }
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

                if (HasSemanticCategory(actions[i].ElementOne))
                {
                    if (IsCorrectSemanticCategory(actions[i].ElementOne))
                    {
                        if (IsCorrectItem(actions[i].ElementOne.ObjectElement))
                        {
                            //Debug.Log("correct " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                            logFile.WriteLine("correct semantic category and correct " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                        }
                        else
                        {
                            logFile.WriteLine("correct semantic cateogry but wrong " + actions[i].ElementOne.ObjectElement.tag + ": " + actions[i].ElementOne.ObjectElement.name);
                        }
                    }
                    else
                    {
                        logFile.WriteLine("Incorrect semantic category " + actions[i].ElementOne.ObjectElement.tag);
                    }
                }
                if (HasSemanticCategory(actions[i].ElementTwo))
                {
                    if (IsCorrectSemanticCategory(actions[i].ElementTwo))
                    {
                        if (IsCorrectItem(actions[i].ElementTwo.ObjectElement))
                        {
                            //Debug.Log("correct " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                            logFile.WriteLine("Correct semantic category and correct " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                        }
                        else
                        {
                            logFile.WriteLine("Correct semantic category but wrong " + actions[i].ElementTwo.ObjectElement.tag + ": " + actions[i].ElementTwo.ObjectElement.name);
                        }
                    }
                    else
                    {
                        logFile.WriteLine("Incorrect semantic category " + actions[i].ElementTwo.ObjectElement.tag);
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

            /*for (int j = 0; j < xmlErrands.Count; j++)
            //foreach (XMLErrand errand in xmlErrands)
            {
                XMLErrand errand = xmlErrands[j];
                logFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
                for (int k = 0; k < errand.Subtasks.Count; k++)
                //foreach (XMLSubtask subtask in errand.Subtasks)
                {
                    XMLSubtask subtask = errand.Subtasks[k];
                    logFile.WriteLine("Subtask number " + k + " subtask ID " + subtask.ID);
                    logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement);
                }
                for (int l = 0; l < errand.AuxSubtasks.Count; l++)
                //foreach (XMLSubtask auxSubtask in errand.AuxSubtasks)
                {
                    XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                    logFile.WriteLine("Subtask number " + l + " auxSubtask ID " + auxSubtask.ID);
                    logFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
                }
                logFile.WriteLine("\n");
            }*/
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
        bool taskAdded = false;

        printActions();
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        {
            for (int i = 0; i < actions.Count; i++)
            {
                taskFound = false;
                taskID = 0;
                PrimitiveAction action = actions[i];
                Debug.Log("Action " + action.Name + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);
                scoreFile.WriteLine("\nAction " + action.Name + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);

                taskAdded = CheckSubtasks(action, scoreFile);
                if (!taskAdded)
                {
                    taskAdded = CheckAuxSubtasks(action, scoreFile);
                    if (!taskAdded)
                    {
                        if (taskFound)
                        {
                            AddRepetition(taskID, scoreFile);
                        }
                        else
                        {
                            AddUnknownIntrusion(scoreFile);
                        }                        
                    }
                    continue;
                }
            }
            for (int i = 0; i < executionList.Count; i++)
            {
                scoreFile.WriteLine("ErrandID " + executionList[i].ErrandID + " SubtaskNumber " + executionList[i].SubtaskNumber
                    + " TaskType " + executionList[i].TaskType.ToString() + (executionList[i].SemanticError ? " semanticError" : "") + 
                    (executionList[i].EpisodicError ? " episodicError": ""));
            }
        }
    }
    
    private bool CheckSubtasks(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];         
            for (int k = 0; k < errand.Subtasks.Count; k++)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                bool semanticError; // should be always false since the semantic categories are the same
                bool episodicError;
                // subtask with objects from same semantic category               
                //if (subtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(subtask.Action, action))
                if (subtask.Action.Name.Equals(action.Name) && AreSimilarObjects(subtask.Action, action))
                {
                    //Debug.Log("Errand " + j + " " + errand.Name + " Subtask " + (k + 1) + " ID " + subtask.ID + " and action are the same");
                    scoreFile.WriteLine("Errand " + j + " " + errand.Name + " Subtask " + (k + 1) + " ID " + subtask.ID + " and action are the same ");
                    //scoreFile.WriteLine("Objects " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement
                    // + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);
                    // similar objects
                    //if (AreSimilarObjects(subtask.Action, action))
                
                    //scoreFile.WriteLine("Objects are similar");
                    if (!executionXMLTasks.Contains(subtask))
                    {
                        //Debug.Log("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);

                        semanticError = CheckSemanticError(action); 
                        episodicError = CheckEpisodicError(action);
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, k + 1, Execution.TaskTypes.Subtask, semanticError, episodicError);
                            executionXMLTasks.Add(subtask);
                            executionList.Add(task);
                        }
                        else
                        {
                            scoreFile.WriteLine("There are some errors");
                            AddKnownIntrusion(scoreFile, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        taskID = k + 1;
                    }
                    
                }
                // different objects, which means there is semantic error
                else if (subtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(subtask.Action, action))
                {
                    semanticError = CheckSemanticError(action);
                    episodicError = CheckEpisodicError(action);
                    scoreFile.WriteLine("Objects " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement
                     + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);
                    /*scoreFile.WriteLine("Semantic categories " + subtask.Action.ElementOne.SemanticCategory + " " + subtask.Action.ElementTwo.SemanticCategory
                   + " " + action.ElementOne.SemanticCategory + " " + action.ElementTwo.SemanticCategory);
                    scoreFile.WriteLine("Objects are disimilar");*/
                    scoreFile.WriteLine("Objects has same semantic categories but are disimilar");
                    AddKnownIntrusion(scoreFile, semanticError, episodicError);
                    return true;
                }
              
                /*if (subtask.Action.Name.Equals(action.Name) && AreSimilarObjects(subtask.Action, action))
                {
                    Debug.Log("Errand " + j + " " + errand.Name + " Subtask " + (k + 1) + " ID " + subtask.ID +" and action are the same");
                    scoreFile.WriteLine("Errand " + j + " " + errand.Name + " Subtask " + (k + 1) + " ID " + subtask.ID + " and action are the same ");
                    if (!executionXMLTasks.Contains(subtask))
                    {
                        Debug.Log("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);

                        semanticError = CheckSemanticError(action);
                        episodicError = CheckEpisodicError(action);
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, k + 1, Execution.TaskTypes.Subtask, semanticError, episodicError);
                            executionXMLTasks.Add(subtask);
                            executionList.Add(task);
                        } 
                        else
                        {
                            AddKnownIntrusion(scoreFile, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        taskID = k + 1;
                    }
                }*/
            }
        }        
        return false;
    }   
    
    private bool CheckAuxSubtasks(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {       
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                if (auxSubtask.Action.Name.Equals(action.Name) && AreSimilarObjects(auxSubtask.Action, action))
                {
                    //Debug.Log("Errand " + j + " " + errand.Name + " auxSubtask " + (l + 1) + " ID " + auxSubtask.ID + " and action are the same");
                    scoreFile.WriteLine("Errand " + j + " " + errand.Name + " auxSubtask " + (l + 1) + " ID " + auxSubtask.ID + " and action are the same");
                    // add the auxSubtask to the checklist, auxSubtask will be ignored during scoring
                    if (!executionXMLTasks.Contains(auxSubtask))
                    {
                        //Debug.Log("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.Subtask);
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.Subtask);

                        bool semanticError = CheckSemanticError(action);
                        bool episodicError = CheckEpisodicError(action);
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, l + 1, Execution.TaskTypes.AuxTask, semanticError, episodicError);
                            executionXMLTasks.Add(auxSubtask);
                            executionList.Add(task);
                        }
                        else
                        {
                            AddKnownIntrusion(scoreFile, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        taskID = l + 1;
                    }
                }
                /*if (auxSubtask.Action.Name.Equals(action.Name) && IsSameObject(auxSubtask.Action, action))
                {
                    Debug.Log("Errand " + j + " " + errand.Name + " auxSubtask " + (l + 1) + " ID " + auxSubtask.ID + " and action are the same");
                    scoreFile.WriteLine("Errand " + j + " " + errand.Name + " auxSubtask " + (l + 1) + " ID " + auxSubtask.ID + " and action are the same");
                    // add the auxSubtask to the checklist, auxSubtask will be ignored during scoring
                    if (!executionXMLTasks.Contains(auxSubtask))
                    {
                        Debug.Log("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.Subtask);
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.Subtask);

                        bool semanticError = CheckSemanticError(action);
                        bool episodicError = CheckEpisodicError(action);
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, l + 1, Execution.TaskTypes.AuxTask, semanticError, episodicError);
                            executionXMLTasks.Add(auxSubtask);
                            executionList.Add(task);
                        }
                        else
                        {
                            AddKnownIntrusion(scoreFile, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        taskID = l + 1;
                    }
                }*/
            }
        }       
        return false;
    } 

    private void AddKnownIntrusion(System.IO.StreamWriter scoreFile, bool semanticError, bool episodicError)
    {      
        //Debug.Log("Adding known intrusion: semanticError " + semanticError + " episodicError " + episodicError);
        scoreFile.WriteLine("Adding known intrusion: semanticError " + semanticError + " episodicError " + episodicError);
        Execution task = new Execution("I", 0, Execution.TaskTypes.Intrusion, semanticError, episodicError);
        executionList.Add(task);        
    }

    private void AddUnknownIntrusion(System.IO.StreamWriter scoreFile)
    {
        //Debug.Log("Adding unknown intrusion");
        scoreFile.WriteLine("Adding unknown intrusion");
        Execution task = new Execution("I", 0, Execution.TaskTypes.Intrusion, false, false);
        executionList.Add(task);
    }

    private void AddRepetition(int taskID, System.IO.StreamWriter scoreFile)
    {
        //Debug.Log("Adding repetition");
        scoreFile.WriteLine("Adding repetition");
        Execution task = new Execution("R", taskID, Execution.TaskTypes.Repetition, false, false);
        //executionXMLTasks.Add(auxSubtask);
        executionList.Add(task);
    }

    private bool AreSameSemanticCategories(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        /*bool sameSemanticCategories = false;
        if (HasSemanticCategory(action.ElementOne)) {
            if (xmlAction.ElementOne.SemanticCategory.Equals(action.ElementOne.SemanticCategory))
            {
                sameSemanticCategories = true;
            }
        }
        if (HasSemanticCategory(action.ElementTwo))
        {
            if (xmlAction.ElementTwo.SemanticCategory.Equals(action.ElementTwo.SemanticCategory))
            {
                sameSemanticCategories = true;
            }
        }*/
        if (((HasSemanticCategory(action.ElementOne)) && (xmlAction.ElementOne.SemanticCategory.Equals(action.ElementOne.SemanticCategory)))
            && ((HasSemanticCategory(action.ElementTwo)) && (xmlAction.ElementTwo.SemanticCategory.Equals(action.ElementTwo.SemanticCategory))))
            return true;
        return false;
    }

    private bool AreSimilarObjects(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        //Debug.Log("xmlElementOne " + xmlAction.ElementOne.ObjectElement + " actionElementOne " + action.ElementOne.ObjectElement.tag + " xmlElementTwo " 
        //    + xmlAction.ElementTwo.ObjectElement + " actionElementTwo " + action.ElementTwo.ObjectElement.tag);
        if (xmlAction.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag) && xmlAction.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag))
            return true;
        return false;
    }

    private bool CheckSemanticError(PrimitiveAction action)
    {
        bool semanticError = false;
        // if the objects have semantic category
        if (HasSemanticCategory(action.ElementOne))
        {
            if (!IsCorrectSemanticCategory(action.ElementOne))
            {
                semanticError = true;
            }
        }
        if (HasSemanticCategory(action.ElementTwo))
        {
            if (!IsCorrectSemanticCategory(action.ElementTwo))
            {
                semanticError = true;
            }
        }      
        return semanticError;
    }

    private bool CheckEpisodicError(PrimitiveAction action)
    {
        bool episodicError = false;
        // if the objects have semantic category
      
        if (HasSemanticCategory(action.ElementOne))
        {
            if (!IsCorrectItem(action.ElementOne.ObjectElement))
            {
                episodicError = true;
            }
        }
        if (HasSemanticCategory(action.ElementTwo))
        {
            if (!IsCorrectItem(action.ElementTwo.ObjectElement))
            {
                episodicError = true;
            }
        }
     
        return episodicError;
    }
    private bool HasSemanticCategory(Element element)
    {
        if (!element.SemanticCategory.Equals(""))
            return true;
        return false;
    }

    private bool IsCorrectSemanticCategory(Element element)
    {
        if (element.CorrectSemanticCategory)
            return true;
        return false;
    }

    private bool IsCorrectItem(GameObject gObject)
    {
        if (gObject.GetComponent<CorrectItem>())
            return true;
        return false;
    }
}
