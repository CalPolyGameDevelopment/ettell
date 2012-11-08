using UnityEngine;
using System.Collections;

public class EdgeOnTap : CollisionManager {
	private XmlUtilities.EdgeColor identity;
	public XmlUtilities.EdgeColor Identity {
		set {
			identity = value;
			renderer.material.color = identity.appearance;
		}
	}
	public override void manageCollision (CollisionData data)
	{
		MiniGameController.endMiniGame(identity.edgeId);
	}
}
