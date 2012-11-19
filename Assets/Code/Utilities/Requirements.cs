using UnityEngine;
using System.Collections;
using System.Xml;

public class Requirements {
	
	private const string REQUIRES = "requires";
	private const string HAVE = "have";
	private const string AT_LEAST = "atLeast";
	private const string EXACTLY = "exactly";
	
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
		int haveQuantity = int.Parse(UserProperty.getProp(haveReq.getString()));
		foreach (XmlNode atLeast in haveReq.SelectNodes(AT_LEAST)) {
			if (MathData.GetInt(atLeast) > haveQuantity) {
				return false;
			}
		}
		foreach (XmlNode exactly in haveReq.SelectNodes(EXACTLY)) {
			if (MathData.GetInt(exactly) != haveQuantity) {
				return false;
			}
		}
		return true;
	}
}