using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldModelManager : MonoBehaviour {

    //public enum VRAISVersion { Practice, Pilot1, Pilot2, Pilot3 };
    //public enum InterferenceVersion { V1, V2, V3, V4, V5 }

    // Interference dialog and variable
    public int interferenceInterval;
    //public int interferenceVersion;
    // when main menu is used, get the version from user input
    //private int interferenceVersion;
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
    private string[] VRAISVersion = { "Practice", "Pilot1", "Pilot2", "Pilot3" };
    private string[] InterferenceVersion = { "None", "V1", "V2", "V3", "V4", "V5" };
    private string fileName;

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
        if (DataManager.Instance.InterferenceVersion > 0)
        {
            dialogList = xmlInterferenceVersions[DataManager.Instance.InterferenceVersion].Dialogs;
        }       
        //dialogList = xmlInterferenceVersions[interferenceVersion].Dialogs;

        fileName = System.DateTime.Today.ToString("yy-MM-dd") + "_" + VRAISVersion[DataManager.Instance.VRAISVersion]
           + "_V" + DataManager.Instance.InterferenceVersion + "_P" + DataManager.Instance.ParticipantID + "_A" + DataManager.Instance.AssessmentNo;
    }

    /// <summary>
    /// update the interference status every cycle 
    /// </summary>
    void Update()
    {
        if ((dialogList != null) && (dialogIndex < dialogs.Length) && ((currentDialog != null) && (currentDialog.DialogClosed())))
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
        if ((dialogList != null) && (actions.Count > 0) && (actions.Count % interferenceInterval == 0) && (!interfering))
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
            }
        }
    }

    /// <summary>
    /// Interference occurred
    /// </summary>
    /// <param name="iObject">object to be added to the object list</param>
    public void InterfereWorldModel(GameObject iObject)
    {
        if ((dialogList != null) && (dialogIndex < dialogs.Length))
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
        if (dialogList != null)
        {
            interfering = currentDialog.GetInterference();
        }
        else
        {
            interfering = false;
        }
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
    public void LogActions()
    {
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_log.txt", true))
        //using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@".\Logs\" + fileName + "_log.txt", true))
        {
            /*for (int j = 0; j < xmlErrands.Count; j++)
            {
                XMLErrand errand = xmlErrands[j];
                logFile.WriteLine("errand " + errand.Name + " ID " + errand.ID);
                for (int k = 0; k < errand.Subtasks.Count; k++)
                {
                    XMLSubtask subtask = errand.Subtasks[k];
                    logFile.WriteLine("Subtask ID " + subtask.ID + " Subtask number " + k);
                    logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementTwo.ObjectElement);
                    /*logFile.WriteLine("Action " + subtask.Action.Name + " " + subtask.Action.ElementOne.ObjectElement + " " + subtask.Action.ElementOne.SemanticCategory
                        + " " + subtask.Action.ElementTwo.ObjectElement + " " + subtask.Action.ElementTwo.SemanticCategory);
                }
                for (int l = 0; l < errand.AuxSubtasks.Count; l++)
                {
                    XMLSubtask auxSubtask = errand.AuxSubtasks[l];
                    logFile.WriteLine("auxSubtask " + auxSubtask.ID);
                    logFile.WriteLine("Aux Action " + auxSubtask.Action.Name + " " + auxSubtask.Action.ElementOne.ObjectElement + " " + auxSubtask.Action.ElementTwo.ObjectElement);
                }
            }

            logFile.WriteLine();*/
            for (int i = 0; i < actions.Count; i++)
            {
                logFile.WriteLine("Action " + i + " " + actions[i].Name + " " +
                    actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementTwo.ObjectElement.tag);
                /*logFile.WriteLine("Action " + i + " " + actions[i].Name + " " +
                    actions[i].ElementOne.ObjectElement.tag + " " + actions[i].ElementOne.SemanticCategory + " " +
                    actions[i].ElementTwo.ObjectElement.tag + " " + actions[i].ElementTwo.SemanticCategory);*/
            }
        }
    }

    /// <summary>
    /// Logging all the interferences to a text file with post-fix _interference.txt in the Logs folder 
    /// </summary>
    public void LogInterferences()
    {
        using (System.IO.StreamWriter interferenceFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_interference.txt", true))
        //using (System.IO.StreamWriter interferenceFile = new System.IO.StreamWriter(@".\Logs\" + fileName + "_interference.txt", true))
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

        using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@"..\Logs\" + fileName + "_score.txt", true))
        //using (System.IO.StreamWriter scoreFile = new System.IO.StreamWriter(@".\Logs\" + fileName + "_score.txt", true))
        {
            // Checking for intrusion and repetition
            errorChecker.CheckIntrusionRepetition(scoreFile);

            // Checking for swapping and mixed error
            errorChecker.CheckSwappingMixedError(scoreFile);

            // Checking for errand order error
            errorChecker.CheckErrandError(scoreFile);

            // Checking the order error
            errorChecker.CheckOrderError(scoreFile);

            // Checking the missing subtasks
            errorChecker.CheckMissError(scoreFile);

            // Checking the interference error
            errorChecker.CheckExecutiveError(scoreFile);

            // Count all the errors
            errorChecker.CountErrors(scoreFile);
        }
    }
}
