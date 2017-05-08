using UnityEngine;
using System.Collections;

public class ErrandErrors {

    /// <summary>
    /// The executed errands list
    /// </summary>
    public string ErrandID
    {
        get;
        set;
    }

    /// <summary>
    /// Semantic Error
    /// </summary>
    public int SemanticError
    {
        get;
        set;
    }

    /// <summary>
    /// Episodic Error
    /// </summary>
    public int EpisodicError
    {
        get;
        set;
    }

    /// <summary>
    /// Order Error
    /// </summary>
    public int OrderError
    {
        get;
        set;
    }

    /// <summary>
    /// Intrusion Error
    /// </summary>
    public int IntrusionError
    {
        get;
        set;
    }

    /// <summary>
    /// Repetition Error
    /// </summary>
    public int RepetitionError
    {
        get;
        set;
    }

    /// <summary>
    /// Executive Error
    /// </summary>
    public int ExecutiveError
    {
        get;
        set;
    }

    public ErrandErrors()
    {
        this.ErrandID = "";
        this.SemanticError = 0;
        this.EpisodicError = 0;
        this.OrderError = 0;
        this.IntrusionError = 0;
        this.RepetitionError = 0;
        this.ExecutiveError = 0;
    }
}
