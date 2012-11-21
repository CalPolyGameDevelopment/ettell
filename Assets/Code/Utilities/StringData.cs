using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

public static class StringData {
	
	private const string STRING = "string";
	private const string MODIFIER = "modifier";
	
	private static Regex userPropReplace = new Regex(@"\\(\w*)");
	
	private static string matchEvaluator(Match m) {
		return UserProperty.getProp(m.Groups[1].Value);
	}
	
	public static string getString(this XmlNode xn) {
		string val = xn.Attributes[XmlUtilities.DATA].Value;
		val = userPropReplace.Replace(val, matchEvaluator);
		return val;
	}
	
	public static string[] getStrings(this XmlNode xn) {
		return xn.GetStringNodes().Select<XmlNode, string>(getString).ToArray();
	}
	
	public static IEnumerable<XmlNode> GetStringNodes(this XmlNode xn) {
		return xn.childNodes(STRING);
	}
	
	public static XmlNode CreateStringNode(this XmlNode xn) {
		return xn.CreateChild(STRING);
	}
	
	public static void SetString(this XmlNode xn, string val) {
		xn.SetAttribute(XmlUtilities.DATA, val);
	}
	
	public static string GetAttribute(this XmlNode xn, string attribute) {
		return xn.Attributes[attribute] == null ? "" : xn.Attributes[attribute].Value;
	}
}
