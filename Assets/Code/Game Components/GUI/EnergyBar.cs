using UnityEngine;
using System.Collections;

public class EnergyBar : MonoBehaviour {

    public float energy = 0;
    public int maxEnergy = 100;
    public float energyIncrement = 25;
    
    public Color backgroundColor = Color.black;
    public Color forgroundColor = Color.red;
 
    
    public string labelText = "";
   
    
    public Rect box = new Rect(10, 10, 400, 20);

    private Texture2D background;
    private Texture2D foreground;
    
    void Start()
    {

        background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

        background.SetPixel(0, 0, backgroundColor);
        foreground.SetPixel(0, 0, forgroundColor);
        
        background.Apply();
        foreground.Apply();
    }


    void OnGUI()
    {
        GUI.BeginGroup(box);
        {
    
            
            GUI.DrawTexture(new Rect(0, 0, box.width, box.height), background, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(0, 0, box.width*energy/maxEnergy, box.height), foreground, ScaleMode.StretchToFill);
            GUI.Label(new Rect(10,0,50,20), labelText);
                 }
        GUI.EndGroup(); ;
    }
 
    public float Value {
        get{
            return energy;
        }
        set{
            // Make sure energy can't go over max or under min?
            energy = value;
        }
    }
    
    
    public void DecrementEnergy(){
        
        energy -= energyIncrement;
        
    }
    
    public void IncrementEnergy(){
        
          energy += energyIncrement;
    }
}