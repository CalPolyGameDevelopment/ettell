using UnityEngine;
using System.Collections;
using System.Xml;

public class MiniGameAPI : MonoBehaviour {
	
	public interface IMiniGame {
		XmlNode Data {
			set;
		}
	}
	
	public MonoBehaviour miniGame;

	public XmlNode Data {
		set {
			(miniGame as IMiniGame).Data = value;
		}
	}
}