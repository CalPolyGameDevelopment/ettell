using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Dialog : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string PROMPT = "prompt";
	private const string RESPONSE = "response";
	private const int BUTTON_SIZE_MODIFIER = 30;
	
	public GUIStyle style;
	private XmlNode[][] options;
	private string[] promptText;
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			XmlNode[] possibleOptions = data.SelectNodes(RESPONSE).Cast<XmlNode>().Where(xn => Requirements.pass(xn)).ToArray();
			promptText = XmlUtilities.getDataFromNode<string>(data, PROMPT, XmlUtilities.getData).ToArray();
			if (possibleOptions.Length == 1) {
				MiniGameController.endMiniGame(XmlUtilities.getData(possibleOptions[0]));
			}
			
			int height = Mathf.RoundToInt(Mathf.Sqrt((float)possibleOptions.Length));
			HashSet<XmlNode>[] rows = new HashSet<XmlNode>[height];
			for (int i = 0; i < height; i++) {
				rows[i] = new HashSet<XmlNode>();
			}
			// This is a greedy approximation to the knapsack problem.  Trying to equalize the number of characters per line
			foreach (XmlNode xn in possibleOptions.OrderBy<XmlNode, int>(xn => -XmlUtilities.getData(xn).Length)) {
				rows[0].Add(xn);
				rows = rows.OrderBy<HashSet<XmlNode>, int>(
					r => r.Aggregate<XmlNode, int>(0, 
						(agg, xml) => agg + XmlUtilities.getData(xml).Length + BUTTON_SIZE_MODIFIER
					)
				).ToArray();
			}
			
			options = new XmlNode[height][];
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
		foreach (XmlNode[] xns in options) {
			GUILayout.BeginHorizontal();
			foreach (XmlNode xn in xns) {
				if (GUILayout.Button(XmlUtilities.getData(xn), style, GUILayout.ExpandHeight(true))) {
					MiniGameController.endMiniGame(xn.Attributes[XmlUtilities.edge].Value);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
}