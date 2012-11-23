using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BullsAndCleots.Gui {

public class EnergyBarManager : MonoBehaviour {
	
	/// <summary>
	/// The number of solutions for the current BC minigame
	/// </summary>
	public int slnCount;
	public int slnLength;
	private EnergyBarWrapper[] playerInfoBars;
	
	void Start() {
		playerInfoBars = new EnergyBarWrapper[slnCount];
		GameObject go;
		
		foreach (int index in Enumerable.Range(0,slnCount)) {
			go = new GameObject();
			go.transform.parent = transform;
			EnergyBar bullsBar = go.AddComponent<EnergyBar>();
			
			go = new GameObject();
			go.transform.parent = transform;
			EnergyBar cleotsBar = go.AddComponent<EnergyBar>();
			cleotsBar.backgroundColor = Color.gray;
			cleotsBar.forgroundColor = Color.green;
			
			// Adhoc-y layout
			bullsBar.box.y = index * 55 + 10;
			cleotsBar.box.y = bullsBar.box.y + bullsBar.box.height + 5;
			
			
			
			playerInfoBars[index] = new EnergyBarWrapper(bullsBar, cleotsBar);
			
		}
	}
	
	public void SetBulls(int index, int bulls) {
		playerInfoBars[index].SetBulls(bulls, slnLength);
	}
	
	public void SetCleots(int index, int cleots) {
		playerInfoBars[index].SetCleots(cleots, slnLength);
	}
	
	public void SetLabel(int index, string label) {
		playerInfoBars[index].SetLabel(label);
	}
}
}