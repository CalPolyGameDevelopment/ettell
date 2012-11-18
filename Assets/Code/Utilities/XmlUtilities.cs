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
	
	private static Regex userPropReplace;
    
	void Start() {
		userPropReplace = new Regex(@"\\(\w*)");
	}
	
	private static string matchEvaluator(Match m) {
		return UserProperty.getProp(m.Groups[1].Value);
	}
	
	public static string getData(XmlNode xn) {
		string val = xn.Attributes[DATA].Value;
		val = userPropReplace.Replace(val, matchEvaluator);
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
}