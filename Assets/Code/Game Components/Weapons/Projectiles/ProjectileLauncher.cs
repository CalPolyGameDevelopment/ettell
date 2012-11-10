using UnityEngine;
using System.Collections;

/// <summary>
/// Projectile Launcher may be attached to any GameObject to give 
/// that Object the license to kill.
/// </summary>
public class ProjectileLauncher: MonoBehaviour {
	
	// How often the launcher reloads and shoots another projectile.
	public float launchRate;
 	
	// The "ummpff" the the launcher places on
	// projectiles
	public int launchForce;
	
	// Cloneobject
	public Projectile projectile;
	
	// The initial vector in which the projectiles 
	// fired from this launcher will travel
	public Vector3 launchVector;
	
	// The initial "spin" of the projectiles
	public Vector3 launchTorque;
	
	// The location on the GameObject where the 
	public Transform projectileEgress;
	
	// The count down to the next launch event.
	private float launchTimer;
	
	private float launchDelay;
	
    private bool isEnabled = true;
    
	// A bit of skew to make sure that launch factor
	// can be set to a reasonable number 
	private const float TIME_FACTOR = 50.0f;
	
    public void Enable(){
        isEnabled = true;
    }
    
    public void Disable(){
        isEnabled = false;
    }
	// Use this for initialization
	void Start () {
	    
        launchDelay = TIME_FACTOR/launchRate;
		launchTimer = launchDelay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        if (!isEnabled)
            return;
        
		// Take the amount of time that has passed off
		// the launch timer since the last call to this 
		// function.
		launchTimer -= Time.deltaTime;
		
		// if the launch timer has "fired".
		if (launchTimer <= 0.0f) {
			
			// instantiate projectile 
			Projectile clone = Instantiate(projectile, 
				projectileEgress.position, projectileEgress.rotation) as Projectile;
			
			// Make the projectile clone a child of the turret
			//clone.transform.parent = gameObject.transform;
			
			// Reset the name so it doesn't have "(Clone)" suffix.
			clone.name = projectile.name;
			
			// Set the launch kinetics
			clone.rigidbody.AddForce(launchVector * launchForce);
			clone.rigidbody.AddTorque(launchTorque);
			
			// reset launch timer
			launchTimer = launchDelay;
		}
		
	}


}
