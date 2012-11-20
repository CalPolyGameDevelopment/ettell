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
		string propPath = System.String.Format("descendant::{0}", propName);
        try {
            return singleton.xmlDoc.SelectSingleNode(propPath).getString();
        }
		catch {
            return singleton.defaultUserState.xmlDoc.SelectSingleNode(propPath).getString();
        }
	}

    /// <summary>
    /// Gets the XML node of the property instead of string stored in the 
    /// data attribute.
    /// </summary>
    public static XmlNode GetPropNode(string propName){
        XmlNode propNode = singleton.xmlDoc.SelectSingleNode(propName);
        if (propNode != null){
            return propNode;
        }
        
        propNode = singleton.defaultUserState.xmlDoc.SelectSingleNode(propName);
        
        if (propNode != null){
            return propNode;
        }
            
        throw new System.ArgumentException(
            string.Format("Unable to locate a UserProp node with name: {}", propName));
    }

	public static void setProp(string propName, string val) {
		XmlNode root = singleton.xmlDoc.DocumentElement;
		foreach (XmlNode toRemove in root.childNodes(propName).ToArray()) {
			root.RemoveChild(toRemove);
		}
		root.CreateChild(propName).SetAttribute(XmlUtilities.DATA, val);
		singleton.save();
	}
}