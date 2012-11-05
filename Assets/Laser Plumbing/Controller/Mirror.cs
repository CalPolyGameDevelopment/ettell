using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {
	
	public float fireDistance;
	
	protected int rotations;
	
	protected bool rotateable;
	
	private static Vector3 rotAxis = new Vector3(0f, 0f, 1f);
	
	protected float rotation {
		get {
			return 90f * rotations + 45f;
		}
	}
	
	private void reRotate() {
		transform.rotation = Quaternion.identity;
		transform.Rotate(rotAxis, rotation);
	}
	
	void Start() {
		reRotate();
		rotateable = true;
	}
	
	void Update() {
		if (rotateable && ClickRaycast.clickedMe(gameObject)) {
			rotations++;
			reRotate();
		}
	}

	public void hitByLaser(Laser laser) {
		rotateable = false;
		
		int quadrant = laser.direction.x > 0 ? (laser.direction.y > 0 ? 0 : 3) : (laser.direction.y > 0 ? 1 : 2);
		float firstQuadrantTan = Mathf.Atan(Mathf.Abs(laser.direction.y) / Mathf.Abs(laser.direction.x)) / Mathf.Deg2Rad;
		float laserAngle = 
			quadrant == 0 ? firstQuadrantTan : (
			quadrant == 1 ? 180f - firstQuadrantTan : (
			quadrant == 2 ? 180f + firstQuadrantTan : (
			360f - firstQuadrantTan
		)));
		
		float clockTestAngle = (laserAngle + 45f) % 360f;
		bool rotClock = clockTestAngle < rotation + 0.001f && clockTestAngle > rotation - 0.001f;
		float antiClockTestAngle = (laserAngle + 315f) % 360f;
		bool rotAnti = antiClockTestAngle < rotation + 0.001f && antiClockTestAngle > rotation - 0.001f;
		
		Vector3 newDirection;
		
		if (rotClock) {
			newDirection = new Vector3(
				laser.direction.y,
				-laser.direction.x,
				laser.direction.z
			);
		}
		else if (rotAnti) {
			newDirection = new Vector3(
				-laser.direction.y,
				laser.direction.x,
				laser.direction.z
			);
		}
		else {
			gameObject.layer = 2; //won't get detected by the next raycast
			Destroy(gameObject);
			laser.project(laser.direction);
			laser.Ready = true;
			return;
		}
		
		Vector3 origin = transform.position + (newDirection.normalized * fireDistance);
		laser.Ready = false;
		GameObject go = Instantiate(laser.gameObject) as GameObject;
		go.transform.parent = laser.transform.parent;
		laser.Ready = true;
		Laser l = go.GetComponent<Laser>();
		l.origin = origin;
		l.project(newDirection);
		l.Ready = true;
	}
}
