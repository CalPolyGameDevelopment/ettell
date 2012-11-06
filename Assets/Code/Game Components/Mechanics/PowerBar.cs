using UnityEngine;
using System.Collections;

public class PowerBar : MonoBehaviour {

    public float power = 50;
    public int maxPower = 100;
    public float powerIncrement = 10;
    
    public Color backgroundColor = Color.black;
    public Color forgroundColor = Color.red;

    public Material addEventMaterial;
    
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
            GUI.DrawTexture(new Rect(0, 0, box.width*power/maxPower, box.height), foreground, ScaleMode.StretchToFill);

                 }
        GUI.EndGroup(); ;
    }
 
    
    void AddPower(GameObject obj){
        
       if (obj.rigidbody.renderer.material.color == addEventMaterial.color)
            power += powerIncrement;
    }
}