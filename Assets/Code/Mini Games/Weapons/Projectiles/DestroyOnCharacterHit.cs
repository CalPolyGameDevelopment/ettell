using UnityEngine;
using System.Collections;

/// <summary>
/// An ad hoc script to destroy the projectile "cubes" that hit
/// the character. There is a better way to do this but this 
/// works right now.
/// </summary>
[RequireComponent (typeof (CharacterController))]
public class DestroyOnCharacterHit : MonoBehaviour {
	
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
        // Get the game
		GameObject obj = hit.collider.gameObject;
		if (obj.name == "Cube"){
			Destroy(obj);
		}

    }

}
