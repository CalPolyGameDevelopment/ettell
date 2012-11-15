using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;

public class XmlUtilities : MonoBehaviour {
	
	public const string data = "data";
	public const string edge = "edge";
	public const string consequence = "consequence";
	public const string atLeast = "atLeast";
	public const string change = "change";
	public const string add = "add";
	public const string multiply = "multiply";
	public const string sceneName = "sceneName";
	public const string mirror = "mirror";
	public const string splitter = "splitter";
	public const string source = "source";
	public const string sink = "sink";
	public const string position = "position";
	public const string width = "width";
	public const string height = "height";
	public const string solution = "solution";
    public const string number = "number";
	public const string duration = "duration";
	public const string resource = "resource";
	public const string endEdge = "endEdge";
    
    public static readonly Dictionary<string,Color> colorNameToValueMap = 
        new Dictionary<string, Color>{
            {"red" , Color.red},
            {"green",Color.green},
            {"blue", Color.blue},
            {"black", Color.black},
            {"cyan", Color.cyan},
            {"magenta", Color.magenta},
            {"white", Color.white},
            {"yellow", Color.yellow},
    };
    
	private delegate string replace();
	
	private static Dictionary<Regex, replace> replacers;
    
	void Start() {
		replacers = new Dictionary<Regex, replace>();
		replacers[new Regex("\\\\year")] = Year;
	}
	
	public static string getData(XmlNode xn) {
		string val = xn.Attributes[data].Value;
		if (val.Contains("\\")) {
			foreach (Regex replacement in replacers.Keys) {
				val = replacement.Replace(val, replacers[replacement]());
			}
		}
		return val;
	}
	
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
	
	private string Year() {
		return UserProperty.getProp("year");
	}
}