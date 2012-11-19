using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml;
using System;

public static class XmlUtilities {
	//Shared xml tags
	public const string DATA = "data";
	public const string RESOURCE = "resource";
	public const string WIDTH = "width";
	public const string HEIGHT = "height";
	
	public static IEnumerable<XmlNode> childNodes(this XmlNode xn, string tagName) {
		return xn.SelectNodes(tagName).Cast<XmlNode>();
	}
	
	public static XmlNode childNode(this XmlNode xn, string tagName) {
		return xn.SelectSingleNode(tagName);
	}
}