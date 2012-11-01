using UnityEngine;
using System.Collections;
using System.Xml;

public class StoryController : MonoBehaviour {
	
	public ImmutableData story;
	
	private static StoryController singleton;
	
	void Start () {
		singleton = this;
		StartCoroutine(loadCurNode());
	}
	
	private IEnumerator loadCurNode() {
		while (!UserProperty.ready) {
			yield return 0;
		}
		while (!MiniGameController.ready) {
			yield return 0;
		}
		string stateMachineNode = UserProperty.getProp("curNode");
		XmlNode curNode = story.xmlDoc.SelectSingleNode(string.Format(@"//*[@id='{0}']/minigame", stateMachineNode));
		MiniGameController.startMiniGame(curNode.Attributes[XmlUtilities.data].Value, curNode);
	}
	
	public static void TraverseEdge(string edgeId) {
		XmlNode edge = singleton.story.xmlDoc.SelectSingleNode(string.Format(@"//*[@id='{0}']", edgeId));
		UserProperty.setProp("curNode", edge.Attributes[XmlUtilities.data].Value);
		singleton.StartCoroutine(singleton.loadCurNode());
	}
}
