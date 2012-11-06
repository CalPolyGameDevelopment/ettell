using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PowerBar))]
public class BullBar : MonoBehaviour {

    public void FoundBull(){
        PowerBar powerBar = GetComponent(typeof(PowerBar)) as PowerBar;
        powerBar.IncrementPower();
    }
}
