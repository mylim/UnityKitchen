﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class XMLParser{
    private XmlDocument errandsFile;
    private XmlDocument interferencesFile;

    // Use this for initialization
    public XMLParser() {
        // loading the errands file
        errandsFile = new XmlDocument();
        errandsFile.Load(@".\Assets\XML\Tasks.xml");

        // loading the interferences file
        interferencesFile = new XmlDocument();
        interferencesFile.Load(@".\Assets\XML\Interferences.xml");
    }

    public List<XMLErrand> ParseXMLErrands() { 
        // Subtasks
        XmlNodeList nodes = errandsFile.DocumentElement.SelectNodes("/Errands/Errand");
        List<XMLErrand> errands = new List<XMLErrand>();       
        foreach (XmlNode node in nodes)
        {
            XMLErrand errand = new XMLErrand();
            errand.Name = node.Attributes["name"].Value;

            // Actions in each subtask
            XmlNodeList nodes2 = errandsFile.DocumentElement.SelectNodes("/Errands/Errand/Subtask");
            foreach (XmlNode node2 in nodes2)
            {               
                XMLSubtask subtask = new XMLSubtask();
                subtask.ID = node2.Attributes["ID"].Value;
                XMLPrimitiveAction pAction = new XMLPrimitiveAction();
                pAction.Name = node2.SelectSingleNode("Name").InnerText;
                if (node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"] != null)
                {
                    //bool e1SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"].Value);
                    pAction.ElementOne = new XMLElement(node2.SelectSingleNode("ElementOne").InnerText, true);
                }
                else
                {
                    pAction.ElementOne = new XMLElement(node2.SelectSingleNode("ElementOne").InnerText, false);
                }
                if (node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"] != null)
                {
                    //bool e2SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"].Value);
                    pAction.ElementTwo = new XMLElement(node2.SelectSingleNode("ElementTwo").InnerText, true);
                }
                else
                {
                    pAction.ElementTwo = new XMLElement(node2.SelectSingleNode("ElementTwo").InnerText, false);
                }
                subtask.Action = pAction;
              
                errand.AddSubtask(subtask);
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
            interference.iObjects = iObjects;
            interferences.Add(interference);
        }

        return interferences;
    }
}
