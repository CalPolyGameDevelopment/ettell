using UnityEngine;
using System.Collections;

public class PeriodicTranslation : MonoBehaviour {
	
	private const float TWO_PI = Mathf.PI * 2f;
	
	public Vector3 axisMagnitudes;
	public float startSin;
	public float frequency;
	
	private Vector3 startPos;
	private float t;
	private Vector3 offset;
	private Vector3 Offset {
		get {
			offset.x = Mathf.Sin(t) * axisMagnitudes.x;
			offset.y = Mathf.Sin(t) * axisMagnitudes.y;
			offset.z = Mathf.Sin(t) * axisMagnitudes.z;
			return offset;
		}
	}
	
	void Start () {
		t = startSin;
		startPos = transform.position - Offset;
	}
	
	void Update () {
		t = (t + Time.deltaTime) % TWO_PI;
		transform.position = startPos + Offset;
	}
}
