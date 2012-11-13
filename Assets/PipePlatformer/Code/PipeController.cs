using UnityEngine;
using System.Collections;

public class PipeController : MonoBehaviour {
	
	public bool playerHere;
	public bool PlayerHere {
		set {
			if (value != playerHere) {
				playerHere = value;
				GetComponent<ParticleSystem>().enableEmission = playerHere;
			}
		}
	}
	
	private RaycastHit searchLaser;
	private Ray searchDirection;

	void Start () {	
		if (playerHere) {
			GetComponent<LightningArcs>().Begin();
		}
	}
	
	void Update () {
		if (!playerHere) {
			return;
		}
		
		bool moved = false;
		bool hitSomething = false;
		
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			searchDirection.direction = -transform.right;
			moved = true;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			searchDirection.direction = transform.right;
			moved = true;
		}
		
		if (moved) {
			searchDirection.origin = transform.position;
			hitSomething = Physics.Raycast(searchDirection, out searchLaser);
		}
		
		if (hitSomething) {
		}
		else {
		}
	}
}
