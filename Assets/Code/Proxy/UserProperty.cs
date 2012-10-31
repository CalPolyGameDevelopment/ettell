using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class UserProperty : WritableXml {
	public XmlLoader defaultUserState;
	
	private static UserProperty singleton;
	
	public override void Start() {
		singleton = this;
		if (PlayerPrefs.GetString(resourceName) == "") {
			MemoryStream ms = new MemoryStream();
			defaultUserState.xmlDoc.Save(ms);
			PlayerPrefs.SetString(resourceName, enc.GetString(ms.GetBuffer()));
			ms.Close();
		}
		base.Start();
	}
	
	public static string getProp(string propName) {
		try {
			return singleton.getDatum<string>(string.Format("descendant::{0}", propName), XmlUtilities.getData);
		}
		catch {
			return singleton.defaultUserState.getDatum<string>(string.Format("descendant::{0}", propName), XmlUtilities.getData);
		}
	}
	
	public static void setProp(string propName, string val) {
		XmlNode xn = singleton.xmlDoc.CreateNode(XmlNodeType.Element, propName, "");
		XmlAttribute xa = singleton.xmlDoc.CreateAttribute(XmlUtilities.data);
		xa.Value = val;
		xn.Attributes.Append(xa);
		singleton.save();
	}
}