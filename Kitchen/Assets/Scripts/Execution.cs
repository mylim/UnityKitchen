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
    }
}
