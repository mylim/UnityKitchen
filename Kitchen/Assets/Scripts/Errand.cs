using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Errand {
    //private string name;
    //private List<Subtask> subtasks;

    /// <summary>
    /// The ID of the errand.
    /// </summary>
    public string ID
    {
        get;
        set;
    }

    /// <summary>
    /// The name of the errand.
    /// </summary>
    public string Name
    {
        get;
        set;
    }

    /// <summary>
    /// The subtasks of the errand.
    /// </summary>
    public List<Subtask> Subtasks
    {
        get;
        set;
    }

    /// <summary>
    /// The auxiliary subtasks of the errand.
    /// </summary>
    public List<Subtask> AuxSubtasks
    {
        get;
        set;
    }


    public Errand()
    {
        this.ID = "";
        this.Name = "";
        this.Subtasks = new List<Subtask>();
        this.AuxSubtasks = new List<Subtask>();
    }

    public Errand(string ID, string name)
    {
        this.ID = ID;
        this.Name = name;
        this.Subtasks = new List<Subtask>();
        this.AuxSubtasks = new List<Subtask>();
    }

    public void AddSubtask(Subtask subtask)
    {
        this.Subtasks.Add(subtask);
    }

    public void AddAuxSubtask(Subtask subtask)
    {
        this.AuxSubtasks.Add(subtask);
    }

}
