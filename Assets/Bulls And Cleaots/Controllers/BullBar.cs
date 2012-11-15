using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnergyBar))]
public class BullBar : MonoBehaviour {

    void Start() {
        GetComponent<EnergyBar>().labelText = "Bulls";
    }
	
    public void FoundBulls(float barFullness) {
        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Fullness = barFullness;
    }
}
