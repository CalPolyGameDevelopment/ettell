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
			Ray prediction = new Ray(origin, transform.forward);
			RaycastHit rch;
			if (Physics.Raycast(prediction, out rch)) {
				GameObject go = Instantiate(laser) as GameObject;
				Laser l = go.GetComponent<Laser>();
				l.target = rch.transform.gameObject;
				l.origin = origin;
				l.Ready = true;
			}
			else {
				throw new MissingReferenceException("Laser is not blocked by any gameobject, therefore ending is not within edges");
			}
		}
	}
}
