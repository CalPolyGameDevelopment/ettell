using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
 
    public List<GameObject> waves;
    
	// Use this for initialization
	void Start () {

        StartCoroutine("doWaves");
	}
	
    public IEnumerable doWaves(){

// Turn off the not used warning for now.
#pragma warning disable 219
        
        foreach (GameObject wave in waves) {
            yield return StartCoroutine("wave.doPhases");
 
        }
#pragma warning restore 219
        
    }
 
    
    
}
