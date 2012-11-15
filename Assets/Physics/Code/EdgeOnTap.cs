using UnityEngine;
using System.Collections;

public class EdgeOnTap : CollisionManager {
	private Ending ending;
	public Ending Ending {
		set {
			ending = value;
			renderer.material.color = ending.color;
		}
	}
	public override void manageCollision (CollisionData data)
	{
		MiniGameController.endMiniGame(ending.edgeId);
	}
}
