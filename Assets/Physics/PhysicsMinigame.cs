using UnityEngine;
using System.Collections;
using System.Linq;

public class PhysicsMinigame : SceneLoadingMiniGame {
	protected override void onMyLevelLoaded ()
	{
		Ending[] endings = Ending.findEndings(data).ToArray();
		EdgeOnTap[] edgeSlots = (EdgeOnTap[])FindObjectsOfType(typeof(EdgeOnTap));
		int i = 0;
		foreach (Ending ending in endings) {
			edgeSlots[i++].Ending = ending;
		}
	}
}
