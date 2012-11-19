using UnityEngine;
using System.Collections;
using System.Xml;

public class Consequences {
	
	private const string CHANGE = "change";
	private const string ADD = "add";
	private const string MULTIPLY = "multiply";
	
	public static void apply(XmlNode consequence) {
		foreach (XmlNode change in consequence.SelectNodes(CHANGE)) {
			string changeProp = change.getString();
			foreach (XmlNode add in change.SelectNodes(ADD)) {
				int delta = MathData.GetInt(add);
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) + delta).ToString()
				);
			}
			foreach (XmlNode mult in change.SelectNodes(MULTIPLY)) {
				int factor = MathData.GetInt(mult);
				UserProperty.setProp(changeProp,
					(int.Parse(UserProperty.getProp(changeProp)) * factor).ToString()
				);
			}
		}
	}
}