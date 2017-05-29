using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Execution {

    public enum TaskTypes {None, Subtask, AuxTask};
    public enum ErrorTypes {Intrusion, IntrusionItem, IntrusionTask, Semantic, Episodic, Repetition, Order, Miss, ErrandOrder, SplitErrand, RepeatErrand, Interference};
    /// <summary>
    /// The ID of the errand.
    /// </summary>
    public string ErrandID
    {
        get;
        set;
    }

    /// <summary>
    /// The ID of the subtask.
    /// </summary>
    public int SubtaskNumber
    {
        get;
        set;
    }

    /// <summary>
    /// The type of the subtask.
    /// </summary>
    public Execution.TaskTypes TaskType
    {
        get;
        set;
    }

    /// <summary>
    /// The error in the subtask.
    /// </summary>
    public List<Execution.ErrorTypes> Errors
    {
        get;
        set;
    }

    /*/// <summary>
    /// Semantic Error
    /// </summary>
    public bool SemanticError
    {
        get;
        set;
    }

    /// <summary>
    /// Episodic Error
    /// </summary>
    public bool EpisodicError
    {
        get;
        set;
    }*/

    public Execution()
    {
        this.ErrandID = "";
        this.SubtaskNumber = 0;
        this.TaskType = Execution.TaskTypes.None;
    }

    public Execution(string errandID, int subtaskNumber, Execution.TaskTypes taskType)
    {
        this.ErrandID = errandID;
        this.SubtaskNumber = subtaskNumber;
        this.TaskType = taskType;
        this.Errors = new List<ErrorTypes>();
    }

    public void AddError(Execution.ErrorTypes error)
    {
        this.Errors.Add(error);
    }

    public void CopyExecution(Execution execution)
    {
        this.ErrandID = execution.ErrandID;
        this.SubtaskNumber = execution.SubtaskNumber;
        this.TaskType = execution.TaskType;
        this.Errors = execution.Errors;
    }
}
