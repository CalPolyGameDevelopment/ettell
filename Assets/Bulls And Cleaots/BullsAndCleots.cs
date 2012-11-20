using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;


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
    
    private Dictionary<string, List<Material>> dataSets;
    private static Color NULL_COLOR = MaterialData.NULL_COLOR;

    private XmlNode data;
    public XmlNode Data {
        set {
            data = value;

            loadLevel();
        }
    }
    
    public GameObject level;



    private Material getItem(XmlNode itemNode){
        Color color = MaterialData.GetColor(itemNode);
        Texture texture = MaterialData.GetTexture(itemNode);
        Material material = new Material(Shader.Find("Diffuse"));

        if (color != NULL_COLOR){
             material.color = color;
        }
        if (texture != null){
            material.mainTexture = texture;
        }
        if (texture == null && color == NULL_COLOR){
            throw new System.MissingFieldException(
                string.Format("Unable to find a color or texture in the item in dataset {}!",
                itemNode.ParentNode.Attributes["id"]));
        }

        return material;
    }
    
    
    private bool addDataSet(XmlNode setNode){
        List<Material> matsList = new List<Material>();
        foreach (XmlNode child in setNode.childNodes(ITEM)){
            matsList.Add (getItem(child));
        }


        string setName = setNode.Attributes["id"].Value;

        dataSets[setName] = matsList;
        return true;
    }

    private void loadDataSets(){
        dataSets = new Dictionary<string, List<Material>>();
        foreach(XmlNode child in data.childNodes(DATASET)){
             addDataSet(child);
        }

    }
    

    private void loadLevel() {
        GameObject go = Instantiate(level) as GameObject;
        go.transform.parent = transform;
        BullsAndCleotsLevelController bcLevel = go.GetComponent<BullsAndCleotsLevelController>();
    
        int solutionLen = MathData.GetInt(UserProperty.GetPropNode(SOLUTION_LEN_PROP));
             
        loadDataSets();
        SolutionManager slnManager = new SolutionManager();
        List<List<Material>> choices = new List<List<Material>>();

        foreach(List<Material> mats in dataSets.Values){
            IEnumerable<Material> matChoices =
                mats.OrderBy(x => Random.value).Take(solutionLen);

            IEnumerable<Material> solution =
                matChoices.OrderBy(x => Random.value).Take(solutionLen);
            choices.Add(matChoices.ToList ());
            Solution sln = new Solution(new ArrayList(solution.ToArray()));
            slnManager.AddSolution(sln);
        }
        bcLevel.InitData = new BCLevelData(slnManager, choices);
 
    }
    
}
