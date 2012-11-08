using UnityEngine;
using System.Collections;

public class CollisionResource : MonoBehaviour {
    void OnCollisionStay(Collision collisionInfo) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				false,
				CollisionManager.collisionStatus.Stay,
				collisionInfo.collider,
				collisionInfo
			)
		);
	}
    void OnCollisionEnter(Collision collisionInfo) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				false,
				CollisionManager.collisionStatus.Enter,
				collisionInfo.collider,
				collisionInfo
			)
		);
	}
    void OnCollisionExit(Collision collisionInfo) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				false,
				CollisionManager.collisionStatus.Exit,
				collisionInfo.collider,
				collisionInfo
			)
		);
	}
    void OnTriggerStay(Collider other) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				true,
				CollisionManager.collisionStatus.Stay,
				other,
				null
			)
		);
	}
    void OnTriggerEnter(Collider other) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				true,
				CollisionManager.collisionStatus.Enter,
				other,
				null
			)
		);
	}
    void OnTriggerExit(Collider other) {
		SendMessageUpwards(
			CollisionManager.messageName,
			new CollisionManager.CollisionData(
				true,
				CollisionManager.collisionStatus.Exit,
				other,
				null
			)
		);
	}
}
