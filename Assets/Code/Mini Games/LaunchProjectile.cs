using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]
public class ProjectileLauncher: MonoBehaviour {
	
	// How often the launcher reloads and shoots another projectile.
	public float launchRate;
	//public Projectile projectile; 
	
	// The count down to the next launch event.
	private float launchTimer;
	
	// Use this for initialization
	void Start () {
		launchTimer = launchRate;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		launchTimer -= Time.deltaTime;
		
		if (launchTimer <= 0.0f){
			// instantiate projectile 
			// do launch
			// reset launch timer
			launchTimer = launchRate;
		}
		
	}


}
