using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Dialog : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string PROMPT = "prompt";
	private const string RESPONSE = "response";
	private const string INVISIBLE = "invisible";
	private const string INTERACTIVE = "interactive";
	private const string ETTELL_PREFIX = "ettellPrefix";
	
	private const int BUTTON_SIZE_MODIFIER = 5;
	
	public GUIStyle promptStyle;
	public GUIStyle buttonStyle;
	private Ending[][] options;
	private string[] promptText;
	private Rect promptRect;
	private Rect optionsRect;
	
	private bool allowInvisibleChoice {
		get {
			XmlNode xn = data.childNode(INVISIBLE);
			return xn == null ? false : xn.getBool();
		}
	}
	
	private XmlNode interactionSource {
		get {
			XmlNode xn = data.childNode(INTERACTIVE);
			if (xn == null) {
				return null;
			}
			XmlNode source = UserProperty.GetPropNode(xn.getString());
			if (source == null) {
				source = UserProperty.AddProp(xn.getString());
			}
			return source;
		}
	}
	
	private string ettellPrefix {
		get {
			XmlNode xn = data.childNode(ETTELL_PREFIX);
			if (xn == null) {
				return "";
			}
			return xn.getString();
		}
	}
	
	private void addPrompt(string promptLine) {
		XmlNode source = interactionSource;
		if (source == null) {
			return;
		}
		source.CreateStringNode().SetString(promptLine);
	}
	
	private Vector2 calcPromptSize() {
		Vector2 r = new Vector2(0f, 0f);
		foreach (string line in promptText) {
			Vector2 lineSize = promptStyle.CalcSize(new GUIContent(line));
			r.x = Mathf.Max(r.x, lineSize.x);
			r.y += lineSize.y;
		}
		return r;
	}
	
	private Vector2 calcButtonsSize() {
		string inflation = new string('m', BUTTON_SIZE_MODIFIER);
		Vector2 r = Vector2.zero;
		foreach (Ending[] row in options) {
			Vector2 rowSize = Vector2.zero;
			foreach (Ending option in row) {
				Vector2 optionSize = buttonStyle.CalcSize(new GUIContent(option.displayText + inflation));
				rowSize.y = Mathf.Max(rowSize.y, optionSize.y);
				rowSize.x += optionSize.x;
			}
			r.x = Mathf.Max(r.x, rowSize.x);
			r.y += rowSize.y;
		}
		return r;
	}
	
	private string[] getOrCreatePromptText() {
		XmlNode source = interactionSource;
		string [] promptText = data.childNode(PROMPT).getStrings();;
		if (source == null) {
			return promptText;
		}
		foreach (string line in promptText) {
			source.CreateStringNode().SetString(line);
		}
		UserProperty.Save();
		return source.getStrings();
	}
	
	private XmlNode data;
    
	public XmlNode Data {
		set {
			data = value;
			Ending[] possibleEndings = Ending.findEndings(data).ToArray();
			
			if (possibleEndings.Length == 1 && allowInvisibleChoice) {
				MiniGameController.endMiniGame(possibleEndings[0].edgeId);
				return;
			}
			
			promptText = getOrCreatePromptText();
			
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
			
			int maxFontSize = -1;
			int minFontSize = -1;
			int curSize = buttonStyle.fontSize;
			float totalHeight;
			float totalWidth;
			Vector2 promptSize = Vector2.zero;
			Vector2 buttonSize = Vector2.zero;
			while (maxFontSize > minFontSize + 1 || maxFontSize == -1) {
				if (minFontSize > 0 && maxFontSize > 0) {
					curSize = (minFontSize + maxFontSize) / 2;
				}
				else if (maxFontSize > 0) {
					curSize /= 2;
				}
				else {
					curSize *= 2;
				}
				
				promptStyle.fontSize = curSize;
				buttonStyle.fontSize = curSize;
				promptSize = calcPromptSize();
				buttonSize = calcButtonsSize();
				totalWidth = Mathf.Max(promptSize.x, buttonSize.x);
				totalHeight = promptSize.y + buttonSize.y;
				
				if (totalWidth > Screen.width || totalHeight > Screen.height) {
					maxFontSize = curSize;
				}
				else {
					minFontSize = curSize;
				}
			}
			
			promptStyle.fontSize = minFontSize;
			buttonStyle.fontSize = minFontSize;
			promptSize = calcPromptSize();
			buttonSize = calcButtonsSize();
			promptRect = new Rect(0f, 0f, Screen.width, promptSize.y);
			optionsRect = new Rect(0f, promptSize.y, Screen.width, Screen.height - promptSize.y);
		}
	}
	
	void OnGUI() {
		if (data == null) {
			return;
		}
		GUILayout.BeginArea(promptRect);
		foreach (string promptLine in promptText) {
			GUILayout.Label(promptLine, promptStyle);
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(optionsRect);
		foreach (Ending[] endings in options) {
			GUILayout.BeginHorizontal();
			foreach (Ending ending in endings) {
				if (GUILayout.Button(ending.displayText, buttonStyle, GUILayout.ExpandHeight(true))) {
					chooseEnding(ending);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();
	}
	
	protected void chooseEnding(Ending ending) {
		XmlNode source = interactionSource;
		if (source != null) {
			addPrompt(ettellPrefix + ending.displayText);
		}	
		MiniGameController.endMiniGame(ending.edgeId);
	}
}