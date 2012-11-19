using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

public static class StringData {
	
	private const string STRING = "string";
	
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
		return xn.childNodes(STRING).Select<XmlNode, string>(getString).ToArray();
	}
}
