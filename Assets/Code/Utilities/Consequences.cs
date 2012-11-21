using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

public class Consequences {
	
	private const string CHANGE = "change";
	private const string ADD = "add";
	private const string MULTIPLY = "multiply";
	private const string LEARN_NAME = "learnName";
	private const string ADD_STRING = "addString";
	private const string BEFORE_CALLED = "beforeCalled";
	private const string APPEARS_IN = "appearsIn";
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
		foreach (XmlNode newNameNode in consequence.SelectNodes(LEARN_NAME)) {
			string newName = newNameNode.getString();
			foreach (string oldName in newNameNode.childNode(BEFORE_CALLED).getStrings()) {
				Regex regex = new Regex(string.Format("^({0}):", oldName));
				foreach (string setting in newNameNode.childNode(APPEARS_IN).getStrings()) {
					XmlNode uProp = UserProperty.GetPropNode(setting);
					foreach (XmlNode replaceable in uProp.GetStringNodes()) {
						replaceable.SetString(regex.Replace(replaceable.getString(), delegate(Match m) {
							return newName + ":";
						}));
					}
				}
			}
		}
		foreach (XmlNode addLines in consequence.SelectNodes(ADD_STRING)) {
			XmlNode dialog = UserProperty.GetPropNode(addLines.childNode(STRING_TARGET).getString());
			foreach (string line in addLines.getStrings()) {
				dialog.CreateStringNode().SetString(line);
			}
		}
	}
}