using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

public class XMLParser{
    private XmlDocument errandsFile;
    private XmlDocument interferencesFile;
    private XmlDocument interferenceVersionsFile;

    private string filePath;
    private string result = "";
    private WWW www;

    IEnumerator loadFile(string file)
    {
        filePath = Path.Combine(Application.streamingAssetsPath, file);
        Debug.Log("filepath " + filePath);
        result = "";
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
            result = File.ReadAllText(filePath);
    }

    // Use this for initialization
    public XMLParser() {

        // Windows
        // loading the errands file
        errandsFile = new XmlDocument();
        errandsFile.Load(@".\Assets\XML\Tasks.xml"); 

        // loading the interferences file
        interferencesFile = new XmlDocument();
        interferencesFile.Load(@".\Assets\XML\Interferences.xml");           

        // loading the interference versions file
        interferenceVersionsFile = new XmlDocument();
        interferenceVersionsFile.Load(@".\Assets\XML\InterferenceVersions.xml");   

        /*//WebGL
        // loading the errands file
        Debug.Log("result " + result);
        loadFile("Tasks.xml");
        errandsFile = new XmlDocument();
        errandsFile.LoadXml(result);

        // loading the interferences file
        Debug.Log("result " + result);
        loadFile("Interferences.xml");
        interferencesFile = new XmlDocument();
        interferencesFile.LoadXml(result);

        // loading the interference versions file
        Debug.Log("result " + result);
        loadFile("InterferenceVersions.xml");
        interferenceVersionsFile = new XmlDocument();
        interferenceVersionsFile.LoadXml(result);*/

        // WebPlayer
        /*// loading the errands file
        errandsFile = new XmlDocument();
        errandsFile.Load(Resources.Load<TextAsset>("Tasks.xml").text);

        // loading the interferences file
        interferencesFile = new XmlDocument();
        interferencesFile.Load(Resources.Load<TextAsset>("Interferences.xml").text);

        // loading the interference versions file
        interferenceVersionsFile = new XmlDocument();
        interferenceVersionsFile.Load(Resources.Load<TextAsset>("InterferenceVersions").text);*/
    }

    public List<XMLErrand> ParseXMLErrands() { 
        // Subtasks
        XmlNodeList nodes = errandsFile.DocumentElement.SelectNodes("/Errands/Errand");
        List<XMLErrand> errands = new List<XMLErrand>();       
        foreach (XmlNode node in nodes)
        {
            XMLErrand errand = new XMLErrand();
            errand.ID = node.Attributes["ID"].Value;
            errand.Name = node.Attributes["name"].Value;
            //Debug.Log("Errand ID " + errand.ID);

            // Actions in each subtask
            //XmlNodeList nodes2 = errandsFile.DocumentElement.SelectNodes("/Errands/Errand/Subtask");
            foreach (XmlNode node2 in node.ChildNodes)
            {               
                XMLSubtask subtask = new XMLSubtask();
                subtask.ID = node2.Attributes["ID"].Value;
                //Debug.Log("Subtask ID " + subtask.ID);

                XMLPrimitiveAction pAction = new XMLPrimitiveAction();
                pAction.Name = node2.SelectSingleNode("Name").InnerText;
                //string semanticCategoryOne = "";
                //string semanticCategoryTwo = "";
                if (node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"] != null)
                {
                    //semanticCategoryOne = node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"].InnerText;
                    //bool e1SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"].Value);
                    pAction.ElementOne = new XMLElement(node2.SelectSingleNode("ElementOne").InnerText, node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"].InnerText);
                }
                else
                {
                    pAction.ElementOne = new XMLElement(node2.SelectSingleNode("ElementOne").InnerText, "");
                }
                if (node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"] != null)
                {
                    //semanticCategoryTwo = node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"].InnerText;
                    //bool e2SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"].Value);
                    pAction.ElementTwo = new XMLElement(node2.SelectSingleNode("ElementTwo").InnerText, node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"].InnerText);
                }
                else
                {
                    pAction.ElementTwo = new XMLElement(node2.SelectSingleNode("ElementTwo").InnerText, "");
                }
                subtask.Action = pAction;
                //Debug.Log("Subtask action " + subtask.Action.Name);

                if (node2.Attributes["Auxiliary"] != null)
                {
                    errand.AddAuxSubtask(subtask);
                    //Debug.Log("Auxiliary true");
                }
                else
                {
                    errand.AddSubtask(subtask);
                    //Debug.Log("subtask count " + errand.Subtasks.Count); 
                }
            }
            errands.Add(errand);
        }
    
        return errands;
    }

    public List<XMLInterference> ParseXMLInterferences()
    {
        // Interference
        XmlNodeList nodes = interferencesFile.DocumentElement.SelectNodes("/Interferences/Interference");
        List<XMLInterference> interferences = new List<XMLInterference>();
        foreach (XmlNode node in nodes)
        {
            XMLInterference interference = new XMLInterference();
            interference.Dialog = node.Attributes["dialog"].Value;

            // Associated objects with the interference
            List<string> iObjects = new List<string>();
            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    iObjects.Add(node.ChildNodes[i].InnerText);
                    //Debug.Log("Object " + node.ChildNodes[i].InnerText);
                }
            }
            interference.IObjects = iObjects;
            interferences.Add(interference);
        }

        return interferences;
    }

    public List<XMLInterferenceVersion> ParseXMLInterferenceVersions()
    {
        //Debug.Log("parsing xml interference version");
        // Interference
        XmlNodeList nodes = interferenceVersionsFile.DocumentElement.SelectNodes("/IVersions/IVersion");
        List<XMLInterferenceVersion> interferenceVersions = new List<XMLInterferenceVersion>();
        foreach (XmlNode node in nodes)
        {
            XMLInterferenceVersion interferenceVersion = new XMLInterferenceVersion();
            interferenceVersion.Number = int.Parse(node.Attributes["number"].Value);
            //Debug.Log("Number " + interferenceVersion.Number);

            // Associated objects with the interference
            List<string> dialogs = new List<string>();
            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    dialogs.Add(node.ChildNodes[i].InnerText);
                    //Debug.Log("Dialog " + node.ChildNodes[i].InnerText);
                    //Debug.Log("Object " + node.ChildNodes[i].InnerText);
                }
            }
            interferenceVersion.Dialogs = dialogs;
            interferenceVersions.Add(interferenceVersion);
        }

        return interferenceVersions;
    }
}
