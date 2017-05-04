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
        bool taskAdded = false;

        LogActions();
        LogInterferences();
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        {
            for (int i = 0; i < actions.Count; i++)
            {
                taskFound = false;
                taskID = 0;
                PrimitiveAction action = actions[i];
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

            string errandID = executionList[0].ErrandID;

            for (int i = 0; i < executionList.Count; i++)
            {
                if (errandID.Equals(executionList[i].ErrandID))
                {

                }
                scoreFile.WriteLine("ErrandID " + executionList[i].ErrandID + " SubtaskNumber " + executionList[i].SubtaskNumber
                    + " TaskType " + executionList[i].TaskType.ToString() + (executionList[i].SemanticError ? " semanticError" : "") + 
                    (executionList[i].EpisodicError ? " episodicError": ""));
            }
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
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);
                        // If no error, add the task
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, k + 1, Execution.TaskTypes.Subtask, semanticError, episodicError);
                            executionXMLTasks.Add(subtask);
                            executionList.Add(task);
                        }
                        // Different objects from the same category is picked
                        else
                        {
                            scoreFile.WriteLine("There is an error in the subtask");
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
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (subtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(subtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);
                    scoreFile.WriteLine("Subtask objects has same semantic categories but are disimilar");
                    AddKnownIntrusion(scoreFile, semanticError, episodicError);
                    return true;
                }
            }
        }        
        return false;
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
                // check whether there is semantic or episodic error
                //bool semanticError = CheckSemanticError(action);
                //bool episodicError = CheckEpisodicError(action);
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
                    //scoreFile.WriteLine("Errand " + j + " " + errand.Name + " auxSubtask " + (l + 1) + " ID " + auxSubtask.ID + " and action are the same");  
                    // add the auxSubtask to the checklist, auxSubtask will be ignored during scoring
                    if (!executionXMLTasks.Contains(auxSubtask))
                    {
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.AuxTask);
                        // If no error, add the auxSubtask
                        if (!semanticError && !episodicError)
                        {
                            Execution task = new Execution(errand.ID, l + 1, Execution.TaskTypes.AuxTask, semanticError, episodicError);
                            executionXMLTasks.Add(auxSubtask);
                            executionList.Add(task);
                        }
                        // Different objects from the same category is picked
                        else
                        {
                            scoreFile.WriteLine("There is an error in the auxSubtask");
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
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (auxSubtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(auxSubtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.AuxTask);
                    scoreFile.WriteLine("AuxSubtask objects has same semantic categories but are disimilar");
                    AddKnownIntrusion(scoreFile, semanticError, episodicError);
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
    private void AddKnownIntrusion(System.IO.StreamWriter scoreFile, bool semanticError, bool episodicError)
    {      
        scoreFile.WriteLine("Adding known intrusion: semanticError " + semanticError + " episodicError " + episodicError);
        Execution task = new Execution("I", 0, Execution.TaskTypes.Intrusion, semanticError, episodicError);
        executionList.Add(task);        
    }

    /// <summary>
    /// Adding unknown intrusion
    /// </summary>
    /// <param name="scoreFile">the file to be written to</param>
    private void AddUnknownIntrusion(System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine("Adding unknown intrusion");
        Execution task = new Execution("I", 0, Execution.TaskTypes.Intrusion, false, false);
        executionList.Add(task);
    }

    /// <summary>
    /// Adding repetition of task
    /// </summary>
    /// <param name="taskID">the repeated task</param>
    /// <param name="scoreFile">the file to be written to</param>
    private void AddRepetition(int taskID, System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine("Adding repetition");
        Execution task = new Execution("R", taskID, Execution.TaskTypes.Repetition, false, false);
        executionList.Add(task);
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
        if (((HasSemanticCategorySubtask(xmlAction.ElementOne)) && ((xmlAction.ElementOne.SemanticCategory.Equals(action.ElementOne.SemanticCategory)) 
            || (xmlAction.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag))))
            && ((HasSemanticCategorySubtask(xmlAction.ElementTwo)) && ((xmlAction.ElementTwo.SemanticCategory.Equals(action.ElementTwo.SemanticCategory))
            || (xmlAction.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag)))))
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
}
