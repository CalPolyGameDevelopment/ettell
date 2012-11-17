using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;

public class XmlUtilities : MonoBehaviour {
	
	//Shared xml tags
	public const string DATA = "data";
	public const string RESOURCE = "resource";
	public const string WIDTH = "width";
	public const string HEIGHT = "height";
    
   
    
	private delegate string replace(string data);
	
	private static Dictionary<Regex, replace> replacers;
    
	void Start() {
		replacers = new Dictionary<Regex, replace>();
		replacers[new Regex("\\\\year")] = Year;
	}
	
	public static string getData(XmlNode xn) {
		string val = xn.Attributes[DATA].Value;
		if (val.Contains("\\")) {
			foreach (Regex replacement in replacers.Keys) {
				val = replacement.Replace(val, replacers[replacement](val));
			}
		}
		return val;
	}
	
    public static bool GetFloat(XmlNode xn, out float data){
        data = 0.0f;
        string val = xn.Attributes[DATA].Value;
        return float.TryParse(val, out data);
    }
    
    public static bool GetInt(XmlNode xn, out int data){
        data = 0;
        string val = xn.Attributes[DATA].Value;
        return int.TryParse(val, out data);
    }
    
    public static bool GetColor(XmlNode xn, out Color data){
        data = Color.white;
        string val = xn.Attributes[DATA].Value;
        return ColorUtilities.TryParse(val, out data);
    }
    // Candidates 
    // Vector3 à la getPosition
    
	public static float[] getPosition(XmlNode position) {
		return getData(position).Split(',').Select(x => float.Parse(x)).ToArray();
	}
	
	public static IEnumerable<T> getDataFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNodeList xnl = xDoc.SelectNodes(xPath);
		return xnl.Cast<XmlNode>().Select<XmlNode, T>(f);
	}
	
	public static T getDatumFromNode<T>(XmlNode xDoc, string xPath, System.Func<XmlNode, T> f) {
		XmlNode xn = xDoc.SelectSingleNode(xPath);
		return f(xn);
	}
	
	private string Year(string data) {
		return UserProperty.getProp("year");
	}
}