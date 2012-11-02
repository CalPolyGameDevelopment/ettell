using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class MiniGameController : MonoBehaviour {
	
	private static MiniGameController singleton;
	
	public static bool ready {
		get {
			return singleton != null;
		}
	}
	
	public GameObject dialog;
	public GameObject platformer;
	
	private Dictionary<string, GameObject> miniGameTypes;
	
	private GameObject current;
	public static GameObject Current {
		get {
			return singleton.current;
		}
	}
	
	void Start () {
		miniGameTypes = new Dictionary<string, GameObject>();
		miniGameTypes["dialog"] = dialog;
		miniGameTypes["platformer"] = platformer;
		singleton = this;
	}
	
	public static void startMiniGame(string name, XmlNode gameData) {
		singleton.current = (GameObject)Instantiate(singleton.miniGameTypes[name]);
		singleton.current.transform.parent = singleton.gameObject.transform;
		singleton.current.GetComponent<MiniGameAPI>().Data = gameData;
	}
	
	public static void endMiniGame(string result) {
		Destroy(singleton.current);
		singleton.current = null;
		StoryController.TraverseEdge(result);
	}
}