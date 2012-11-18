using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class BullsAndCleots : MonoBehaviour, MiniGameAPI.IMiniGame {
 
    const string BUTTER = "butter";
    
    // Props... mad props
    private static class Props {
        public const string SolutionLength = "BCSolutionLength";
        public const string ColorCount = "BCColorCount";
        public const string NumberCount = "BCNumberCount";
    }
    
    public static class ExitEdges {
        public const string IncreaseDifficulty = "BCIncreaseDifficulty";
        public const string IncreaseColors = "BCIncreaseColors";
        public const string IncreaseNumbers = "BCIncreaseNumbers";
    }
    
    private XmlNode data;
    public XmlNode Data {
        set {
            data = value;
            LoadLevel();
        }
    }
    
    public GameObject level;
     

    void LoadLevel() {
        GameObject go = Instantiate(level) as GameObject;
		go.transform.parent = transform;
        BullsAndCleotsLevelController bcLevel = go.GetComponent<BullsAndCleotsLevelController>();
    
        int solutionLen = int.Parse(UserProperty.getProp(Props.SolutionLength));
        int numberCount = int.Parse(UserProperty.getProp(Props.NumberCount));
        int colorCount  = int.Parse(UserProperty.getProp(Props.ColorCount));
        
        
        if (numberCount < solutionLen)
            numberCount = solutionLen;
        if (colorCount < solutionLen)
            colorCount = solutionLen;
 
        
        
        IEnumerable<int> numberChoices = 
            Enumerable.Range(0,9).OrderBy(x => Random.value).Take(numberCount);
       
        IEnumerable<Color> colorChoices =
           ColorUtilities.GetColors().OrderBy(x => Random.value).Take(colorCount);
        
        BCLevelData ld = new BCLevelData(solutionLen,numberChoices,colorChoices);
        ld.fromXml = MaterialData.GetColor(data.SelectSingleNode("butter"));

        
        bcLevel.InitData = ld;

        
        
 
    }
    
}
