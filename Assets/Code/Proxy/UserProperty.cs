using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;

public class UserProperty : WritableXml {
	public XmlLoader defaultUserState;
	private static UserProperty singleton;
	public bool alwaysWipe;
	
	public override void Start() {
		singleton = this;
		//base.Start();
		StartCoroutine(startInner());
	}
	
	private IEnumerator startInner() {
		while (!defaultUserState.xmlAvailable) {
			yield return 0;
		}
		if (alwaysWipe || PlayerPrefs.GetString(resourceName) == "") {
			Wipe();
		}
		init();
	}
	
	public static void Wipe() {
		MemoryStream ms = new MemoryStream();
		singleton.defaultUserState.xmlDoc.Save(ms);
		PlayerPrefs.SetString(singleton.resourceName, enc.GetString(ms.GetBuffer()));
		ms.Close();
	}
	
	public static void ForceReload() {
		singleton.forceReload();
	}
	
	public static bool ready {
		get {
			return singleton != null && singleton.xmlAvailable;
		}
	}
	
	public static string getProp(string propName) {
		return GetPropNode(propName).getString();
	}


	/// <summary>
	/// Gets the XML node of the property instead of string stored in the
	/// data attribute.
	/// </summary>
	public static XmlNode GetPropNode(string propName) {
		XmlNode propNode = singleton.xmlDoc.DocumentElement.childNode(propName);
		
		if (propNode != null) {
			return propNode;
		}
        
		return singleton.defaultUserState.xmlDoc.DocumentElement.childNode(propName);
	}

	public static void setProp(string propName, string val) {
		XmlNode root = singleton.xmlDoc.DocumentElement;
		foreach (XmlNode toRemove in root.childNodes(propName).ToArray()) {
			root.RemoveChild(toRemove);
		}
		root.CreateChild(propName).SetAttribute(XmlUtilities.DATA, val);
		singleton.save();
	}
	
	public static XmlNode AddProp(string propName) {
		XmlNode xn = singleton.xmlDoc.DocumentElement.CreateChild(propName);
		singleton.save();
		return xn;
	}
	
	public static void Save() {
		singleton.save();
	}
}