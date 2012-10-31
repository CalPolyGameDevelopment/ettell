using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DialogData : MiniGameData {
	
	private string getText(XmlNode xn) {
		return xn.Attributes["text"].Value;
	}
	
	protected string promptText() {
		return getDatum<string>("descendant::prompt", getText);
	}
	
	protected IEnumerable<string> responseTexts() {
		return getData<string>("descendant::response", getText);
	}
	
	void OnGUI() {
		GUILayout.Label(promptText());
		foreach (string r in responseTexts()) {
			if (GUILayout.Button(r)) {
				//traverse edge
			}
		}
	}
}
