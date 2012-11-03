using UnityEngine;
using System.Collections;


public class ProjectileLauncher: MonoBehaviour {
	
	// How often the launcher reloads and shoots another projectile.
	public float launchRate;
 	
	public int launchForce;
	
	public Projectile projectile;
	
	public Vector3 launchVector;
	public Vector3 launchTorque;
	public Transform projectileEgress;
	// The count down to the next launch event.
	private float launchTimer;
	private float launchDelay;
	private const float TIME_FACTOR = 50.0f;
	
	// Use this for initialization
	void Start () {
		launchDelay = TIME_FACTOR/launchRate;
		launchTimer = launchDelay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		launchTimer -= Time.deltaTime;
		
		if (launchTimer <= 0.0f) {
			// instantiate projectile 
			Projectile clone = Instantiate(projectile, 
				projectileEgress.position, projectileEgress.rotation) as Projectile;
			// add launch force
			clone.rigidbody.AddForce(launchVector * launchForce);
			clone.rigidbody.AddTorque(launchTorque);
			// reset launch timer
			launchTimer = launchDelay;
		}
		
	}


}
