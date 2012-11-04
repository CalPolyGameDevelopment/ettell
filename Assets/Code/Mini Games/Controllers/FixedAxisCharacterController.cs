using UnityEngine;
using System.Collections;

/// <summary>
/// Simplistic CharacterController that fixes the character to one axis.
/// 
/// </summary>
[RequireComponent (typeof (CharacterController))]
public class FixedAxisCharacterController: MonoBehaviour {
	
	public enum AxisChoice {
		X,
		Y,
		Z
	};
	
	public float maxSpeed;
	public AxisChoice axis;
	
	private CharacterController controller;
	private Vector3 velocity = Vector3.zero;

	
	// Update is called once per frame
	void Update () {
	
		velocity = Vector3.zero;
		
		controller = GetComponent(
			typeof(CharacterController)) as CharacterController;
		
		// Determine if movement is desired along the 
		// selected axis by the user. If set, "velocity" will be an
		// axis specific unit vector.
		if (axis == AxisChoice.X) {
			velocity.x = Input.GetAxis("Horizontal");
		} else if (axis == AxisChoice.Y) {
			velocity.y = Input.GetAxis("Vertical");
		} else if (axis == AxisChoice.Z) {
			// NOTE: Jump doesn't have a negative button
			velocity.z = Input.GetAxis("Jump");
		}
		
		// Scale the unit vector 
		velocity = transform.TransformDirection(
			velocity * maxSpeed * Time.deltaTime);
		
		// Move the character
		controller.Move(velocity);
	}
}
