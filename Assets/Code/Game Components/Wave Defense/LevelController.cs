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
        foreach (GameObject wave in waves) {
            yield return StartCoroutine("wave.doPhases");
        }
    }
 
    
    
}
