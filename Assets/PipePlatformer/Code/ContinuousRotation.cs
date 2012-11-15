using UnityEngine;
using System.Collections;

public class ContinuousRotation : MonoBehaviour {
	public Vector3 axisMagnitudes;
	void FixedUpdate () {
		transform.Rotate(axisMagnitudes * Time.deltaTime);
	}
}
