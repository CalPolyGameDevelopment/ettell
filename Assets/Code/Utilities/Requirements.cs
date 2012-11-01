using UnityEngine;
using System.Collections;
using System.Xml;

public class Requirements {
	
	public static bool pass(XmlNode test) {
		if (test.Attributes[XmlUtilities.requires] == null) {
			return true;
		}
		if (!StoryController.Ready) {
			return false;
		}
		XmlNode req = StoryController.findById(test.Attributes[XmlUtilities.requires].Value);
		foreach (XmlNode haveReq in req.SelectNodes(XmlUtilities.have)) {
			if (!checkHave(haveReq)) {
				return false;
			}
		}
		return true;
	}
	
	private static bool checkHave(XmlNode haveReq) {
		int haveQuantity = int.Parse(UserProperty.getProp(haveReq.Attributes[XmlUtilities.data].Value));
		foreach (XmlNode atLeast in haveReq.SelectNodes(XmlUtilities.atLeast)) {
			if (int.Parse(atLeast.Attributes[XmlUtilities.data].Value) > haveQuantity) {
				return false;
			}
		}
		return true;
	}
}