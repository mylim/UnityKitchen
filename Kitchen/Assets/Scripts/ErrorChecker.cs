﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ErrorChecker {

    private static string INTRUSION_ID = "IN";
    // List of primitive actions and interferences as they occured
    private List<PrimitiveAction> actions;
    private List<Interference> interferences;

    // Ordered list of execution 
    private List<XMLErrand> xmlErrands;
    private List<XMLInterference> xmlInterferences;
    private List<XMLSubtask> executionXMLTasks; 

    // Scoring variables
    private List<Execution> intrusionRepetitionErrors;
    private List<Execution> swappingMixedErrors;
    private List<Execution> errandOrderErrors;
    private List<Execution> orderErrors;
    private List<Execution> missErrors;

    private bool taskFound;
    private string errandID;
    private int taskID;
    private bool rSemanticError;
    private bool rEpisodicError;
    private int[] errorCount = new int[System.Enum.GetNames(typeof(Execution.ErrorTypes)).Length];    

    /// <summary>
    /// Contructor for Error Checker
    /// </summary>
    /// <param name="xmlErrands"></param>
    /// <param name="xmlInterferences"></param>
    public ErrorChecker(List<PrimitiveAction> actions, List<XMLErrand> xmlErrands, List<Interference> interferences, List<XMLInterference> xmlInterferences)
    {
        this.actions = actions;
        this.xmlErrands = xmlErrands;
        this.interferences = interferences;
        this.xmlInterferences = xmlInterferences;

        for (int i = 0; i < interferences.Count; i++)
        {
            Debug.Log(interferences[i].Dialog.name);
        }
    }

    /// <summary>
    /// Check for intrusion and repetition
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckIntrusionRepetition(System.IO.StreamWriter scoreFile)
    {
        intrusionRepetitionErrors = new List<Execution>();
        executionXMLTasks = new List<XMLSubtask>();
        bool taskAdded = false;

        for (int i = 0; i < actions.Count; i++)
        {
            taskFound = false;
            errandID = "";         
            taskID = 0;
            PrimitiveAction action = actions[i];
            scoreFile.WriteLine(action.TimeStamp.ToLongTimeString() + " Action " + action.Name + " " + actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementOne.SemanticCategory + " " +
                    actions[i].ElementTwo.ObjectElement.tag + " " + actions[i].ElementTwo.SemanticCategory);

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
                        taskAdded = CheckIntrusionTask(action, scoreFile);
                        if (!taskAdded)
                        {
                            //taskAdded = CheckIntrusionItem(action, scoreFile);
                            //if (!taskAdded)
                            {
                                List<Execution.ErrorTypes> errors = new List<Execution.ErrorTypes>();
                                errors.Add(Execution.ErrorTypes.IntrusionFull);
                                AddUnknownIntrusion(scoreFile, errors);
                            }
                        }
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
                + " TaskType " + execution.TaskType.ToString() + " Errors: " + error);
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
                        SetRepetition(errand.ID, k + 1, semanticError, episodicError);
                        //AddRepetition(scoreFile, errand.ID, (k + 1), Execution.TaskTypes.Subtask, semanticError, episodicError);                                              
                    }                    
                }
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (subtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(subtask.Action, action))
                {
                    if (!executionXMLTasks.Contains(subtask))
                    {
                        scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (k + 1) + " " + Execution.TaskTypes.Subtask);
                        scoreFile.WriteLine("Subtask objects has same semantic categories but are disimilar");
                        AddKnownIntrusion(scoreFile, errand.ID, (k + 1), Execution.TaskTypes.Subtask, semanticError, episodicError);
                        return true;
                    }
                    else
                    {
                        SetRepetition(errand.ID, k + 1, semanticError, episodicError);
                    }
                }
            }
        }
        return false;
    }

    private void SetRepetition(string eID, int tID, bool semanticError, bool episodicError)
    {
        taskFound = true;
        errandID = eID;
        taskID = tID;
        rSemanticError = semanticError;
        rEpisodicError = episodicError;
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
                }
                // objects are from different categories under the same semantic family, e.g. Mug vs CoffeeMug, hence there is semantic error
                else if (auxSubtask.Action.Name.Equals(action.Name) && AreSameSemanticCategories(auxSubtask.Action, action))
                {
                    scoreFile.WriteLine("Errand ID, k and type " + errand.ID + " " + (l + 1) + " " + Execution.TaskTypes.AuxTask);
                    scoreFile.WriteLine("AuxSubtask objects has same semantic categories but are disimilar");
                    AddKnownIntrusion(scoreFile, errand.ID, (l + 1), Execution.TaskTypes.AuxTask, semanticError, episodicError);
                    return true;
                }              
            }
        }
        return false;
    }

    /// <summary>
    /// Check for correct subtask performed with a random item 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="scoreFile"></param>
    /// <returns></returns>
    /*private bool CheckIntrusionItem(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {
        List<Execution.ErrorTypes> errors = new List<Execution.ErrorTypes>();       

        for (int j = 0; j < xmlErrands.Count; j++)
        {
            XMLErrand errand = xmlErrands[j];
            for (int k = 0; k < errand.Subtasks.Count; k++)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                {
                    // error in which people perform the correct sub task using a random item (i.e. item which is not part an episodic or semantic category)                
                    if (subtask.Action.Name.Equals(action.Name) && 
                        (((!HasSemanticCategory(action.ElementOne)) && subtask.Action.ElementOne.ObjectElement.Equals(action.ElementOne.ObjectElement.tag) && (!HasSemanticCategory(action.ElementTwo))) ||  
                        ((!HasSemanticCategory(action.ElementOne)) && (!HasSemanticCategory(action.ElementTwo)) && subtask.Action.ElementTwo.ObjectElement.Equals(action.ElementTwo.ObjectElement.tag))))
                    {
                        errors.Add(Execution.ErrorTypes.IntrusionItem);
                        AddUnknownIntrusion(scoreFile, errors);
                        return true;
                    }                            
                }
            }
        }
        return false;
    }*/

    /// <summary>
    /// Check for random subtask performed with the correct item they were instructed to use
    /// </summary>
    /// <param name="action"></param>
    /// <param name="scoreFile"></param>
    /// <returns></returns>
    private bool CheckIntrusionTask(PrimitiveAction action, System.IO.StreamWriter scoreFile)
    {
        List<Execution.ErrorTypes> errors = new List<Execution.ErrorTypes>();

        // error in which people perform a sub task which has not been instructed to them, using an item they were instructed to use
        if ((HasSemanticCategory(action.ElementOne) && IsCorrectSemanticCategory(action.ElementOne)) || (HasSemanticCategory(action.ElementTwo) && IsCorrectSemanticCategory(action.ElementTwo)))
        {
            errors.Add(Execution.ErrorTypes.IntrusionTask);
            AddUnknownIntrusion(scoreFile, errors);
            return true;
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
    private void AddUnknownIntrusion(System.IO.StreamWriter scoreFile, List<Execution.ErrorTypes> errors)
    {
        Execution task = new Execution(INTRUSION_ID, 0, Execution.TaskTypes.None);
        for (int i = 0; i < errors.Count; i++)
        {
            task.Errors.Add(errors[i]);
            scoreFile.WriteLine("Adding " + errors[i]);
        }        
        intrusionRepetitionErrors.Add(task);
    }

    /// <summary>
    /// Adding repetition of task
    /// </summary>
    /// <param name="taskID">the repeated task</param>
    /// <param name="scoreFile">the file to be written to</param>
    //private Execution AddRepetition(System.IO.StreamWriter scoreFile, string errandID, int subtaskNumber, Execution.TaskTypes taskType, bool semanticError, bool episodicError)
    private Execution AddRepetition(System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine("Adding repetition");
        Execution task = new Execution(errandID, taskID, Execution.TaskTypes.Subtask);
        task.Errors.Add(Execution.ErrorTypes.Repetition);
        if (rSemanticError)
        {          
            task.Errors.Add(Execution.ErrorTypes.Semantic);
        }
        else if (rEpisodicError)
        {           
            task.Errors.Add(Execution.ErrorTypes.Episodic);
        }
        intrusionRepetitionErrors.Add(task);
        return task;
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
    /// Are the objects of the action similar, that is, from the same semantic category, Eg. black mug, green mug, red mug, blue mug
    /// </summary>
    /// <param name="xmlAction">action loaded from the xml file</param>
    /// <param name="action">current action performed by the user</param>
    /// <returns></returns>
    private bool AreSimilarObjects(XMLPrimitiveAction xmlAction, PrimitiveAction action)
    {
        // return true if object are the same semantic category
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
    /// Check and label all swapping and mixed errors
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckSwappingMixedError(System.IO.StreamWriter scoreFile)
    {
        List<string> errandIDs = new List<string>();
        List<Execution> executions = new List<Execution>();
        swappingMixedErrors = new List<Execution>();
        // index for the next errand    
        int taskIndex = -1;
        // the first errand id should be the id of the first task
        string errandID = intrusionRepetitionErrors[0].ErrandID;

        for (int i = 0; i < intrusionRepetitionErrors.Count; i++)
        {
            Execution exe = new Execution();
            exe.CopyExecution(intrusionRepetitionErrors[i]);

            // if the execution list errandID is different from the current errandID and it is not the next errand index
            if (!intrusionRepetitionErrors[i].ErrandID.Equals(errandID)
                && !intrusionRepetitionErrors[i].TaskType.Equals(Execution.TaskTypes.AuxTask)
                && !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionFull)
                //&& !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionItem)
                && !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionTask))
            {
                if((taskIndex > 0) && (exe.SubtaskNumber == taskIndex))
                {
                    if (exe.Errors.Contains(Execution.ErrorTypes.Repetition))
                    {
                        // Add mixed error - swapping and repetition
                        exe.Errors.Add(Execution.ErrorTypes.Mixed);
                    }
                    else
                    {
                        // Add swapping error
                        exe.Errors.Add(Execution.ErrorTypes.Swapping);                        
                    }                    
                }
                taskIndex = -1;
                errandID = intrusionRepetitionErrors[i].ErrandID;
            }
            else if (intrusionRepetitionErrors[i].ErrandID.Equals(errandID))
            {
                taskIndex = exe.SubtaskNumber + 1;
            }
            swappingMixedErrors.Add(exe);
        }

        scoreFile.WriteLine();
        scoreFile.WriteLine("Swapping and mixed errors");
        for (int i = 0; i < swappingMixedErrors.Count; i++)
        {
            Execution execution = swappingMixedErrors[i];
            string error = "";
            for (int j = 0; j < execution.Errors.Count; j++)
            {
                error += " " + execution.Errors[j];
            }
            scoreFile.WriteLine("ErrandID " + execution.ErrandID + " SubtaskNumber " + execution.SubtaskNumber
                + " TaskType " + execution.TaskType.ToString() + " Errors: " + error);
        }
    }

    /// <summary>
    /// Check and label all errand level errors
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckErrandError(System.IO.StreamWriter scoreFile)
    {
        List<string> errandIDs = new List<string>();
        List<Execution> executions = new List<Execution>();
        errandOrderErrors = new List<Execution>();
        // index for the next errand    
        int errandIndex = 0;
        //int maxIndex = 1;
        // the first errand id should be the id of the first errand
        string errandID = xmlErrands[errandIndex++].ID;

        for (int i = 0; i < swappingMixedErrors.Count; i++)
        {
            Execution exe = new Execution();
            exe.CopyExecution(swappingMixedErrors[i]);

            // if the execution list errandID is different from the current errandID and it is not the next errand index
            if (!swappingMixedErrors[i].ErrandID.Equals(errandID)
                && !swappingMixedErrors[i].TaskType.Equals(Execution.TaskTypes.AuxTask)
                && !swappingMixedErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionFull)
                //&& !intrusionRepetitionErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionItem)
                && !swappingMixedErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionTask))
            {
                // if maxIndex is less or equal to errand count and the errandID is the same as the next ID in the maximum errand sequence
                if ((i != 0) && (errandIndex < xmlErrands.Count) && (swappingMixedErrors[i].ErrandID.Equals(xmlErrands[errandIndex].ID)))
                {
                    errandID = xmlErrands[errandIndex++].ID;
                }
                // errand ID is not for the next errand in the maximum sequence
                else
                {
                    errandID = swappingMixedErrors[i].ErrandID;
                    // get the index for the current errand
                    for (int j = 0; j < xmlErrands.Count; j++)
                    {
                        if (errandID.Equals(xmlErrands[j].ID))
                        {
                            errandIndex = j + 1;
                            break;
                        }
                    }
                    // Add errand order error
                    exe.Errors.Add(Execution.ErrorTypes.ErrandOrder);
                }

                // if the errand has never been performed 
                if (!errandIDs.Contains(exe.ErrandID))
                {
                    // add the id to the errand id checking list
                    errandIDs.Add(exe.ErrandID);
                }
                else
                {
                    // Checking whether the errand subtask(s) has already been done                 
                    int k = i;
                    string id = exe.ErrandID;
                    bool repeat = false;
                    while (k < swappingMixedErrors.Count && swappingMixedErrors[k].ErrandID.Equals(id) && !repeat)
                    {
                        if (swappingMixedErrors[k++].Errors.Contains(Execution.ErrorTypes.Repetition))
                        {
                            // Add repeat errand error
                            exe.Errors.Add(Execution.ErrorTypes.RepeatErrand);
                            repeat = true;
                        }
                    }

                    // if there is no repetition within the errand block
                    if (!repeat)
                    {
                        exe.Errors.Add(Execution.ErrorTypes.SplitErrand);
                    }
                }
            }
            errandOrderErrors.Add(exe);
        }

        scoreFile.WriteLine();
        scoreFile.WriteLine("Errand level errors");
        for (int i = 0; i < errandOrderErrors.Count; i++)
        {
            Execution execution = errandOrderErrors[i];
            string error = "";
            for (int j = 0; j < execution.Errors.Count; j++)
            {
                error += " " + execution.Errors[j];
            }
            scoreFile.WriteLine("ErrandID " + execution.ErrandID + " SubtaskNumber " + execution.SubtaskNumber
                + " TaskType " + execution.TaskType.ToString() + " Errors: " + error);
        }
    }

    /// <summary>
    /// Checking the order errors
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckOrderError(System.IO.StreamWriter scoreFile)
    {
        orderErrors = new List<Execution>();
        int errandIndex = 0;
        string errandID = "";
        int[] maxErrandTaskIndex = new int[xmlErrands.Count];
        int[] errandTaskIndex = new int[xmlErrands.Count];

        // Initialise each errand task and maximum indexes
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
            // ignore intrusion, repetition and auxTask
            if (!orderErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionFull)
                //&&!orderErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionItem)
                && !orderErrors[i].Errors.Contains(Execution.ErrorTypes.IntrusionTask)
                && !orderErrors[i].Errors.Contains(Execution.ErrorTypes.Repetition) 
                && !orderErrors[i].TaskType.Equals(Execution.TaskTypes.AuxTask))
            {
                for (int j = 0; j < xmlErrands.Count; j++)
                {
                    if (errandID.Equals(xmlErrands[j].ID))
                    {
                        errandIndex = j;
                        break;
                    }
                }

                if (maxErrandTaskIndex[errandIndex] <= (xmlErrands[errandIndex].Subtasks.Count + 1))
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
                + " TaskType " + execution.TaskType.ToString() + " Errors: " + error);
        }
    }

    /// <summary>
    /// Checking missing subtasks 
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckMissError(System.IO.StreamWriter scoreFile)
    {
        scoreFile.WriteLine();
        scoreFile.WriteLine("Miss Errors");
        for (int i = 0; i < xmlErrands.Count; i++)
        {
            XMLErrand errand = xmlErrands[i];
            for (int j = 0; j < xmlErrands[i].Subtasks.Count; j++)
            {
                XMLSubtask subtask = errand.Subtasks[j];
                if (!executionXMLTasks.Contains(subtask))
                {
                    errorCount[(int)Execution.ErrorTypes.Misses]++;
                    scoreFile.WriteLine("ErrandID E" + (i + 1) + " Subtask number " + (j + 1));
                }
            }
        }
    }

    /// <summary>
    /// Checking interference error
    /// </summary>
    /// <param name="scoreFile"></param>
    public void CheckExecutiveError(System.IO.StreamWriter scoreFile)
    {
        List<string> executiveError = new List<string>();
        int index = 0;

        for (int i = 0; i < xmlInterferences.Count; i++)
        {
            bool error = true;
            bool dialogFound = false;
            for (int k = 0; k < interferences.Count; k++)
            {
                if (interferences[k].Dialog.name.Equals(xmlInterferences[i].Dialog))
                {
                    index = k;
                    dialogFound = true;
                    //scoreFile.WriteLine("Dialog found");
                    break;
                }
            }

            if (dialogFound)
            {
                for (int j = 0; j < xmlInterferences[i].IObjects.Count && (error); j++)
                {
                    string xmlObj = xmlInterferences[i].IObjects[j];
                    for (int l = 0; (l < interferences[index].iObjects.Count) && (error); l++)
                    {
                        string obj = interferences[index].iObjects[l].iObject.tag;
                        if (obj.Equals(xmlObj))
                        {
                            error = false;
                        }
                    }
                }

                if (error)
                {
                    executiveError.Add(interferences[index].Dialog.name);
                }
            }
        }

        scoreFile.WriteLine();
        scoreFile.WriteLine("Executive errors");
        for (int i = 0; i < executiveError.Count; i++)
        {
            scoreFile.WriteLine(executiveError[i]);
        }
        errorCount[(int)Execution.ErrorTypes.Executive] = executiveError.Count;
    }

    public void CountErrors(System.IO.StreamWriter scoreFile)
    {
        for (int i = 0; i < orderErrors.Count; i++)
        {
            for (int j = 0; j < orderErrors[i].Errors.Count; j++)
            {
                int errorIndex = (int)orderErrors[i].Errors[j];
                errorCount[errorIndex]++;
            }
        }

        scoreFile.WriteLine();
        string[] errorNames = System.Enum.GetNames(typeof(Execution.ErrorTypes));
        for (int i = 0; i < errorNames.Length; i++)
        {
            scoreFile.WriteLine("Total " + errorNames[i] + ": " + errorCount[i]);
        }
    }
}
