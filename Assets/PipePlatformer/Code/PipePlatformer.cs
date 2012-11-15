using UnityEngine;
using System.Collections;
using System.Linq;

public class PipePlatformer : Platformer {
	protected override void onMyLevelLoaded ()
	{
		Ending[] endings = Ending.findEndings(data).OrderBy<Ending, float>(x => x.difficulty).ToArray();
		EndNode[] nodes = FindObjectsOfType(typeof(EndNode)).Cast<EndNode>().OrderBy<EndNode, float>(x => x.difficulty).ToArray();
		if (nodes.Length != endings.Length) {
			Debug.LogWarning(Application.loadedLevelName + " is only designed to handle " + nodes.Length.ToString() + " endings");
		}
		for (int i = 0; i < nodes.Length && i < endings.Length; i++) {
			nodes[i].Ending = endings[i];
		}
	}
}
