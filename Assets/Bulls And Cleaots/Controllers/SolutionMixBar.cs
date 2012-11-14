using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnergyBar))]
public class SolutionMixBar : MonoBehaviour {

    void Start() {
        GetComponent<EnergyBar>().labelText = "Solution Mix";
    }

    public void FoundSolution(float barFullness) {
        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Fullness = barFullness;
    }
}

