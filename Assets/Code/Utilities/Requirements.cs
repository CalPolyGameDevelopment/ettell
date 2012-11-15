using UnityEngine;
using System.Collections;
using System.Xml;

public class Requirements {
	
	private const string REQUIRES = "requires";
	private const string HAVE = "have";
	
	public static bool pass(XmlNode test) {
		if (test.Attributes[REQUIRES] == null) {
			return true;
		}
		if (!StoryController.Ready) {
			return false;
		}
		XmlNode req = StoryController.findById(test.Attributes[REQUIRES].Value);
		foreach (XmlNode haveReq in req.SelectNodes(HAVE)) {
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