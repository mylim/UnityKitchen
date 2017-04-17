using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class XMLParser{
    private XmlDocument doc;
    // Use this for initialization
    public XMLParser() {
        doc = new XmlDocument();
        doc.Load(@".\Assets\XML\Tasks.xml");
    }

    public List<Errand> ParseXML() { 
        // Subtasks
        XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Errands/Errand");
        List<Errand> errands = new List<Errand>();       
        foreach (XmlNode node in nodes)
        {
            Errand errand = new Errand();
            errand.Name = node.Attributes["name"].Value;

            // Actions in each subtask
            XmlNodeList nodes2 = doc.DocumentElement.SelectNodes("/Errands/Errand/Subtask");
            foreach (XmlNode node2 in nodes2)
            {               
                Subtask subtask = new Subtask();
                subtask.ID = node2.Attributes["ID"].Value;
                PrimitiveAction pAction = new PrimitiveAction();
                pAction.Name = node2.SelectSingleNode("Name").InnerText;
                if (node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"] != null)
                {
                    //bool e1SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementOne").Attributes["semanticCategory"].Value);
                    pAction.ElementOne = new Element(node2.SelectSingleNode("ElementOne").InnerText, true);
                }
                else
                {
                    pAction.ElementOne = new Element(node2.SelectSingleNode("ElementOne").InnerText, false);
                }
                if (node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"] != null)
                {
                    //bool e2SemanticCategory = bool.Parse(node2.SelectSingleNode("ElementTwo").Attributes["semanticCategory"].Value);
                    pAction.ElementTwo = new Element(node2.SelectSingleNode("ElementTwo").InnerText, true);
                }
                else
                {
                    pAction.ElementTwo = new Element(node2.SelectSingleNode("ElementTwo").InnerText, false);
                }
                subtask.Action = pAction;
              
                errand.AddSubtask(subtask);
            }           
            errands.Add(errand);
        }
    
        return errands;
    }
}
