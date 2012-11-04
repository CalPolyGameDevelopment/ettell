using UnityEngine;
using System.Collections;

public class Sink : MonoBehaviour {
	
	public string edge;
	private bool hit;
	
	public void hitByLaser(Laser laser) {
		if (!hit) {
			MiniGameController.endMiniGame(edge);
			hit = true;
		}
	}
}
