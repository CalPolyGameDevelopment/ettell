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
	
	public static XmlNode CreateChild(this XmlNode xn, string tagName) {
		XmlNode x = xn.OwnerDocument.CreateNode(XmlNodeType.Element, tagName, "");
		xn.AppendChild(x);
		return x;
	}
	
	public static void SetAttribute(this XmlNode xn, string attribute, string val) {
		XmlAttribute xa = xn.OwnerDocument.CreateAttribute(attribute);
		xa.Value = val;
		xn.Attributes.Append(xa);
	}
}