using UnityEngine;
using System.Collections;

public class PipeController : MonoBehaviour {
	
	public bool playerHere;
	public bool PlayerHere {
		set {
			if (value != playerHere) {
				playerHere = value;
				if (value && GetComponent<EndNode>() != null) {
					GetComponent<EndNode>().notifyEnd();
				}
			}
		}
	}
	
	private RaycastHit searchLaser;
	private Ray searchDirection;

	void Start () {
	}
	
	public void kill(string edgeId) {
		if (playerHere) {
			MiniGameController.endMiniGame(edgeId);
		}
		Destroy(gameObject);
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
			PipeController other = searchLaser.transform.gameObject.GetComponent<PipeController>();
			if (other != null) {
				PlayerHere = false;
				GetComponentInChildren<EttellPipeController>().Lerp(this, other);
			}
		}
	}
}
