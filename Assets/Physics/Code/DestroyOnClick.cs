using UnityEngine;
using System.Collections;

public class DestroyOnClick : MonoBehaviour {

	void Update () {
		if (ClickRaycast.clickedMe(gameObject)) {
			Destroy(gameObject);
		}
	}
}
