using UnityEngine;
using System.Collections;
using System.Linq;

public class PhysicsMinigame : SceneLoadingMiniGame {
	protected override void onMyLevelLoaded ()
	{
		XmlUtilities.EdgeColor[] colors = XmlUtilities.getDataFromNode<XmlUtilities.EdgeColor>(data, XmlUtilities.color, XmlUtilities.parseColor).ToArray();
		EdgeOnTap[] edgeSlots = (EdgeOnTap[])FindObjectsOfType(typeof(EdgeOnTap));
		int i = 0;
		foreach (XmlUtilities.EdgeColor color in colors) {
			edgeSlots[i++].Identity = color;
		}
	}
}
