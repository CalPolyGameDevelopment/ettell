using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnergyBar))]
public class CleotBar : MonoBehaviour {

    void Start() {
        GetComponent<EnergyBar>().labelText = "Cleots";
    }

    public void FoundCleots(float barFullness) {
        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Fullness = barFullness;
    }
}
