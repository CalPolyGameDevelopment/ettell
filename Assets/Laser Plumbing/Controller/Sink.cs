using UnityEngine;
using System.Collections;

public class Sink : MonoBehaviour {
	
	private Ending edge;
	private bool hit;
	
	public Ending Edge {
		set {
			edge = value;
			GetComponentInChildren<MeshRenderer>().material.color = edge.color;
		}
	}
	
	public void hitByLaser(Laser laser) {
		if (!hit) {
			MiniGameController.endMiniGame(edge.edgeId);
			hit = true;
		}
	}
}
