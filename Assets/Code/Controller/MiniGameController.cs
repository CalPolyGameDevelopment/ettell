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
    public GameObject redBluePlatformer;
	public GameObject laserPlumber;
	public GameObject match3;
	public GameObject bullsAndCleots;
	public GameObject physics;
    
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
		miniGameTypes["laserPlumber"] = laserPlumber;
        miniGameTypes["RedBlueTurretPlatformer"] = redBluePlatformer;
        miniGameTypes["match3"] = match3;
        miniGameTypes["BullsAndCleots"] = bullsAndCleots;
        miniGameTypes["physics"] = physics;
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
		singleton.StartCoroutine(finishEndMiniGame(result));
	}
	
	public static IEnumerator<int> finishEndMiniGame(string result) {
		while (!(Application.loadedLevelName == "Empty" || Application.loadedLevelName == "Start")) {
			yield return 0;
		}
		StoryController.TraverseEdge(result);
	
        singleton.StartCoroutine(finishEndMiniGame(result));
 }

 public static IEnumerator<int> finishEndMiniGame(string result) {
     while (!(Application.loadedLevelName == "Empty" || Application.loadedLevelName == "Start")) {
         yield return 0;
     }
     StoryController.TraverseEdge(result);
 }

}