using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DialogData : ImmutableData {
	
	protected string promptText() {
		return getDatum<string>("descendant::prompt", XmlUtilities.getData);
	}
	
	protected IEnumerable<string> responseTexts() {
		return getData<string>("descendant::response", XmlUtilities.getData);
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
