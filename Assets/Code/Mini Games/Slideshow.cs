using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Slideshow : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			startShow();
		}
	}
	
	private bool started;
	private int curPicture;
	private float slideTime;
	private float t;
	private string completionEdge;
	private Texture[] textures;
	
	private static Texture getTexture(XmlNode resource) {
		return (Texture)Resources.Load(XmlUtilities.getData(resource));
	}
	
	private void startShow() {
		textures = XmlUtilities.getDataFromNode<Texture>(data, XmlUtilities.resource, getTexture).ToArray();
		curPicture = 0;
		started = true;
		slideTime = float.Parse(XmlUtilities.getData(data.SelectSingleNode(XmlUtilities.duration)));
		t = 0f;
		completionEdge = XmlUtilities.getData(data.SelectSingleNode(XmlUtilities.endEdge));
	}
	
	void OnGUI() {
		if (!started) {
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), textures[curPicture]);
	}
	
	void Update() {
		if ((t += Time.deltaTime) > slideTime) {
			curPicture ++;
			t -= slideTime;
		}
		if (curPicture >= textures.Length) {
			started = false;
			MiniGameController.endMiniGame(completionEdge);
		}
	}
}
