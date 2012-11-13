using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class Ending {
	
	private const string ENDING = "ending";
	private const string DIFFICULTY = "difficulty";
	private const string COLOR = "color";
	
	private XmlNode data;
	
	public Color color {
		get {
			string[] hexes = XmlUtilities.getDatumFromNode<string>(data, COLOR, XmlUtilities.getData).Split(',');
			return new Color(
				float.Parse(hexes[0]) / 255f,
				float.Parse(hexes[1]) / 255f,
				float.Parse(hexes[2]) / 255f
			);
		}
	}
	
	public float difficulty {
		get {
			return float.Parse(XmlUtilities.getDatumFromNode<string>(data, DIFFICULTY, XmlUtilities.getData));
		}
	}
	
	public string edgeId {
		get {
			return XmlUtilities.getData(data);
		}
	}
	
	public Ending (XmlNode xn) {
		data = xn;
	}
	
	public static IEnumerable<Ending> findEndings (XmlNode xn) {
		return XmlUtilities.getDataFromNode<Ending>(xn, ENDING, x => new Ending(x));
	}
}
