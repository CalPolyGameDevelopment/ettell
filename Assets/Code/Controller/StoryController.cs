using UnityEngine;
using System.Collections;
using System.Xml;

public class StoryController : MonoBehaviour {
	
	private const string CONSEQUENCE = "consequence";
	
	public ImmutableData story;
	public static ImmutableData Story {
		get {
			return singleton == null ? null : singleton.story;
		}
	}
	
	private static StoryController singleton;
	
	private bool ready = false;
	public static bool Ready {
		get {
			return singleton != null && singleton.ready;
		}
	}
	
	void Start () {
		singleton = this;
		StartCoroutine(loadCurNode());
	}
	
	public static XmlNode findById(string id) {
		if (singleton == null || !Story.xmlAvailable) {
			throw new MissingComponentException("StoryController hasn't been setup");
		}
		return Story.xmlDoc.SelectSingleNode(string.Format("//*[@id='{0}']", id));
	}
	
	private IEnumerator loadCurNode() {
		ready = false;
		while (!UserProperty.ready) {
			yield return 0;
		}
		while (!MiniGameController.ready) {
			yield return 0;
		}
		string stateMachineNode = UserProperty.getProp("curNode");
		XmlNode curNode = story.xmlDoc.SelectSingleNode(string.Format(@"//*[@id='{0}']/minigame", stateMachineNode));
		ready = true;
		
		MiniGameController.startMiniGame(curNode);
	}
	
	public static void TraverseEdge(string edgeId) {
		XmlNode edge = findById(edgeId);
		foreach (XmlNode consequence in edge.SelectNodes(CONSEQUENCE)) {
			Consequences.apply(consequence);
		}
		UserProperty.setProp("curNode", XmlUtilities.getData<string>(edge));
		singleton.StartCoroutine(singleton.loadCurNode());
	}
}
