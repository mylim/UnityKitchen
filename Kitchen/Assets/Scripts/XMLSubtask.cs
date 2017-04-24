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

    /*/// <summary>
    /// Whether the action is auxiliary
    /// </summary>
    public bool Auxiliary
    {
        get;
        set;
    }*/

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
        //this.Auxiliary = false;
        this.Action = new XMLPrimitiveAction();
    }

    public XMLSubtask(string ID, bool auxiliary, XMLPrimitiveAction action) 
    {
        this.ID = ID;
        //this.Auxiliary = auxiliary;
        this.Action = action;
    }
}
