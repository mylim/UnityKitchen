using UnityEngine;
using System.Collections;

public class Element{

    /// <summary>
    /// The object element 
    /// </summary>
    public GameObject ObjectElement
    {
        get;
        set;
    }

    /// <summary>
    /// The first element of the primitive action.
    /// </summary>
    public bool SemanticCategory
    {
        get;
        set;
    }

    // CONSTRUCTORS

    /// <summary>
    /// Constructor for a new empty element
    /// </summary>
    public Element()
    {
        this.ObjectElement = new GameObject();
        this.SemanticCategory = false;
    }

    public Element(GameObject objectElement)
    {
        this.ObjectElement = objectElement;
        this.SemanticCategory = false;
    }

    public Element(string tag, bool semanticCategory)
    {
        this.ObjectElement = new GameObject();
        this.ObjectElement.tag = tag;
        if (semanticCategory)
        {
            this.SemanticCategory = true;
        }
        else
        {            
            this.SemanticCategory = false;
        }
    }
}
