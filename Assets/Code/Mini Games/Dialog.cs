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
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, 20f));
		foreach (string promptLine in promptText()) {
			GUILayout.Label(promptLine);
		}
		GUILayout.EndArea();
		XmlNodeList options = data.SelectNodes("descendant::response");
		int width;
		for (
			width = (int)Mathf.Sqrt(options.Count);
			width > 0 && width * (int)(((float)options.Count) / width) != options.Count;
			width++
		);
		GUILayout.BeginArea(new Rect(0f, 20f, Screen.width, Screen.height - 20f));
		int cur = 0;
		foreach (XmlNode xn in options) {
			if (cur % width == 0) {
				GUILayout.BeginHorizontal();
			}
			GUI.enabled = Requirements.pass(xn);
			if (GUILayout.Button(XmlUtilities.getData(xn), GUILayout.ExpandHeight(true))) {
				MiniGameController.endMiniGame(xn.Attributes[XmlUtilities.edge].Value);
			}
			GUI.enabled = true;
			if (++cur % width == 0) {
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndArea();
	}
}
