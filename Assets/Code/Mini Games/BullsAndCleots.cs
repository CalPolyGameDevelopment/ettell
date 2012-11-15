using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class BullsAndCleots : MonoBehaviour, MiniGameAPI.IMiniGame {
 
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
 
    HashSet<int> numbers = new HashSet<int>{
        1,2,3,
        4,5,6,
        7,8,9,
        0
    };
    
    List<Color> colors = new List<Color>{
        Color.black,
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow,
        Color.white,
        Color.magenta,
        Color.cyan
    };
 
    

    void LoadLevel() {
        GameObject go = Instantiate(level) as GameObject;
		go.transform.parent = transform;
        BullsAndCleotsLevelController bcLevel = go.GetComponent<BullsAndCleotsLevelController>();
    
        int solutionLen = int.Parse(UserProperty.getProp(Props.SolutionLength));
        int numberCount = int.Parse(UserProperty.getProp (Props.NumberCount));
        int colorCount  = int.Parse(UserProperty.getProp(Props.ColorCount));
        
        
        if (numberCount < solutionLen)
            numberCount = solutionLen;
        if (colorCount < solutionLen)
            colorCount = solutionLen;
 
        
        
        IEnumerable<int> numberChoices = 
            numbers.OrderBy(x => Random.value).Take(numberCount);
       
        IEnumerable<Color> colorChoices = 
            colors.OrderBy(x=>Random.value).Take(colorCount);
        
        bcLevel.InitData = new BCLevelData(solutionLen,numberChoices,colorChoices);
                
       
    }

}
