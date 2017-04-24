using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Task interference  
/// </summary>
public class XMLInterference {
    /// <summary>
    /// The interference dialog.
    /// </summary>
    public string Dialog
    {
        get;
        set;
    }

    /// <summary>
    /// The objects involved in the interference
    /// </summary>
    public List<string> iObjects
    {
        get;
        set;
    }

    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty state.
    /// </summary>
    public XMLInterference()
    {
        this.Dialog = "";
        this.iObjects = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    public XMLInterference(string dialog)
    {
        this.Dialog = dialog;
        this.iObjects = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    ///  /// <param name="iObjects">The objects list of the interference</param>
    public XMLInterference(string dialog, List<string> iObjects)
    {
        Add(dialog, iObjects);
    }

    /// <summary>
    /// Adds an interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    /// <param name="iObjects">The objects list of the interference</param>
    public void Add(string dialog, List<string> iObjects)
    {
        this.Dialog = dialog;
        this.iObjects = iObjects;
    }

    /// <summary>
    /// Adding new object to the object list
    /// </summary>
    /// <param name="iObject">The new object</param>
    public void AddObject(string iObject)
    {
        this.iObjects.Add(iObject);
    }

}
