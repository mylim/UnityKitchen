using UnityEngine;
using System.Collections;

public class XMLElement{

    /// <summary>
    /// The object element 
    /// </summary>
    public string ObjectElement
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
    public XMLElement()
    {
        this.ObjectElement = "";
        this.SemanticCategory = false;
    }

    public XMLElement(string objectElement)
    {
        this.ObjectElement = objectElement;
        this.SemanticCategory = false;
    }

    public XMLElement(string tag, bool semanticCategory)
    {
        this.ObjectElement = tag;
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
