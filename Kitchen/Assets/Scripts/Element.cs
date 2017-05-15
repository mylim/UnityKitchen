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
    /// The semantic category of the element
    /// </summary>
    public string SemanticCategory
    {
        get;
        set;
    }

    /// <summary>
    /// Whether the element belongs to the right semantic category
    /// </summary>
    public bool CorrectSemanticCategory
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
        this.SemanticCategory = "";
        this.CorrectSemanticCategory = false;
    }

    public Element(GameObject objectElement)
    {
        this.ObjectElement = objectElement;
        this.SemanticCategory = "";
        this.CorrectSemanticCategory = false;
    }

    public Element(GameObject element, string semanticCategory, bool correctSemanticCategory)
    {
        this.ObjectElement = element;
        this.SemanticCategory = semanticCategory;
        this.CorrectSemanticCategory = correctSemanticCategory;
    }
}
