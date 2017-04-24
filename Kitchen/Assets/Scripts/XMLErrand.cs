using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XMLErrand {
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
    public List<XMLSubtask> Subtasks
    {
        get;
        set;
    }

    /// <summary>
    /// The subtasks of the errand.
    /// </summary>
    public List<XMLSubtask> AuxSubtasks
    {
        get;
        set;
    }

    public XMLErrand()
    {
        this.ID = "";
        this.Name = "";
        Subtasks = new List<XMLSubtask>();
        AuxSubtasks = new List<XMLSubtask>();
    }

    public XMLErrand(string ID, string name)
    {
        this.ID = ID;
        this.Name = name;
        this.Subtasks = new List<XMLSubtask>();
        AuxSubtasks = new List<XMLSubtask>();
    }

    public void AddSubtask(XMLSubtask subtask)
    {
        this.Subtasks.Add(subtask);
    }

    public void AddAuxSubtask(XMLSubtask subtask)
    {
        this.AuxSubtasks.Add(subtask);
    }

}
