using UnityEngine;
using System.Collections;
using System.Xml;

public abstract class SceneLoadingMiniGame : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			Application.LoadLevel(XmlUtilities.getData(data.SelectSingleNode(XmlUtilities.sceneName)));
		}
	}
	
	void OnDestroy() {
		Application.LoadLevel("Empty");
	}
}