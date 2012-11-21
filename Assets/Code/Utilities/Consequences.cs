using UnityEngine;
using System.Collections;
using System.Xml;

public class Consequences {
	
	private const string CHANGE = "change";
	private const string ADD = "add";
	private const string MULTIPLY = "multiply";
	private const string LEARN_NAME = "learnName";
	private const string ADD_STRING = "addString";
	private const string STRING_TARGET = "stringTarget";
	
	public static void apply(XmlNode consequence) {
		foreach (XmlNode change in consequence.SelectNodes(CHANGE)) {
			string changeProp = change.getString();
			foreach (XmlNode add in change.SelectNodes(ADD)) {
				int delta = MathData.GetInt(add);
				UserProperty.setProp(changeProp,
					(UserProperty.GetPropNode(changeProp).GetInt() + delta).ToString()
				);
			}
			foreach (XmlNode mult in change.SelectNodes(MULTIPLY)) {
				int factor = MathData.GetInt(mult);
				UserProperty.setProp(changeProp,
					(UserProperty.GetPropNode(changeProp).GetInt() * factor).ToString()
				);
			}
		}
		foreach (XmlNode newName in consequence.SelectNodes(LEARN_NAME)) {
			//TODO
		}
		foreach (XmlNode addLines in consequence.SelectNodes(ADD_STRING)) {
			XmlNode dialog = UserProperty.GetPropNode(addLines.childNode(STRING_TARGET).getString());
			foreach (string line in addLines.getStrings()) {
				dialog.CreateStringNode().SetString(line);
			}
		}
	}
}