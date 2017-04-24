using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XMLSubtask{

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
    public XMLPrimitiveAction Action
    {
        get;
        set;
    }

    public XMLSubtask() 
    {
        this.ID = "";
        this.Action = new XMLPrimitiveAction();
    }

    public XMLSubtask(string ID, XMLPrimitiveAction action) 
    {
        this.ID = ID;
        this.Action = action;
    }
}
