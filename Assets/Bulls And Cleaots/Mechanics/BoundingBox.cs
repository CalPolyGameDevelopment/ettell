using UnityEngine;
using System.Collections;

/// <summary>
/// Bounding box.
///  
/// Assumes XZ plane projection centered on the parent GameObjects transform.
/// </summary>
public class BoundingBox : MonoBehaviour {

	public float width;
	public float height;
	private Color retinaBurningColor = new Color(255, 0, 255);
	
	Vector3 Size {
		get{
			return new Vector3(width, 0, height);			
		}
	}
	
	public Rect Bounds{
		get {
			float left, top;
			left = transform.position.x - width/2.0f;
			top = transform.position.z - height;
			return new Rect(left, top, width, height);
		}
	}
	
	void OnDrawGizmos(){
		Gizmos.color = retinaBurningColor;
		Gizmos.DrawWireCube(transform.position, Size);

	}
}
