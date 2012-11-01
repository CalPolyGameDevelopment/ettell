using UnityEngine;
using System.Collections;
using System.Xml;

public class StoryController : MonoBehaviour {
	
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
		MiniGameController.startMiniGame(curNode.Attributes[XmlUtilities.data].Value, curNode);
		ready = true;
	}
	
	public static void TraverseEdge(string edgeId) {
		XmlNode edge = singleton.story.xmlDoc.SelectSingleNode(string.Format(@"//*[@id='{0}']", edgeId));
		foreach (XmlNode consequence in edge.SelectNodes(XmlUtilities.consequence)) {
			Consequences.apply(consequence);
		}
		UserProperty.setProp("curNode", edge.Attributes[XmlUtilities.data].Value);
		singleton.StartCoroutine(singleton.loadCurNode());
	}
}
