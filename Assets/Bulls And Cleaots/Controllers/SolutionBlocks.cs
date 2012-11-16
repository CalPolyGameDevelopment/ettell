using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class SolutionComponent {
       
    Texture2D texture;
    Color color;
    object data;
    bool isColor;
    
    public bool IsColor{
        get {
            return isColor;
        }
    }
    
    public SolutionComponent(string textureName, object slnData){
        texture = Resources.Load(textureName) as Texture2D;
        data = slnData;
        isColor = false;
    }
    
    public SolutionComponent(Color c, object slnData) {
        color = c;
        data = slnData;
        isColor = true;
    }
    
    public Texture2D GetTexture(){
        return texture;
    }
    public Color GetColor(){
        return color;
    }
    public object GetData(){
        return data;
    }
    
}

public class SolutionBlocks : MonoBehaviour {
 
    
    public GameObject blockPrefab;
    
    public Transform startTransform;
    
    private List<GameObject> blocks; 
    private float layoutSpacing = 1.2f;
    
    private SolutionComponent[] choices;
    private int count;
    
    public SolutionComponent[] Choices  {
        set{
            count = value.GetLength(0);
            choices = new SolutionComponent[count];
            Array.Copy(value, choices, count);
        }
    }
 
     
    
    // Use this for initialization
    void Start() {

        Vector3 pos = startTransform.position;

        Func<Vector3, Vector3> LayoutObject = oldPos => new Vector3(
            oldPos.x + layoutSpacing, oldPos.y, oldPos.z);

        blocks = new List<GameObject>();
        
        foreach (SolutionComponent sc in choices){
            GameObject newBlock = Instantiate(blockPrefab) as GameObject;
            newBlock.transform.parent = gameObject.transform;
            
            SolutionBlock solutionBlock = newBlock.GetComponent<SolutionBlock>();
            newBlock.transform.parent = gameObject.transform;
            newBlock.transform.position = LayoutObject(pos);
   
            if (sc.IsColor)
                newBlock.renderer.material.color = sc.GetColor();
            else
                newBlock.renderer.material.mainTexture = sc.GetTexture();
            
            solutionBlock.data = sc.GetData();
            blocks.Add(newBlock);
            
            pos = newBlock.transform.position;
        }


        

    }


    public void Reset() {
        IEnumerable<Draggable> draggables = Enumerable.Select<GameObject,Draggable>(
            blocks, b => b.GetComponent<Draggable>());

        foreach (Draggable d in draggables) {
            d.Reset();
        }

    }

}
