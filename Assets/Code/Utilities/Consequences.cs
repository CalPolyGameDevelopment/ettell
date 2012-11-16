using UnityEngine;
using System.Collections;
using System.Xml;

public class Consequences {
	
	private const string CHANGE = "change";
	private const string ADD = "add";
	private const string MULTIPLY = "multiply";
	
	public static void apply(XmlNode consequence) {
		foreach (XmlNode change in consequence.SelectNodes(CHANGE)) {
			string changeProp = XmlUtilities.getData(change);
			foreach (XmlNode add in change.SelectNodes(ADD)) {
				int delta = int.Parse(XmlUtilities.getData(add));
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) + delta).ToString()
				);
			}
			foreach (XmlNode mult in change.SelectNodes(MULTIPLY)) {
				int factor = int.Parse(XmlUtilities.getData(mult));
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) * factor).ToString()
				);
			}
		}
	}
}