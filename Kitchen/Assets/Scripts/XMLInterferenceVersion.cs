using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XMLInterferenceVersion
{

    /// <summary>
    /// The version number
    /// </summary>
    public int Number
    {
        get;
        set;
    }

    /// <summary>
    /// The interference dialogs list
    /// </summary>
    public List<string> Dialogs
    {
        get;
        set;
    }

    /// <summary>
    /// Constructor for a new empty interference version
    /// </summary>
    public XMLInterferenceVersion()
    {
        this.Number = 0;
        this.Dialogs = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference version
    /// </summary>
    /// <param name="number"></param>
    public XMLInterferenceVersion(int number)
    {
        this.Number = number;
        this.Dialogs = new List<string>();
    }

    /// <summary>
    /// Constructor for a new interference version
    /// </summary>
    /// <param name="number"></param>
    /// <param name="dialogs"></param>
    public XMLInterferenceVersion(int number, List<string> dialogs)
    {
        this.Number = number;
        this.Dialogs = dialogs;
    }

    /// <summary>
    /// Adding a new dialog to the list
    /// </summary>
    /// <param name="dialog"></param>
    public void AddDialog(string dialog)
    {
        this.Dialogs.Add(dialog);
    }
}
