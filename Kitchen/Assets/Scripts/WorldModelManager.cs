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
    private List<XMLSubtask> executionXMLTasks;
    private List<Execution> intrusionRepetitionErrors;  
    private List<Execution> errandOrderErrors;
    private List<Execution> orderErrors;
    private List<Execution> missErrors;
    private int actionIndex;
    private Dictionary<string, GameObject> objects;
    private bool interfering;
    private XMLParser xmlParser;
    private List<XMLErrand> xmlErrands;
    private List<XMLInterference> xmlInterferences;

    // Scoring variables
    private bool taskFound;
    private string errandID;
    private int taskID;

    // Use this for initialization
    void Start() {
        actionIndex = 0;
        dialogIndex = 0;
        actions = new List<PrimitiveAction>();
        interferences = new List<Interference>();       
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

    /// <summary>
    /// update the interference status every cycle 
    /// </summary>
    void Update()
    {
        if ((dialogIndex < dialogs.Length) && dialogs[dialogIndex].DialogClosed())
        {
            UpdateInterference();
            interferences[dialogIndex].Answer = dialogs[dialogIndex].GetAnswer();
            dialogIndex++;
        }
    }

    /// <summary>
    /// Update the world model with the new action performed by the user
    /// </summary>
    /// <param name="pAction">the action name</param>
    /// <param name="elementOne">interaction object one</param>
    /// <param name="elementTwo">interaction object two</param>
    public void UpdateWorldModel(string pAction, GameObject elementOne, GameObject elementTwo)
    {
        /*Debug.Log("action " + pAction);
        Debug.Log("elementOne " + elementOne.tag);
        Debug.Log("elementTwo " + elementTwo.tag);*/

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

            /*if (!objects.ContainsKey(elementOne.transform.parent.tag))
                objects.Add(elementOne.transform.parent.tag, elementOne);
            else
            {
                objects[elementOne.transform.parent.tag] = elementOne;
            }*/
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

            /*if (!objects.ContainsKey(elementTwo.transform.parent.tag))
            {
                objects.Add(elementTwo.transform.parent.tag, elementTwo);
            }
            else
            {
                objects[elementTwo.transform.parent.tag] = elementTwo;
            }*/
        }
        // adding the primitive action performed to the list of actions
        actions.Add(new PrimitiveAction(pAction, eOne, eTwo));

        // activate the interference is the interval is reached and no other interference is active
        if ((actions.Count > 0) && (actions.Count % interferenceInterval == 0) && (!interfering))
        {
            if ((dialogIndex < dialogs.Length))
            {
                Debug.Log("Dialog index to show" + dialogIndex);
                dialogs[dialogIndex].ShowDialog();
                UpdateInterference();
                // adding the interference action            
                interferences.Add(new Interference(dialogs[dialogIndex]));             
            }
        }
    }

    /// <summary>
    /// Interference occurred
    /// </summary>
    /// <param name="iObject">object to be added to the object list</param>
    public void InterfereWorldModel(GameObject iObject)
    {
        if ((dialogIndex < dialogs.Length))
        {
            interferences[dialogIndex].AddObject(iObject);
        }
    }

    /// <summary>
    /// Update the state of interfering variable
    /// </summary>
    private void UpdateInterference()
    {
        interfering = dialogs[dialogIndex].GetInterference();
    }

    /// <summary>
    /// Get the interfering value
    /// </summary>
    /// <returns></returns>
    public bool GetInterference()
    {
        return interfering;
    }

    /// <summary>
    /// Logging all the actions to a text file with post-fix _log.txt in the Logs folder 
    /// </summary>
    private void LogActions()
    {
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_log.txt", true))
        {
            for (int j = 0; j < xmlErrands.Count; j++)
            {
                XMLErrand errand = xmlErrands[j];
                logFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
                for (int k = 0; k < errand.Subtasks.Count; k++)
                {
                    XMLSubtask subtask = errand.Subtasks[k];
                    logFile.WriteLine("sSubtask ID " + subtask.ID + " Subtask number " + k);
                    logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementOne.SemanticCategory 
                        + " " + subtask.Action.ElementTwo.ObjectElement + " " + subtask.Action.ElementTwo.SemanticCategory);
                }
                for (int l = 0; l < errand.AuxSubtasks.Count; l++)
                {
                    XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                    logFile.WriteLine("auxSubtask " + auxSubtask.ID);
                    logFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
                }
            }

            logFile.WriteLine();
            for (int i = 0; i < actions.Count; i++)
            {
                logFile.WriteLine("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementTwo.ObjectElement.tag);

                // if ElementOne of the action has semantic category
                if (HasSemanticCategory(actions[i].ElementOne))
                {
                    if (IsCorrectSemanticCategory(actions[i].ElementOne))
                    {
                        if (IsCorrectItem(actions[i].ElementOne.ObjectElement))
                        {
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
                // if ElementTwo of the action has semantic category
                if (HasSemanticCategory(actions[i].ElementTwo))
                {
                    if (IsCorrectSemanticCategory(actions[i].ElementTwo))
                    {
                        if (IsCorrectItem(actions[i].ElementTwo.ObjectElement))
                        {
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

          
        }
    }

    /// <summary>
    /// Logging all the interferences to a text file with post-fix _interference.txt in the Logs folder 
    /// </summary>
    private void LogInterferences()
    {
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter interferenceFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_interference.txt", true))
        {
            for (int j = 0; j < interferences.Count; j++)
            {
                interferenceFile.WriteLine("Interference : " + interferences[j].Dialog.name);
                for (int k = 0; k < interferences[j].iObjects.Count; k++)
                {
                    interferenceFile.WriteLine("Click : " + interferences[j].iObjects[k].tag);
                }
                interferenceFile.WriteLine("Answer : " + interferences[j].Answer);
            }
        }
    }

    /// <summary>
    /// Score the action of the user at the same time write to a text file with post-fix _score.txt in the Logs folder 
    /// </summary>
    public void Score()
    {
        LogActions();
        LogInterferences();
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        {
            // Checking for intrusion and repetition
            CheckIntrusionRepetition(scoreFile);

            // Checking for errand order error
            CheckErrandOrderError(scoreFile);

            // Checking the order error
            CheckOrderError(scoreFile);

            // Checking the missing subtasks
            CheckMissError(scoreFile);
            /*// Separate the task in the execution list into errands
            List<ErrandExecution> errandsExecutionList = new List<ErrandExecution>();
            ErrandExecution errandsExecution = new ErrandExecution();
            errandsExecutionList.Add(errandsExecution);
            // the first errand id should be the id of the first errand
            //int errandIndex = 0;
            //string errandID = xmlErrands[errandIndex++].ID;
            string errandID = executionList[0].ErrandID;
            string tempErrandID = "";

            for (int i = 0; i < executionList.Count; i++)
            {
                Execution exe = new Execution();
                exe.CopyExecution(executionList[i]);

                // if the execution list errandID is the same as the current errandID, add the subtask to the current errand
                if (executionList[i].ErrandID.Equals(errandID))
                {
                    errandsExecution.ExecutionList.Add(exe);
                    tempErrandID = "";
                }
                // if the execution list errandID is I (intrusion) or R (repetition), add the subtask to the current errand
                else if (executionList[i].TaskType.Equals(Execution.TaskTypes.Intrusion) || executionList[i].TaskType.Equals(Execution.TaskTypes.Repetition))
                {
                    errandsExecution.ExecutionList.Add(exe);
                }
                // if the execution list errandID is different from the current errandID and it is not the next errand index
                else if (!executionList[i].ErrandID.Equals(errandID))
                    //&& !executionList[i].ErrandID.Equals(xmlErrands[errandIndex].ID))
                {
                    // the subtask errandID is the same as the tempErrandID, so, this is a task that follows the errand order subtask
                    if (executionList[i].ErrandID.Equals(tempErrandID))
                    {
                        exe.TaskType = Execution.TaskTypes.ErrandOrder;
                        errandsExecution.ExecutionList.Add(exe);
                    }
                    else if ((i + 1) < executionList.Count)
                    {
                        // if the next subtask in the execution list has the same errandID as the current errandID, this subtask is an errand order error
                        if (executionList[i + 1].ErrandID.Equals(errandID) || executionList[i + 1].TaskType.Equals(Execution.TaskTypes.Intrusion) || executionList[i + 1].TaskType.Equals(Execution.TaskTypes.Repetition))
                        {
                            Execution eoSubtask = new Execution("EO", executionList[i].SubtaskNumber,
                                Execution.TaskTypes.ErrandOrder, executionList[i].SemanticError, executionList[i].EpisodicError);
                            errandsExecution.ExecutionList.Add(eoSubtask);
                        }
                        // if the 2nd next subtask errandID in the execution list is different from the current errandID
                        else if (!executionList[i + 1].ErrandID.Equals(errandID))
                        {
                            if ((i + 2) < executionList.Count)
                            {
                                // if the 3rd next subtask errandID in the execution list is the same as the current errandID, 
                                // this subtask is an errand order error
                                if (executionList[i + 2].ErrandID.Equals(errandID) || executionList[i + 2].TaskType.Equals(Execution.TaskTypes.Intrusion) || executionList[i + 2].TaskType.Equals(Execution.TaskTypes.Repetition))
                                {
                                    tempErrandID = executionList[i].ErrandID;
                                    Execution eoSubtask = new Execution("EO", executionList[i].SubtaskNumber,
                                   Execution.TaskTypes.ErrandOrder, executionList[i].SemanticError, executionList[i].EpisodicError);
                                    errandsExecution.ExecutionList.Add(eoSubtask);
                                }
                                // if the 3rd next subtask errandID in the execution list is also different from the current errandID, 
                                // assume that the participant has switched errand
                                else if (!executionList[i + 2].ErrandID.Equals(errandID))
                                {                                  
                                    scoreFile.WriteLine("New errand " + exe.ErrandID + " SubtaskNumber " + exe.SubtaskNumber
                                        + " TaskType " + exe.TaskType.ToString() + (exe.SemanticError ? " semanticError" : "") +
                                        (exe.EpisodicError ? " episodicError" : ""));
                                    errandID = executionList[i].ErrandID;
                                    tempErrandID = "";
                                    errandsExecution = new ErrandExecution();
                                    errandsExecutionList.Add(errandsExecution);
                                    errandsExecution.ExecutionList.Add(exe);                                 
                                }
                            }
                            // this is the second last subtask
                            else
                            {
                                tempErrandID = executionList[i].ErrandID;
                                Execution eoSubtask = new Execution("EO", executionList[i].SubtaskNumber,
                                    Execution.TaskTypes.ErrandOrder, executionList[i].SemanticError, executionList[i].EpisodicError);
                                errandsExecution.ExecutionList.Add(eoSubtask);
                            }   
                        }
                    }
                    // this is the last subtask
                    else
                    {
                        Execution eoSubtask = new Execution("EO", executionList[i].SubtaskNumber,
                            Execution.TaskTypes.ErrandOrder, executionList[i].SemanticError, executionList[i].EpisodicError);
                        errandsExecution.ExecutionList.Add(eoSubtask);
                    }
                }
            }

            scoreFile.WriteLine("Count " + errandsExecutionList.Count);

            for (int i = 0; i < errandsExecutionList.Count; i++)
            {
                ErrandExecution errand = errandsExecutionList[i];
                for (int j = 0; j < errand.ExecutionList.Count; j++)
                {
                    scoreFile.WriteLine("ErrandID " + errand.ExecutionList[j].ErrandID + " SubtaskNumber " + errand.ExecutionList[j].SubtaskNumber
                    + " TaskType " + errand.ExecutionList[j].TaskType.ToString() + (errand.ExecutionList[j].SemanticError ? " semanticError" : "") +
                    (errand.ExecutionList[j].EpisodicError ? " episodicError" : ""));
                }
            }*/
        }
    }

    /// <summary>
    /// Check for intrusion and repetition
    /// </summary>
    /// <param name="scoreFile"></param>
    private void CheckIntrusionRepetition(System.IO.StreamWriter scoreFile)
    {
        intrusionRepetitionErrors = new List<Execution>();
        executionXMLTasks = new List<XMLSubtask>();
        bool taskAdded = false;

        for (int i = 0; i < actions.Count; i++)
        {
            taskFound = false;
            taskID = 0;
            PrimitiveAction action = actions[i];
            scoreFile.WriteLine("\nAction " + action.Name + " " + action.ElementOne.ObjectElement.tag + " " + action.ElementTwo.ObjectElement.tag);

            // check is a subtask exists
            taskAdded = CheckSubtasks(action, scoreFile);
            // if subtask does not exist
            if (!taskAdded)
            {
                // check the auxSubtask list
                taskAdded = CheckAuxSubtasks(action, scoreFile);
                if (!taskAdded)
                {
                    // if subtask was found but not added, that means it is a repetition of an already added task
                    if (taskFound)
                    {
                        AddRepetition(scoreFile);
                    }
                    // if the subtask was not found, it is an intrusion
                    else
                    {
                        AddUnknownIntrusion(scoreFile);
                    }
                }
                continue;
            }
        }

        scoreFile.WriteLine();
        scoreFile.WriteLine("Intrusion Repetition Error");
        for (int i = 0; i < intrusionRepetitionErrors.Count; i++)
        {
            Execution execution = intrusionRepetitionErrors[i];
            string error = "";
            for (int j = 0; j < execution.Errors.Count; j++)
            {
                error += " " + execution.Errors[j];
            }
            scoreFile.WriteLine("ErrandID " + execution.ErrandID + " SubtaskNumber " + execution.SubtaskNumber
                + " TaskType " + execution.TaskType.ToString() + " Error " + error);
        }
    }

    /// <summary>
    /// Checking if the action performed is a subtask of an errand
    /// </summary>
    /// <param name="action">action performed by the user</param>
    /// <param name="scoreFile">the file to be written to</param>
    /// <returns></returns>
    private bool CheckSubtasks(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];         
            for (int k = 0; k < errand.Subtasks.Count; k++)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                bool semanticError = false;
                bool episodicError = false;
                // if the subtask has semantic category
                if (HasSemanticCategorySubtask(subtask.Action.ElementOne) || HasSemanticCategorySubtask(subtask.Action.ElementTwo))
                {
                    // check whether there is semantic or episodic error
                    semanticError = CheckSemanticError(subtask.Action, action);
                    episodicError = CheckEpisodicError(subtask.Action, action);
                }

                // subtask with similar objects, no semantic error as objects are from same semantic category but
                // there can still be episodic error if a different object from the same category is , e.g. blue mug vs green mug
                if (subtask.Action.Name.Equals(action.Name) && AreSimilarObjects(subtask.Action, action))
                {
                    //scoreFile.WriteLine("Errand " + j + " " + errand.Name + " Subtask " + (k + 1) + " ID " + subtask.ID + " and action are the same ");
                    if (!executionXMLTasks.Contains(subtask))
                    {
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k+1) + " " + Execution.TaskTypes.Subtask);                     
                        // If no error, add the task as checked
                        if (!semanticError && !episodicError)
                        { 
                            executionXMLTasks.Add(subtask);
                            AddTask(errand.ID, (k + 1), Execution.TaskTypes.Subtask);
                        }
                        // Different objects from the same category is picked
                        else
                        {
                            scoreFile.WriteLine("There is an error in the subtask");
                            AddKnownIntrusion(scoreFile, errand.ID, (k + 1), Execution.TaskTypes.Subtask, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        errandID = errand.ID;
                        taskID = k + 1;
                    }                    
                }
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (subtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(subtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k+1) + " " + Execution.TaskTypes.Subtask);
                    scoreFile.WriteLine("Subtask objects has same semantic categories but are disimilar");
                    AddKnownIntrusion(scoreFile, errand.ID, (k + 1), Execution.TaskTypes.Subtask, semanticError, episodicError);
                    return true;
                }
            }
        }        
        return false;
    }

    /// <summary>
    /// Add the task to the list
    /// </summary>
    /// <param name="errandID"></param>
    /// <param name="subtaskNumber"></param>
    /// <param name="taskType"></param>
    /// <param name="semanticError"></param>
    /// <param name="episodicError"></param>
    private void AddTask(string errandID, int subtaskNumber, Execution.TaskTypes taskType)
    {
        Execution task = new Execution(errandID, subtaskNumber, taskType);
        intrusionRepetitionErrors.Add(task);
    }

    /// <summary>
    /// Checking if the action performed is an auxSubtask of an errand
    /// </summary>
    /// <param name="action">action performed by the user</param>
    /// <param name="scoreFile">the file to be written to</param>
    /// <returns></returns>
    private bool CheckAuxSubtasks(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {       
        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];           
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                bool semanticError = false;
                bool episodicError = false;
                // if the subtask has semantic category
                if (HasSemanticCategorySubtask(auxSubtask.Action.ElementOne) || HasSemanticCategorySubtask(auxSubtask.Action.ElementTwo))
                {
                    // check whether there is semantic or episodic error
                    semanticError = CheckSemanticError(auxSubtask.Action, action);
                    episodicError = CheckEpisodicError(auxSubtask.Action, action);
                }

                // auxSubtask with similar objects, no semantic error as objects are from same semantic category but
                // there can still be episodic error if a different object from the same category is , e.g. blue mug vs green mug
                if (auxSubtask.Action.Name.Equals(action.Name) && AreSimilarObjects(auxSubtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.AuxTask);
                    // If no error, add the auxSubtask
                    if (!semanticError && !episodicError)
                    {                                         
                        if (!executionXMLTasks.Contains(auxSubtask))
                            executionXMLTasks.Add(auxSubtask);
                        AddTask(errand.ID, (l + 1), Execution.TaskTypes.AuxTask);
                    }
                    // Different objects from the same category is picked
                    else
                    {
                        scoreFile.WriteLine("There is an error in the auxSubtask");                       
                        AddKnownIntrusion(scoreFile, errand.ID, (l + 1), Execution.TaskTypes.AuxTask, semanticError, episodicError);
                    }
                    return true;
                    /*// add the auxSubtask to the checklist, auxSubtask will be ignored during scoring
                    if (!executionXMLTasks.Contains(auxSubtask))
                    {
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + l+1 + " " + Execution.TaskTypes.AuxTask);
                        // If no error, add the auxSubtask
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, l+1, Execution.TaskTypes.AuxTask, semanticError, episodicError);
                            executionXMLTasks.Add(auxSubtask);
                            intrusionRepetitionErrors.Add(task);
                        }
                        // Different objects from the same category is picked
                        else
                        {
                            scoreFile.WriteLine("There is an error in the auxSubtask");
                            errandID = errand.ID;
                            taskID = l + 1;
                            AddKnownIntrusion(scoreFile, semanticError, episodicError);
                        }
                        return true;
                    }
                    else
                    {
                        taskFound = true;
                        errandID = errand.ID;
                        taskID = l + 1;
                    }*/
                }
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (auxSubtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(auxSubtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l+1) + " " + Execution.TaskTypes.AuxTask);
                    scoreFile.WriteLine("AuxSubtask objects has same semantic categories but are disimilar");                  
                    AddKnownIntrusion(scoreFile, errand.ID, (l + 1), Execution.TaskTypes.AuxTask, semanticError, episodicError);
                    return true;
                }
            }
        }       
        return false;
    } 

    /// <summary>
    ///  Adding intrusion of known task with semantic and/or episodic error
    /// </summary>
    /// <param name="scoreFile">the file to be written to</param>
    /// <param name="semanticError">whether there is a semantic error</param>
    /// <param name="episodicError">whether there is an episodic error</param>
    private void AddKnownIntrusion(System.IO.StreamWriter scoreFile, string errandID, int subtaskNumber, Execution.TaskTypes taskType, bool semanticError, bool episodicError)
    {
        Execution task = new Execution(errandID, subtaskNumber, taskType);
        if (semanticError)
        {
            scoreFile.WriteLine("Adding semanticError ");
            task.Errors.Add(Execution.ErrorTypes.Semantic);
        }
        else if (episodicError)
        {
            scoreFile.WriteLine("Adding episodicError ");
            task.Errors.Add(Execution.ErrorTypes.Episodic);
        }      
        intrusionRepetitionErrors.Add(task);        
    }

    /// <summary>
    /// Adding unknown intrusion
    /// </summary>
    /// <param name="scoreFile">the file to be written to</param>
    private void AddUnknownIntrusion(System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine("Adding unknown intrusion");
        Execution task = new Execution("I", 0, Execution.TaskTypes.None);
        task.Errors.Add(Execution.ErrorTypes.Intrusion);
        intrusionRepetitionErrors.Add(task);
    }

    /// <summary>
    /// Adding repetition of task
    /// </summary>
    /// <param name="taskID">the repeated task</param>
    /// <param name="scoreFile">the file to be written to</param>
    private void AddRepetition(System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine("Adding repetition");
        Execution task = new Execution(errandID, taskID, Execution.TaskTypes.Subtask);
        task.Errors.Add(Execution.ErrorTypes.Repetition);
        intrusionRepetitionErrors.Add(task);
    }

    /// <summary>
    /// Are the objects of the action from the same semantic family, eg. BeverageContainers: Mug, CoffeeMug, Cup, Jar
    /// </summary>
    /// <param name="xmlAction">action loaded from the xml file</param>
    /// <param name="action">current action performed by the user</param>
    /// <returns></returns>
    private bool AreSameSemanticCategories(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        // if there is a semantic category, make sure they are the same, if there isn't, make sure the element are the same
        if ((((HasSemanticCategorySubtask(xmlAction.ElementOne)) && (xmlAction.ElementOne.SemanticCategory.Equals(action.ElementOne.SemanticCategory))) 
            || (xmlAction.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag)))
            && (((HasSemanticCategorySubtask(xmlAction.ElementTwo)) && (xmlAction.ElementTwo.SemanticCategory.Equals(action.ElementTwo.SemanticCategory)))
            || (xmlAction.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag))))
            return true;
        return false;
    }

    /// <summary>
    /// Are the objects of the action similar, that is, from the same semantic category
    /// </summary>
    /// <param name="xmlAction">action loaded from the xml file</param>
    /// <param name="action">current action performed by the user</param>
    /// <returns></returns>
    private bool AreSimilarObjects(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        if (xmlAction.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag) && xmlAction.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag))
            return true;
        return false;
    }

    /// <summary>
    /// check whether the object of the action is from the correct semantic category
    /// </summary>
    /// <param name="action">action performed by the user</param>
    /// <returns></returns>
    private bool CheckSemanticError(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        bool semanticError = false;
        // if the objects have semantic category
        if (HasSemanticCategorySubtask(xmlAction.ElementOne))
        {
            if (!IsCorrectSemanticCategory(action.ElementOne))
            {
                semanticError = true;
            }
        }
        if (HasSemanticCategorySubtask(xmlAction.ElementTwo))
        {
            if (!IsCorrectSemanticCategory(action.ElementTwo))
            {
                semanticError = true;
            }
        }      
        return semanticError;
    }

    /// <summary>
    /// check whether the object of the action is the correct object 
    /// </summary>
    /// <param name="action">the action performed by the user</param>
    /// <returns></returns>
    private bool CheckEpisodicError(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        bool episodicError = false;
        // if the objects have semantic category      
        if (HasSemanticCategorySubtask(xmlAction.ElementOne))
        {
            if (!IsCorrectItem(action.ElementOne.ObjectElement))
            {
                episodicError = true;
            }
        }
        if (HasSemanticCategorySubtask(xmlAction.ElementTwo))
        {
            if (!IsCorrectItem(action.ElementTwo.ObjectElement))
            {
                episodicError = true;
            }
        }     
        return episodicError;
    }

    /// <summary>
    /// Check if the action in the loaded subtask has semantic category
    /// </summary>
    /// <param name="subtask"></param>
    /// <returns></returns>
    private bool HasSemanticCategorySubtask(XMLElement element)
    {
        if (!element.SemanticCategory.Equals(""))
            return true;
        return false;
    }

    /// <summary>
    /// Check whether the element has a semantic category
    /// </summary>
    /// <param name="element">element which contains the object</param>
    /// <returns></returns>
    private bool HasSemanticCategory(Element element)
    {
        if (!element.SemanticCategory.Equals(""))
            return true;
        return false;
    }

    /// <summary>
    /// check whether the element is from the correct semantic category
    /// </summary>
    /// <param name="element">element which contains the object</param>
    /// <returns></returns>
    private bool IsCorrectSemanticCategory(Element element)
    {
        if (element.CorrectSemanticCategory)
            return true;
        return false;
    }

    /// <summary>
    /// Check whether the object is the correct object to be used
    /// </summary>
    /// <param name="gObject">game object</param>
    /// <returns></returns>
    private bool IsCorrectItem(GameObject gObject)
    {
        if (gObject.GetComponent<CorrectItem>())
            return true;
        return false;
    }


    /// <summary>
    /// Check and label all errand order error
    /// </summary>
    /// <param name="scoreFile"></param>
    private void CheckErrandOrderError(System.IO.StreamWriter scoreFile)
    {
        errandOrderErrors = new List<Execution>();
        // the first errand id should be the id of the first errand
        int errandIndex = 0;
        int maxIndex = 1;
        string errandID = xmlErrands[errandIndex++].ID;

        for (int i = 0; i < intrusionRepetitionErrors.Count; i++)
        {
            Execution exe = new Execution();
            exe.CopyExecution(intrusionRepetitionErrors[i]);

            /*// if the execution list errandID is the same as the current errandID
            if (intrusionRepetitionErrors[i].ErrandID.Equals(errandID))
            {
                errandOrderErrors.Add(exe);
            }
            // if the task is an intrusion or repetition, add the subtask as it is
            else if (intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.Intrusion) || intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.Repetition))
            {
                errandOrderErrors.Add(exe);
            }
            // if the execution list errandID is different from the current errandID and it is not the next errand index
            else */
            if (!intrusionRepetitionErrors[i].ErrandID.Equals(errandID) && !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.Intrusion) && !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.Repetition))
            {
                // if maxIndex is less or equal to errand count
                if (maxIndex < xmlErrands.Count)
                {
                    // the errandID is the same as the next ID in the maximum errand sequence
                    if (intrusionRepetitionErrors[i].ErrandID.Equals(xmlErrands[maxIndex].ID))
                    {
                        errandID = xmlErrands[maxIndex++].ID;
                    }
                    // errand ID is not for the next errand in the maximum sequence
                    else
                    {
                        errandID = intrusionRepetitionErrors[i].ErrandID;
                        // get the index for the current errand
                        for (int j = 0; j < xmlErrands.Count; j++)
                        {
                            if (errandID.Equals(xmlErrands[j].ID))
                            {
                                errandIndex = j + 1;
                                break;
                            }
                        }
                        // if the errandIndex is higher, update the maxIndex to the new index
                        if (maxIndex < errandIndex)
                        {
                            maxIndex = errandIndex;
                        }
                        // Add errand order error
                        exe.Errors.Add(Execution.ErrorTypes.ErrandOrder);                       
                    }
                }
                else
                {
                    errandID = intrusionRepetitionErrors[i].ErrandID;
                    // Add errand order error
                    exe.Errors.Add(Execution.ErrorTypes.ErrandOrder);
                }                
            }
            errandOrderErrors.Add(exe);
        }

        scoreFile.WriteLine();
        scoreFile.WriteLine("Errand order errors");
        for (int i = 0; i < errandOrderErrors.Count; i++)
        {
            Execution execution = errandOrderErrors[i];
            string error = "";
            for (int j = 0; j < execution.Errors.Count; j++)
            {
                error += " " + execution.Errors[j];
            }
            scoreFile.WriteLine("ErrandID " + execution.ErrandID + " SubtaskNumber " + execution.SubtaskNumber
                + " TaskType " + execution.TaskType.ToString() + " Error " + error);
        }
    }

    /// <summary>
    /// Checking the order errors
    /// </summary>
    /// <param name="scoreFile"></param>
    private void CheckOrderError(System.IO.StreamWriter scoreFile)
    {
        orderErrors = new List<Execution>();
        int errandIndex = 0;
        string errandID = "";
        int[] maxErrandTaskIndex = new int[xmlErrands.Count];
        int[] errandTaskIndex = new int[xmlErrands.Count];

        // Initialise the each errand task and maximum index
        for (int i = 0; i < xmlErrands.Count; i++)
        { 
            errandTaskIndex[i] = 0;
            maxErrandTaskIndex[i] = 1;
        }

        // Copy the errandOrderErrors list to orderErrors list
        for (int j = 0; j < errandOrderErrors.Count; j++)
        {
            Execution exe = new Execution();
            exe.CopyExecution(errandOrderErrors[j]);
            orderErrors.Add(exe);
        }

        // Checking for order errors
        for (int i = 0; i < orderErrors.Count; i++)
        {
            errandID = orderErrors[i].ErrandID;
            if (!errandID.Equals("I"))
            {
                for (int j = 0; j < xmlErrands.Count; j++)
                {
                    if (errandID.Equals(xmlErrands[j].ID))
                    {
                        errandIndex = j;
                        break;
                    }
                }

                if (maxErrandTaskIndex[errandIndex] <= (xmlErrands[errandIndex].Subtasks.Count+1))
                {
                    // the subtask is the next in the max sequence, no error, add the task as it is
                    if (orderErrors[i].SubtaskNumber == maxErrandTaskIndex[errandIndex])
                    {                        
                        maxErrandTaskIndex[errandIndex] += 1;
                    }
                    else if (orderErrors[i].SubtaskNumber == errandTaskIndex[errandIndex])
                    {                        
                        errandTaskIndex[errandIndex] += 1;
                        if (maxErrandTaskIndex[errandIndex] < errandTaskIndex[errandIndex])
                        {
                            maxErrandTaskIndex[errandIndex] = errandTaskIndex[errandIndex];
                        }
                    }
                    else
                    {
                        errandTaskIndex[errandIndex] = orderErrors[i].SubtaskNumber + 1;
                        if (maxErrandTaskIndex[errandIndex] < errandTaskIndex[errandIndex])
                        {
                            maxErrandTaskIndex[errandIndex] = errandTaskIndex[errandIndex];
                        }
                        orderErrors[i].Errors.Add(Execution.ErrorTypes.Order);
                    }
                }
            }
        }

        /*for (int i = 0; i < xmlErrands.Count; i++)
        {
            errandID = xmlErrands[i].ID;
            maxIndex = 1;
            for (int j = 0; j < orderErrors.Count; j++)
            {
                //Execution exe = new Execution();
                //exe.CopyExecution(errandOrderErrors[j]);

                if (orderErrors[j].ErrandID.Equals(errandID) && !orderErrors[j].TaskType.Equals(Execution.TaskTypes.AuxTask))
                {
                    if (maxIndex < xmlErrands[i].Subtasks.Count)
                    {
                        // the subtask is the next in the max sequence, no error, add the task as it is
                        if (orderErrors[j].SubtaskNumber == maxIndex)
                        {
                            //orderErrors.Add(exe);
                            maxIndex += 1;
                        }
                        else if (orderErrors[j].SubtaskNumber == taskIndex)
                        {
                            //orderErrors.Add(exe);
                            taskIndex += 1;
                            if (maxIndex < taskIndex)
                            {
                                maxIndex = taskIndex;
                            }
                        }
                        else
                        {
                            taskIndex = orderErrors[j].SubtaskNumber + 1;
                            if (maxIndex < taskIndex)
                            {
                                maxIndex = taskIndex;
                            }
                            // Add order error
                            //Execution oSubtask = new Execution(errandID, errandOrderErrors[j].SubtaskNumber,
                            //                Execution.TaskTypes.Order, errandOrderErrors[j].SemanticError, errandOrderErrors[j].EpisodicError);
                            //orderErrors.Add(oSubtask);
                            orderErrors[j].TaskType = Execution.TaskTypes.Order;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }*/

        scoreFile.WriteLine();
        scoreFile.WriteLine("Order Errors");
        for (int i = 0; i < orderErrors.Count; i++)
        {
            Execution execution = orderErrors[i];
            string error = "";
            for (int j = 0; j < execution.Errors.Count; j++)
            {
                error += " " + execution.Errors[j];
            }
            scoreFile.WriteLine("ErrandID " + execution.ErrandID + " SubtaskNumber " + execution.SubtaskNumber
                + " TaskType " + execution.TaskType.ToString() + " Error " + error);
        }
    }

    /// <summary>
    /// Check missing subtasks 
    /// </summary>
    /// <param name="scoreFile"></param>
    private void CheckMissError(System.IO.StreamWriter scoreFile)
    {
        List<XMLSubtask> missErrors = new List<XMLSubtask>();
        for(int i = 0; i < xmlErrands.Count; i++)
        {
            XMLErrand errand = xmlErrands[i];
            for (int j = 0; j < xmlErrands[i].Subtasks.Count; j++)
            {
                XMLSubtask subtask = errand.Subtasks[j];
                if (!executionXMLTasks.Contains(subtask))
                {
                    missErrors.Add(subtask);
                    scoreFile.WriteLine("ErrandID E" + (i + 1) + " Subtask number " + (j + 1));
                }
            }
        }

        /*scoreFile.WriteLine();
        scoreFile.WriteLine("Miss Errors");
        for (int i = 0; i < missErrors.Count; i++)
        {
            scoreFile.WriteLine("ErrandID " + missErrors[i].ID + " Subtask number ");
        }*/

        /*for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];
            scoreFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
            for (int k = 0; k < errand.Subtasks.Count; k++)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                scoreFile.WriteLine("sSubtask ID " + subtask.ID + " Subtask number " + k);
                scoreFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementOne.SemanticCategory
                    + " " + subtask.Action.ElementTwo.ObjectElement + " " + subtask.Action.ElementTwo.SemanticCategory);
            }
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                scoreFile.WriteLine("auxSubtask " + auxSubtask.ID);
                scoreFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
            }
        }*/

    }
}
