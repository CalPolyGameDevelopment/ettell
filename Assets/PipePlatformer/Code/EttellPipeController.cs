using UnityEngine;
using System.Collections;

public class EttellPipeController : MonoBehaviour {
	
	public float speed;
	
	private float t;
	private PipeController fromPipe;
	private PipeController toPipe;
	
	public void Lerp(PipeController fromPipe, PipeController toPipe) {
		this.fromPipe = fromPipe;
		this.toPipe = toPipe;
		t = 0f;
		transform.localPosition = transform.parent.localPosition;
		transform.parent = transform.parent.parent;
	}
	
	void Start() {
		t = -1f;
	}
	
	void Update() {
		Vector3 cameraPos = Camera.mainCamera.transform.position;
		cameraPos.x = this.transform.position.x;
		cameraPos.y = this.transform.position.y;
		Camera.mainCamera.transform.position = cameraPos;
		Camera.mainCamera.transform.rotation = transform.rotation;
	}
	
	void FixedUpdate() {
		if (t < 0f) {
			return;
		}
		t += Time.deltaTime;
		
		transform.localPosition = Vector3.Lerp(fromPipe.transform.localPosition, toPipe.transform.localPosition, t / speed);
		transform.rotation = Quaternion.Lerp(fromPipe.transform.localRotation, toPipe.transform.localRotation, t / speed);
		
		if (t > speed) {
			t = -1f;
			toPipe.PlayerHere = true;
			transform.parent = toPipe.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}
}