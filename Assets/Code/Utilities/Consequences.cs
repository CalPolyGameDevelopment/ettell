using UnityEngine;
using System.Collections;
using System.Xml;

public class Consequences {
	
	public static void apply(XmlNode consequence) {
		foreach (XmlNode change in consequence.SelectNodes(XmlUtilities.change)) {
			string changeProp = XmlUtilities.getData(change);
			foreach (XmlNode add in change.SelectNodes(XmlUtilities.add)) {
				int delta = int.Parse(XmlUtilities.getData(add));
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) + delta).ToString()
				);
			}
			foreach (XmlNode mult in change.SelectNodes(XmlUtilities.multiply)) {
				int factor = int.Parse(XmlUtilities.getData(mult));
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) * factor).ToString()
				);
			}
		}
	}
}