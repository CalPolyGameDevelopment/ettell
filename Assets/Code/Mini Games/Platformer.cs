using UnityEngine;
using System.Collections;
using System.Xml;

public class Platformer : SceneLoadingMiniGame {
	
	void OnGUI() {
		if (GUILayout.Button("test")) {
			MiniGameController.endMiniGame("firstPuzzleRecur");
		}
	}
}
