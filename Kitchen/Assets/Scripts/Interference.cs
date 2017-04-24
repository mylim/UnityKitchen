using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Task interference  
/// </summary>
public class Interference {
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
    public List<GameObject> iObjects
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
        this.Dialog = new InterferenceDialog();
        this.iObjects = new List<GameObject>();
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    public Interference(InterferenceDialog dialog)
    {
        this.Dialog = dialog;
        this.iObjects = new List<GameObject>();
    }


    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    ///  /// <param name="iObjects">The objects list of the interference</param>
    public Interference(InterferenceDialog dialog, List<GameObject> iObjects)
    {
        Add(dialog, iObjects);
    }

    /// <summary>
    /// Adds an interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    /// <param name="iObjects">The objects list of the interference</param>
    public void Add(InterferenceDialog dialog, List<GameObject> iObjects)
    {
        this.Dialog = dialog;
        this.iObjects = iObjects;
    }

    /// <summary>
    /// Adding new object to the object list
    /// </summary>
    /// <param name="iObject">The new object</param>
    public void AddObject(GameObject iObject)
    {
        this.iObjects.Add(iObject);
    }

}
