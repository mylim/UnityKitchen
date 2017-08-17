using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A primitive action can be represented as: "pickedUp(player, kettle)", "in(teabag, teacup)" (i.e. player picked up kettle, teabag is in the teacup), 
/// </summary>
public class PrimitiveAction {
  
    /// <summary>
    /// The action timeStamp
    /// </summary>
    public DateTime TimeStamp
    {
        get;
        set;
    }

    /// <summary>
    /// The name of the primitive action.
    /// </summary>
    public string Name
    {
        get;
        set;
    }

    /// <summary>
    /// The first element of the primitive action.
    /// </summary>
    public Element ElementOne
    {
        get;
        set;
    }

    /// <summary>
    /// The second element of the primitive action.
    /// </summary>
    public Element ElementTwo
    {
        get;
        set;
    }


    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty state.
    /// </summary>
    public PrimitiveAction()
    {
        this.TimeStamp = System.DateTime.Now;
        this.Name = "";
        this.ElementOne = new Element();
        this.ElementTwo = new Element();
    }

    /// <summary>
    /// Constructor for a new primitive action.
    /// </summary>
    /// <param name="name">The name of the state</param>
    public PrimitiveAction(DateTime timeStamp, string name, Element elementOne, Element elementTwo)
    {
        Add(timeStamp, name, elementOne, elementTwo);
    }

    /// <summary>
    /// Adds a primitive action.
    /// </summary>
    /// <param name="timeStamp">The time when the action is performed</param>
    /// <param name="name">The action name</param>
    /// <param name="elementOne">The first element of the primitive action</param>
    /// <param name="elementTwo">The second element of the primitive action</param>
    public void Add(DateTime timeStamp, string name, Element elementOne, Element elementTwo)
    {
        this.TimeStamp = timeStamp;
        this.Name = name;
        this.ElementOne = elementOne;
        this.ElementTwo = elementTwo;
    }
}
