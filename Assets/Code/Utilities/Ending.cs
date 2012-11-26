using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Ending {
	
	private const string ENDING = "ending";
	private const string DIFFICULTY = "difficulty";
	private const string DISPLAY_TEXT = "display_text";
	private XmlNode data;
	
	public Color color {
		get {
			return MaterialData.GetColor(data);
		}
	}
	
	public float difficulty {
		get {
			return MathData.GetFloat(data.SelectSingleNode(DIFFICULTY));
		}
	}
	
	public string displayText {
		get {
			return data.SelectSingleNode(DISPLAY_TEXT).getString();
		}
	}
	
	public string edgeId {
		get {
			return data.getString();
		}
	}
	
	public bool displayKey {
		get {
			Color? c;
			try {
				c = color;
			}
			catch {
				return false;
			}
			return data.childNode(DISPLAY_TEXT) != null && c != null;
		}
	}
	
	private Ending(XmlNode xn) {
		data = xn;
	}

	public XmlNode otherData(string tagName) {
		return data.SelectSingleNode(tagName);
	}

	public static IEnumerable<Ending> findEndings(XmlNode xn) {
		return xn.childNodes(ENDING).Where(Requirements.passRequirements)
            .Select<XmlNode, Ending>(x => new Ending(x));
	}
}