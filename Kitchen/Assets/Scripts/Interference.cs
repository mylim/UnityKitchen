using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Task interference  
/// </summary>
public class Interference {

    /// <summary>
    /// Time when the interference started
    /// </summary>
    public DateTime StartTime
    {
        get;
        set;
    }

    /// <summary>
    /// Time when the interference ended
    /// </summary>
    public DateTime EndTime
    {
        get;
        set;
    }

    /// <summary>
    /// The interference dialog.
    /// </summary>
    public InterferenceDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    /// The objects involved in the interference
    /// </summary>
    public List<InterferenceObject> iObjects
    {
        get;
        set;
    }

    /// <summary>
    /// The answer to the interference question
    /// </summary>
    public string Answer
    {
        get;
        set;
    }

    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty state.
    /// </summary>
    public Interference()
    {
        this.StartTime = System.DateTime.Now;
        this.Dialog = new InterferenceDialog();
        this.iObjects = new List<InterferenceObject>();
        this.Answer = "";
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    public Interference(InterferenceDialog dialog)
    {
        this.StartTime = System.DateTime.Now;
        this.Dialog = dialog;
        this.iObjects = new List<InterferenceObject>();
        this.Answer = "";
    }


    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    ///  /// <param name="iObjects">The objects list of the interference</param>
    public Interference(InterferenceDialog dialog, List<InterferenceObject> iObjects)
    {
        Add(dialog, iObjects);
    }

    /// <summary>
    /// Adds an interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    /// <param name="iObjects">The objects list of the interference</param>
    public void Add(InterferenceDialog dialog, List<InterferenceObject> iObjects)
    {
        this.Dialog = dialog;
        this.iObjects = iObjects;
    }

    /// <summary>
    /// Adding new object to the object list
    /// </summary>
    /// <param name="iObject">The new object</param>
    public void AddObject(DateTime time, GameObject iObject)
    {
        InterferenceObject intObject = new InterferenceObject(time, iObject);
        this.iObjects.Add(intObject);
    }
}
