using UnityEngine;
using System.Collections;

public class EndNode : MonoBehaviour {
	public float difficulty;
	
	private Ending ending;
	public Ending Ending {
		set {
			ending = value;
			renderer.material.color = ending.color;
		}
		get {
			return ending;
		}
	}
	
	public void notifyEnd() {
		MiniGameController.endMiniGame(ending.edgeId);
	}
}
