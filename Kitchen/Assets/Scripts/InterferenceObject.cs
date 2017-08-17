using UnityEngine;
using System.Collections;
using System;

public class InterferenceObject {

    /// <summary>
    /// Time when the interference occurred
    /// </summary>
    public DateTime TimeStamp
    {
        get;
        set;
    }

    /// <summary>
    /// The object clicked in the interference
    /// </summary>
    public GameObject iObject
    {
        get;
        set;
    }

    public InterferenceObject(DateTime time, GameObject iObject)
    {
        this.TimeStamp = time;
        this.iObject = iObject;
    }
}
