using UnityEngine;
using System.Collections;
using System.Xml;

public static class BoolData {
	public static bool getBool(this XmlNode xn) {
		return bool.Parse(xn.getString());
	}
}
