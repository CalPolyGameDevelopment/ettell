using UnityEngine;
using System.Collections;

public class RisingWaterEnding : CollisionManager {
	public float speed;
	
	public override void manageCollision (CollisionData data)
	{
		PipeController other = data.other.GetComponent<PipeController>();
		if (other != null) {
			other.kill(GetComponent<EndNode>().Ending.edgeId);
		}
	}
	
	void FixedUpdate() {
		Vector3 pos = transform.position;
		pos.y += speed * Time.deltaTime;
		transform.position = pos;
	}
}
