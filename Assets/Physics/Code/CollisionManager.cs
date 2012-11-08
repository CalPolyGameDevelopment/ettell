using UnityEngine;
using System.Collections;

public abstract class CollisionManager : MonoBehaviour {
	public enum collisionStatus {
		Enter,
		Stay,
		Exit
	};
	public struct CollisionData {
		public bool trigger;
		public collisionStatus status;
		public Collider other;
		public Collision collisionInfo;
		public CollisionData(
			bool trigger,
			collisionStatus status,
			Collider other,
			Collision collisionInfo
		) {
			this.trigger = trigger;
			this.status = status;
			this.other = other;
			this.collisionInfo = collisionInfo;
		}
	};
	public const string messageName = "manageCollision";
	
	
	public abstract void manageCollision(CollisionData data);
}
