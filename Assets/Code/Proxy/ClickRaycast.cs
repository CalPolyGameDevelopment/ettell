using UnityEngine;
using System.Collections;

public class ClickRaycast : MonoBehaviour {
	
	private static ClickRaycast singleton;
	
	private RaycastHit cache;
	private bool cached;
	public static RaycastHit Cache {
		get {
			return singleton.cache;
		}
	}
	
	void Start () {
		singleton = this;
	}
	
	void Update () {
		cached = false;
	}
	
	public static bool mouseRaycast() {
		if (Input.GetMouseButtonDown(0)) {// && Camera.main != null) {
			if (!singleton.cached) {
				Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out singleton.cache);
				singleton.cached = true;
			}
			return true;
		}
		return false;
	}
	
	public static bool clickedMe(GameObject me) {
		if (!mouseRaycast()) {
			return false;
		}
		return singleton.cache.transform != null && singleton.cache.transform.gameObject == me;
	}
}