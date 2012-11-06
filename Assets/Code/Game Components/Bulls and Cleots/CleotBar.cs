using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PowerBar))]
public class CleotBar : MonoBehaviour {

    public void FoundCleot(){
        PowerBar powerBar = GetComponent(typeof(PowerBar)) as PowerBar;
        powerBar.IncrementPower();
    }
}
