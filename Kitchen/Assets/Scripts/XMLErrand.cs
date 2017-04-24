using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XMLErrand {
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
    public List<XMLSubtask> Subtasks
    {
        get;
        set;
    }

    public XMLErrand()
    {
        this.Name = "";
        Subtasks = new List<XMLSubtask>();
    }

    public XMLErrand(string name)
    {
        this.Name = name;
        this.Subtasks = new List<XMLSubtask>();
    }

    public void AddSubtask(XMLSubtask subtask)
    {
        this.Subtasks.Add(subtask);
    }
}
