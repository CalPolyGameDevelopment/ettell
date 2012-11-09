using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class BullsAndCleots : MonoBehaviour, MiniGameAPI.IMiniGame {
 
    private XmlNode data;
    public XmlNode Data {
     set {
         data = value;
         LoadLevel();
     }
    }
    
    public GameObject level;
    

    
    void SetSolutionNumbers(BullsAndCleotsLevelController level, 
            XmlNode solutionNode){
        List<int> solutionNumbers = new List<int>();
        
        foreach (XmlNode numberNode in solutionNode.SelectNodes(XmlUtilities.number)){
            int number = int.Parse(XmlUtilities.getData(numberNode));
            solutionNumbers.Add(number);
        }
        level.solutionDigits = solutionNumbers.ToArray();
    }
    
    void SetSolutionColors(BullsAndCleotsLevelController level,
            XmlNode solutionNode){
        List<Color> solutionColors = new List<Color>();
        
        foreach (XmlNode colorNode in solutionNode.SelectNodes(XmlUtilities.color)){
            string colorName = XmlUtilities.getData(colorNode);
            Color newColor = XmlUtilities.colorNameToValueMap[colorName];
            solutionColors.Add(newColor);
        }
        level.solutionColors = solutionColors.ToArray();
       
    }
    
    void LoadLevel(){
        GameObject go = Instantiate(level) as GameObject;
        BullsAndCleotsLevelController bcLevel = go.GetComponent<BullsAndCleotsLevelController>();

        foreach (XmlNode solutionNode in data.SelectNodes(XmlUtilities.solution)){
            string solutionTypeName = XmlUtilities.getData(solutionNode);            
            if (solutionTypeName == "colors"){
                SetSolutionColors(bcLevel, solutionNode);
            }
            else if (solutionTypeName == "numbers"){
                SetSolutionNumbers(bcLevel, solutionNode);
            }
            else{
                Debug.LogError("unknown solution type: " + solutionTypeName);
            }
        }
    }
    
}
