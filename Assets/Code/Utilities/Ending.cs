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

            return MaterialData.GetColor(
                data.SelectSingleNode(ColorUtilities.COLOR));
		}
	}
	
	public float difficulty {
		get {
			return MathData.GetFloat(data.SelectSingleNode(DIFFICULTY));
		}
	}
	
	public string displayText {
		get {
			return XmlUtilities.getData(data.SelectSingleNode(DISPLAY_TEXT));
		}
	}
	
	public string edgeId {
		get {
			return XmlUtilities.getData(data);
		}
	}
	
	private Ending(XmlNode xn) {
		data = xn;
    }
	

	public XmlNode otherData (string tagName) {
		return data.SelectSingleNode(tagName);
	}

	public static IEnumerable<Ending> findEndings (XmlNode xn) {
        XmlNodeList nodes = xn.SelectNodes(ENDING);
        
        
        var availableEndings = 
            from n in nodes.Cast<XmlNode>()
            where Requirements.pass(n)
            select new Ending(n);
        
        
        foreach (var end in availableEndings){
            yield return (Ending)end;
        }
    }

}
