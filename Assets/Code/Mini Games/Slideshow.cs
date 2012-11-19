using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Slideshow : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string DURATION = "duration";
	private const string END_EDGE = "endEdge";
	
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
	
	private void startShow() {
        textures = data.childNodes(XmlUtilities.RESOURCE).Select(x => x.GetTexture()).ToArray();
        curPicture = 0;
		started = true;
		slideTime = MathData.GetFloat(data.SelectSingleNode(DURATION));
		t = 0f;
		completionEdge = data.SelectSingleNode(END_EDGE).getString();
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
