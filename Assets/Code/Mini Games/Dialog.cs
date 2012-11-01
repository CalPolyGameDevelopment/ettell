using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class Dialog : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
		}
	}
	
	protected IEnumerable<string> promptText() {
		return XmlUtilities.getDataFromNode<string>(data, "descendant::prompt", XmlUtilities.getData);
	}
	
	void OnGUI() {
		if (data == null) {
			return;
		}
		foreach (string promptLine in promptText()) {
			GUILayout.Label(promptLine);
		}
		foreach (XmlNode xn in data.SelectNodes("descendant::response")) {
			GUI.enabled = Requirements.pass(xn);
			if (GUILayout.Button(XmlUtilities.getData(xn))) {
				MiniGameController.endMiniGame(xn.Attributes[XmlUtilities.edge].Value);
			}
			GUI.enabled = true;
		}
	}
}
