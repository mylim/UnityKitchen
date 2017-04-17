using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Subtask{

    /// <summary>
    /// The ID of the subtask.
    /// </summary>
    public string ID
    {
        get;
        set;
    }

    /// <summary>
    /// The primitive action.
    /// </summary>
    public PrimitiveAction Action
    {
        get;
        set;
    }

    public Subtask() 
    {
        this.ID = "";
        this.Action = new PrimitiveAction();
    }

    public Subtask(string ID, PrimitiveAction action) 
    {
        this.ID = ID;
        this.Action = action;
    }
}
