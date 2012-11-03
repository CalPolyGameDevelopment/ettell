using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FixedAxisCharacterController: MonoBehaviour {
	
	public float maxSpeed;
	public bool instantaneousAcceleration;
	public float acceleration;
	
	private CharacterController controller;
	private Vector3 velocity = Vector3.zero;

	
	// Use this for initialization
	void Start () {
	
		if (instantaneousAcceleration){
			acceleration = maxSpeed;	
		}
		
	}

	
	// Update is called once per frame
	void Update () {
	
		velocity = Vector3.zero;
		controller = GetComponent(
				typeof(CharacterController)) as CharacterController;
		velocity.x = Input.GetAxis("Horizontal");
		velocity = transform.TransformDirection(
			velocity * maxSpeed * Time.deltaTime);

		controller.Move(velocity);
	}
}
