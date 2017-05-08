using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ErrandExecution {

    /// <summary>
    /// The execution list for each errand
    /// </summary>
    public List<Execution> ExecutionList
    {
        get;
        set;
    }

    public ErrandExecution()
    {
        this.ExecutionList = new List<Execution>();
    }
}
