using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public struct DataSetItem{
    public static Shader DefaultShader = Shader.Find("Diffuse");
    object val; 
    Material material;
    
    public DataSetItem(object v){
        val = v;
        material = new Material(DefaultShader);
    }
    
    public DataSetItem(object v, Color c) : this(v) {
        material.color = c;
    }
    
    public DataSetItem(object v, Texture t) : this(v) {
        material.mainTexture = t;
        
    }
    
    public DataSetItem(object v, Texture t, Color c) : this(v) {
        material.color = c;
        material.mainTexture = t;
    }
}

public class BullsAndCleots : MonoBehaviour, MiniGameAPI.IMiniGame {
 
  
    

    /// <summary>
    /// The tag name of the Solution Length node in the user properties.
    /// </summary>
    public static string SOLUTION_LEN_PROP = "BCSolutionLength";

    public static string DATASET = "dataset";
    public static string ITEM = "item";
       
    // Use endings.cs?
    public static class ExitEdges {
        public const string IncreaseDifficulty = "BCIncreaseDifficulty";
        public const string IncreaseColors = "BCIncreaseColors";
        public const string IncreaseNumbers = "BCIncreaseNumbers";
    }
    
    private Dictionary<string, List<object>> dataSets;
    
    private XmlNode data;
    public XmlNode Data {
        set {
            data = value;
            loadLevel();
        }
    }
    
    public GameObject level;
    
    object getItem(XmlNode itemNode){
        return null;
    }
    
    
    List<object> getDataSet(XmlNode setNode){
        return XmlUtilities.childNodes(setNode, ITEM)
                .Select<XmlNode,object>(x => getItem(x)).ToList();
            
    }
    
    void getDataSets(){
        XmlUtilities.childNodes(data, DATASET)
            .Select<XmlNode,List<object>>(x => getDataSet(x));
    }
    

    void loadLevel() {
        GameObject go = Instantiate(level) as GameObject;
     go.transform.parent = transform;
        BullsAndCleotsLevelController bcLevel = go.GetComponent<BullsAndCleotsLevelController>();
    
        int solutionLen = MathData.GetInt(UserProperty.GetPropNode(SOLUTION_LEN_PROP));
             

        
    
        /*
        
        IEnumerable<int> numberChoices = 
            Enumerable.Range(0,9).OrderBy(x => Random.value).Take(numberCount);
       
        IEnumerable<Color> colorChoices =
           ColorUtilities.GetColors().OrderBy(x => Random.value).Take(colorCount);
        
        BCLevelData ld = new BCLevelData(solutionLen,numberChoices,colorChoices);
        ld.fromXml = MaterialData.GetColor(data.SelectSingleNode("butter"));

        
        bcLevel.InitData = ld;

        */
        
 
    }
    
}
