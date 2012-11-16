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
			return ColorUtilities.ColorFromXML(data);
		}
	}
	
	public float difficulty {
		get {
			return float.Parse(XmlUtilities.getDatumFromNode<string>(data, DIFFICULTY, XmlUtilities.getData));
		}
	}
	
	public string displayText {
		get {
			return XmlUtilities.getDatumFromNode<string>(data, DISPLAY_TEXT, XmlUtilities.getData);
		}
	}
	
	public string edgeId {
		get {
			return XmlUtilities.getData(data);
		}
	}
	
	public Ending(XmlNode xn) {
		data = xn;
	}
	
	public static IEnumerable<Ending> findEndings(XmlNode xn) {
		return XmlUtilities.getDataFromNode<Ending>(xn, ENDING, x => new Ending(x)).Where(x => Requirements.pass(x.data));
	}
}
