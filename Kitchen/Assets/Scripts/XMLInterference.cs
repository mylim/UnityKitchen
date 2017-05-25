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
    public List<string> IObjects
    {
        get;
        set;
    }

    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty interference
    /// </summary>
    public XMLInterference()
    {
        this.Dialog = "";
        this.IObjects = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    public XMLInterference(string dialog)
    {
        this.Dialog = dialog;
        this.IObjects = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference
    /// </summary>
    /// <param name="dialog">The interference dialog</param>
    ///  /// <param name="iObjects">The objects list of the interference</param>
    public XMLInterference(string dialog, List<string> iObjects)
    {
        this.Dialog = dialog;
        this.IObjects = iObjects;
    }

    /// <summary>
    /// Adding new object to the object list
    /// </summary>
    /// <param name="iObject">The new object</param>
    public void AddObject(string iObject)
    {
        this.IObjects.Add(iObject);
    }

}
