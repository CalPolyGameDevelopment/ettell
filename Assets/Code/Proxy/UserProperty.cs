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
		StartCoroutine(startInner());
	}
	
	private IEnumerator startInner() {
		while (!defaultUserState.xmlAvailable) {
			yield return 0;
		}
		if (alwaysWipe || PlayerPrefs.GetString(resourceName) == "") {
			MemoryStream ms = new MemoryStream();
			defaultUserState.xmlDoc.Save(ms);
			PlayerPrefs.SetString(resourceName, enc.GetString(ms.GetBuffer()));
			ms.Close();
		}
		base.Start();
	}
	
	public static bool ready {
		get {
			return singleton != null && singleton.xmlAvailable;
		}
	}
	
	public static string getProp(string propName) {
		return GetPropNode(propName).getString();
	}

    public static XmlNode GetPropNode(string propName){
        XmlNode propNode = singleton.xmlDoc.DocumentElement.childNode(propName);
		
        if (propNode != null){
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