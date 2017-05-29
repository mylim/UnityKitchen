using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldModelManager : MonoBehaviour {
    // Interference dialog and variable
    public int interferenceInterval;
    private int interferenceVersion;
    public InterferenceDialog[] dialogs;
    private InterferenceDialog currentDialog;
    private int dialogIndex;
    private bool interfering;

    // List of primitive actions and interferences as they occured
    private List<PrimitiveAction> actions;
    private List<Interference> interferences;
 
    // List of errands and interference loaded from xml files
    private XMLParser xmlParser;
    private List<XMLErrand> xmlErrands;
    private List<XMLInterference> xmlInterferences;
    private List<XMLInterferenceVersion> xmlInterferenceVersions;
    private List<string> dialogList;

    private ErrorChecker errorChecker;

    // Use this for initialization
    void Start() {
        dialogIndex = 0;
        actions = new List<PrimitiveAction>();
        interferences = new List<Interference>();
        xmlParser = new XMLParser();
        xmlErrands = xmlParser.ParseXMLErrands();
        xmlInterferences = xmlParser.ParseXMLInterferences();

        // parse the interference versions from xml and get the right interference dialogs list
        xmlInterferenceVersions = xmlParser.ParseXMLInterferenceVersions();
        dialogList = xmlInterferenceVersions[DataManager.Instance.InterferenceVersion].Dialogs;
        /*for (int j = 0; j < xmlErrands.Count; j++)
        //foreach (XMLErrand errand in xmlErrands)
        {
            XMLErrand errand = xmlErrands[j];
            Debug.Log("errand " + errand.Name + " ID " + errand.ID);
            for (int k = 0; k < errand.Subtasks.Count; k++)
            //foreach (XMLSubtask subtask in errand.Subtasks)
            {
                XMLSubtask subtask = errand.Subtasks[k];
                Debug.Log("Subtask number " + k);
                Debug.Log("subtask " + subtask.ID);
                Debug.Log("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement);
            }
            for (int l = 0; l < errand.AuxSubtasks.Count; l++)
            //foreach (XMLSubtask auxSubtask in errand.AuxSubtasks)
            {
                XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                Debug.Log("auxSubtask " + auxSubtask.ID);
                Debug.Log("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
            }
        }*/
        /*foreach (XMLInterference interference in xmlInterferences)
        {
            Debug.Log("Interference " + interference.Dialog);
            List<string> iObjects = interference.iObjects;
            foreach (string iObject in iObjects)
            {
                Debug.Log("Object " + iObject);
            }
        }*/

        /*foreach (XMLInterferenceVersion iVersion in xmlInterferenceVersions)
        {
            Debug.Log("InterferenceVersion " + iVersion.Number);
            List<string> dialogs = iVersion.Dialogs;
            foreach (string dialog in dialogs)
            {
                Debug.Log("Dialog " + dialog);
            }
        }*/
    }

    /// <summary>
    /// update the interference status every cycle 
    /// </summary>
    void Update()
    {
        /*if ((dialogIndex < dialogs.Length) && dialogs[dialogIndex].DialogClosed())
        {
            UpdateInterference();
            interferences[dialogIndex].Answer = dialogs[dialogIndex].GetAnswer();
            dialogIndex++;
        }*/

        if ((dialogIndex < dialogs.Length) && ((currentDialog != null) && (currentDialog.DialogClosed())))
        {
            Debug.Log("dialog index " + dialogIndex);
            UpdateInterference();
            interferences[dialogIndex].Answer = currentDialog.GetAnswer();
            dialogIndex++;
            currentDialog = null;
        }
    }

    /// <summary>
    /// Update the world model with the new action performed by the user
    /// </summary>
    /// <param name="pAction">the action name</param>
    /// <param name="elementOne">interaction object one</param>
    /// <param name="elementTwo">interaction object two</param>
    public void UpdateWorldModel(string pAction, GameObject elementOne, GameObject elementTwo)
    {
        //adding the action performed
        Element eOne = new Element(elementOne);
        Element eTwo = new Element(elementTwo);        
        if (elementOne.transform.parent != null && elementOne.transform.parent.GetComponent<SemanticCategory>())
        {
            // element one has a semantic category
            eOne.SemanticCategory = elementOne.transform.parent.tag;
            // element one belongs to the right semantic category
            if(elementOne.transform.parent.GetComponent<CorrectSemanticCategory>())
            {
                eOne.CorrectSemanticCategory = true;
            }
        }
        if (elementTwo.transform.parent != null && elementTwo.transform.parent.GetComponent<SemanticCategory>())
        {
            // element two has a semantic category
            eTwo.SemanticCategory = elementTwo.transform.parent.tag;
            // element two belongs to the right semantic category
            if (elementTwo.transform.parent.GetComponent<CorrectSemanticCategory>())
            {
                eTwo.CorrectSemanticCategory = true;
            }
        }
        // adding the primitive action performed to the list of actions
        actions.Add(new PrimitiveAction(pAction, eOne, eTwo));

        // activate the interference is the interval is reached and no other interference is active
        if ((actions.Count > 0) && (actions.Count % interferenceInterval == 0) && (!interfering))
        {
            if ((dialogIndex < dialogs.Length))
            {
                Debug.Log("Dialog index to show" + dialogIndex);
                for (int i = 0; i < dialogs.Length; i++)
                {
                    if (dialogs[i].name.Equals(dialogList[dialogIndex]))
                    {
                        currentDialog = dialogs[i];
                        break;
                    }                    
                }

                if (currentDialog != null)
                {
                    currentDialog.ShowDialog();
                    UpdateInterference();
                    //adding the interference action
                    interferences.Add(new Interference(currentDialog));
                }
                /*dialogs[dialogIndex].ShowDialog();
                UpdateInterference();
                // adding the interference action            
                interferences.Add(new Interference(dialogs[dialogIndex]));      */
            }
        }
    }

    /// <summary>
    /// Interference occurred
    /// </summary>
    /// <param name="iObject">object to be added to the object list</param>
    public void InterfereWorldModel(GameObject iObject)
    {
        if ((dialogIndex < dialogs.Length))
        {
            interferences[dialogIndex].AddObject(iObject);
        }
    }

    /// <summary>
    /// Update the state of interfering variable
    /// </summary>
    private void UpdateInterference()
    {
        //interfering = dialogs[dialogIndex].GetInterference();
        interfering = currentDialog.GetInterference();
    }

    /// <summary>
    /// Get the interfering value
    /// </summary>
    /// <returns></returns>
    public bool GetInterference()
    {
        return interfering;
    }

    /// <summary>
    /// Logging all the actions to a text file with post-fix _log.txt in the Logs folder 
    /// </summary>
    private void LogActions()
    {
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        //using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_log.txt", true))
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_log.txt", true))
        {
            for (int j = 0; j < xmlErrands.Count; j++)
            {
                XMLErrand errand = xmlErrands[j];
                logFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
                for (int k = 0; k < errand.Subtasks.Count; k++)
                {
                    XMLSubtask subtask = errand.Subtasks[k];
                    logFile.WriteLine("sSubtask ID " + subtask.ID + " Subtask number " + k);
                    logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementOne.SemanticCategory
                        + " " + subtask.Action.ElementTwo.ObjectElement + " " + subtask.Action.ElementTwo.SemanticCategory);
                }
                for (int l = 0; l < errand.AuxSubtasks.Count; l++)
                {
                    XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                    logFile.WriteLine("auxSubtask " + auxSubtask.ID);
                    logFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
                }
            }

            logFile.WriteLine();
            for (int i = 0; i < actions.Count; i++)
            {
                logFile.WriteLine("Action " + i + " " + actions[i].Name + " " + actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementTwo.ObjectElement.tag);
            }
        }
    }

    /// <summary>
    /// Logging all the interferences to a text file with post-fix _interference.txt in the Logs folder 
    /// </summary>
    private void LogInterferences()
    {
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        //using (System.IO.StreamWriter interferenceFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_interference.txt", true))
        using (System.IO.StreamWriter interferenceFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_interference.txt", true))
        {
            /*foreach (XMLInterferenceVersion iVersion in xmlInterferenceVersions)
            {
                interferenceFile.WriteLine("InterferenceVersion " + iVersion.Number);
                List<string> dialogs = iVersion.Dialogs;
                foreach (string dialog in dialogs)
                {
                    interferenceFile.WriteLine("Dialog " + dialog);
                }
            }*/
            for (int j = 0; j < interferences.Count; j++)
            {
                interferenceFile.WriteLine("Interference : " + interferences[j].Dialog.name);
                for (int k = 0; k < interferences[j].iObjects.Count; k++)
                {
                    interferenceFile.WriteLine("Click : " + interferences[j].iObjects[k].tag);
                }
                interferenceFile.WriteLine("Answer : " + interferences[j].Answer);
            }
        }
    }

    /// <summary>
    /// Score the action of the user at the same time write to a text file with post-fix _score.txt in the Logs folder 
    /// </summary>
    public void Score()
    {
        LogActions();
        LogInterferences();

        errorChecker = new ErrorChecker(actions, xmlErrands, interferences, xmlInterferences);
        string fileName = System.DateTime.Today.ToString("yy-MM-dd");
        //using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        {
            // Checking for intrusion and repetition
            errorChecker.CheckIntrusionRepetition(scoreFile);

            // Checking for errand order error
            errorChecker.CheckErrandError(scoreFile);

            // Checking the order error
            errorChecker.CheckOrderError(scoreFile);

            // Checking the missing subtasks
            errorChecker.CheckMissError(scoreFile);

            // Checking the interference error
            errorChecker.CheckInterferenceError(scoreFile);

            // Count all the errors
            errorChecker.CountErrors(scoreFile);
        }
    }
}
