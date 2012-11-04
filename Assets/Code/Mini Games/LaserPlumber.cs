using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml;

public class LaserPlumber : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			genBoard();
		}
	}
	public GameObject source;
	public GameObject mirror;
	public GameObject splitter;
	public GameObject sink;
	
	private void setPos(GameObject toSet, XmlNode theirData) {
		XmlNode position = theirData.SelectSingleNode(XmlUtilities.position);
		if (position != null) {
			float[] coords = XmlUtilities.getPosition(position);
			toSet.transform.position = new Vector3(coords[0], coords[1], coords[2]);
		}
	}
	
	private void genBoard() {
		foreach (XmlNode sourceData in data.SelectNodes(XmlUtilities.source)) {
			GameObject cur = Instantiate(source) as GameObject;
			setPos(cur, sourceData);
		}
		foreach (XmlNode mirrorData in data.SelectNodes(XmlUtilities.mirror)) {
			GameObject cur = Instantiate(mirror) as GameObject;
			setPos(cur, mirrorData);
		}
		foreach (XmlNode sinkData in data.SelectNodes(XmlUtilities.sink)) {
			GameObject cur = Instantiate(sink) as GameObject;
			setPos(cur, sinkData);
			cur.GetComponent<Sink>().edge = XmlUtilities.getData(sinkData);
		}
		foreach (XmlNode splitterData in data.SelectNodes(XmlUtilities.splitter)) {
			GameObject cur = Instantiate(splitter) as GameObject;
			setPos(cur, splitterData);
		}
	}
}
