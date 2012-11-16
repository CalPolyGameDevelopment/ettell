using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Dialog : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string PROMPT = "prompt";
	private const string RESPONSE = "response";
	
	private const int BUTTON_SIZE_MODIFIER = 30;
	
	public GUIStyle style;
	private Ending[][] options;
	private string[] promptText;
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			
			promptText = XmlUtilities.getDataFromNode<string>(data, PROMPT, XmlUtilities.getData<string>).ToArray();
			Ending[] possibleEndings = Ending.findEndings(data).ToArray();
			
			if (possibleEndings.Length == 1) {
				MiniGameController.endMiniGame(possibleEndings[0].edgeId);
			}
			
			int height = Mathf.RoundToInt(Mathf.Sqrt((float)possibleEndings.Length));
			HashSet<Ending>[] rows = new HashSet<Ending>[height];
			for (int i = 0; i < height; i++) {
				rows[i] = new HashSet<Ending>();
			}
			// This is a greedy approximation to the knapsack problem.  Trying to equalize the number of characters per line
			foreach (Ending ending in possibleEndings.OrderBy<Ending, int>(e => -e.displayText.Length)) {
				rows[0].Add(ending);
				rows = rows.OrderBy<HashSet<Ending>, int>(
					r => r.Aggregate<Ending, int>(0, 
						(agg, e) => agg + e.displayText.Length + BUTTON_SIZE_MODIFIER
					)
				).ToArray();
			}
			
			options = new Ending[height][];
			for (int i = 0; i < height; i++) {
				options[i] = rows[i].ToArray();
			}
		}
	}
	
	void OnGUI() {
		if (data == null) {
			return;
		}
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, 20f));
		foreach (string promptLine in promptText) {
			GUILayout.Label(promptLine);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0f, 20f, Screen.width, Screen.height - 20f));
		foreach (Ending[] endings in options) {
			GUILayout.BeginHorizontal();
			foreach (Ending ending in endings) {
				if (GUILayout.Button(ending.displayText, style, GUILayout.ExpandHeight(true))) {
					MiniGameController.endMiniGame(ending.edgeId);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}