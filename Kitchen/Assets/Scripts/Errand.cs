using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Errand {
    //private string name;
    //private List<Subtask> subtasks;

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

    public Errand()
    {
        this.Name = "";
        Subtasks = new List<Subtask>();
    }

    public Errand(string name)
    {
        this.Name = name;
        this.Subtasks = new List<Subtask>();
    }

    public void AddSubtask(Subtask subtask)
    {
        this.Subtasks.Add(subtask);
    }
}
