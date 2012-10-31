using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml;

public class XmlUtilities {
	
	public const string datasource = "datasource";
	public const string data = "data";
	public const string user = "user";
	public const string format = "format";
	
	public static string getData(XmlNode xn) {
		if (xn.Attributes[datasource] == null) {
			return xn.Attributes[data].Value;
		}
		switch (xn.Attributes[datasource].Value) {
		case user:
			return UserProperty.getProp(xn.Attributes[data].Value);
		case format:
			XmlNodeList xnl = xn.SelectNodes(format);
			return string.Format(
				xn.Attributes[data].Value,
				xnl.Cast<XmlNode>().Select<XmlNode, string>(getData).ToArray()
			);
		default:
			throw new UnityException("xml has unsupported datasource: " + xn.Attributes[datasource]);
		}
	}
}