using UnityEngine;
using System.Collections;

public class Execution {

    public enum TaskTypes {None, Subtask, AuxTask, Intrusion, Repetition};
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
    /// The ID of the auxSubtask.
    /// </summary>
    public Execution.TaskTypes TaskType
    {
        get;
        set;
    }

    /// <summary>
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
    }

    public Execution()
    {
        this.ErrandID = "";
        this.SubtaskNumber = 0;
        this.TaskType = Execution.TaskTypes.None;
    }

    public Execution(string errandID, int subtaskNumber, Execution.TaskTypes taskType, bool SemanticError, bool EpisodicError)
    {
        this.ErrandID = errandID;
        this.SubtaskNumber = subtaskNumber;
        this.TaskType = taskType;
        this.SemanticError = SemanticError;
        this.EpisodicError = EpisodicError;
    }

    public void CopyExecution(Execution execution)
    {
        this.ErrandID = execution.ErrandID;
        this.SubtaskNumber = execution.SubtaskNumber;
        this.TaskType = execution.TaskType;
        this.SemanticError = execution.SemanticError;
        this.EpisodicError = execution.EpisodicError;
    }
}
