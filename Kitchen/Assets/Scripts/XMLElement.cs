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
    public string SemanticCategory
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
        this.SemanticCategory = "";
    }

    public XMLElement(string tag)
    {
        this.ObjectElement = tag;
        this.SemanticCategory = "";
    }

    public XMLElement(string tag, string semanticCategory)
    {
        this.ObjectElement = tag;
        this.SemanticCategory = semanticCategory;
    }
}
