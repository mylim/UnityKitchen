using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A primitive action can be represented as: "pickedUp(player, kettle)", "in(teabag, teacup)" (i.e. player picked up kettle, teabag is in the teacup), 
/// </summary>
public class PrimitiveAction : MonoBehaviour {
  
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
    public GameObject ElementOne
    {
        get;
        set;
    }

    /// <summary>
    /// The second element of the primitive action.
    /// </summary>
    public GameObject ElementTwo
    {
        get;
        set;
    }


    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty state.
    /// </summary>
    /// <param name="name">The name of the state</param>
    public PrimitiveAction(string name)
    {
        this.Name = name;
    }

    /// <summary>
    /// Constructor for a new primitive action.
    /// </summary>
    /// <param name="name">The name of the state</param>
    public PrimitiveAction(string name, GameObject elementOne, GameObject elementTwo)
    {
        Add(name, elementOne, elementTwo);
    }

    /// <summary>
    /// Adds a primitive action.
    /// </summary>
    /// <param name="variable">The action name</param>
    /// <param name="elementOne">The first element of the primitive action</param>
    /// <param name="elementTwo">The second element of the primitive action</param>
    public void Add(string name, GameObject elementOne, GameObject elementTwo)
    {
        this.Name = name;
        this.ElementOne = elementOne;
        this.ElementTwo = elementTwo;
    }
}
