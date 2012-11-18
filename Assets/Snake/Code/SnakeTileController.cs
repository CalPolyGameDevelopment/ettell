using UnityEngine;
using System.Collections;

public class SnakeTileController : MonoBehaviour {
	
	public Color color {
		set {
			if (value == SnakeGame.EMPTY_COLOR) {
				renderer.enabled = false;
			}
			else {
				renderer.enabled = true;
			}
			renderer.material.color = value;
		}
		get {
			return renderer.material.color;
		}
	}
}
