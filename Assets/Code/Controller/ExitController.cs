using UnityEngine;
using System.Collections;

public class ExitController : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}