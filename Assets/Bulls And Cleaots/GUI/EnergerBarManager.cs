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
	private bool ready = false;

	private delegate void setFunction(int index, object o);

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
			// Equally space the bar pairs through the height of the game.
			bullsBar.box.y = (Screen.height * (index+0.5f))/slnCount;
			cleotsBar.box.y = bullsBar.box.y + bullsBar.box.height + 5;
			
			
			
			playerInfoBars[index] = new EnergyBarWrapper(bullsBar, cleotsBar);
			
		}

			ready = true;
	}

	public bool IsReady{
			get{ return ready; }
	}

	public void SetBulls(int index, object bulls) {
		if(IsReady){
			playerInfoBars[index].SetBulls((int)bulls, slnLength);
		}
		else {
			StartCoroutine(deferSetUntilReady(SetBulls, index, bulls));
		}
	}
	
	public void SetCleots(int index, object cleots) {
		if(IsReady){
			playerInfoBars[index].SetCleots((int)cleots, slnLength);
		}
		else {
			StartCoroutine(deferSetUntilReady(SetCleots, index, cleots));
		}
	}

	public void SetLabel(int index, object label) {
		if(IsReady){
			playerInfoBars[index].SetLabel((string)label);
		}
		else {
			StartCoroutine(deferSetUntilReady(SetLabel,index,label));
		}
	}

	private IEnumerator deferSetUntilReady(setFunction f, int index, object o){
		while(!IsReady){
			yield return 0;
		}
		f(index, o);
	}
	
}
}