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
    public PrimitiveAction Action
    {
        get;
        set;
    }

    public Subtask() 
    {
        this.ID = "";
        //this.Auxiliary = false;
        this.Action = new PrimitiveAction();
    }

    public Subtask(string ID, bool auxiliary, PrimitiveAction action) 
    {
        this.ID = ID;
        //this.Auxiliary = auxiliary;
        this.Action = action;
    }
}
