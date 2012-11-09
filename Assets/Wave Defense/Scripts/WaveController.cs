using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {
 
    public List<Wave> waves;
    
    void Start(){
       foreach (Wave wave in waves){
            wave.Spawn(gameObject);
             break;
        }
    }
    
    
    
}
