using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {
	
	public GameObject laser;
	public float firingDelay;
	public float fireDistance;
	
	private float t;
	private bool shot;

	void Start () {
		t = 0f;
	}
	
	void Update () {
		t += Time.deltaTime;
		
		if (!shot && t >= firingDelay) {
			shot = true;
			
			Vector3 origin = transform.position + (transform.forward.normalized * fireDistance);
			GameObject go = Instantiate(laser) as GameObject;
			Laser l = go.GetComponent<Laser>();
			l.origin = origin;
			l.project(transform.forward);
			l.Ready = true;
		}
	}
}
