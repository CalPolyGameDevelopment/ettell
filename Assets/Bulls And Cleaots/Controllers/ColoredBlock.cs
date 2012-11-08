using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ColoredBlock : MonoBehaviour {
 
    
    
    public Color color { 
        get{
            return renderer.material.color;
        }
    }
	
    
    
}
