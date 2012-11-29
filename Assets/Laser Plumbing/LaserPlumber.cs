using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class LaserPlumber : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private const string MIRROR = "mirror";
	private const string SPLITTER = "splitter";
	private const string SOURCE = "source";
	private const string SINK = "sink";
	private const string POSITION = "position";
	
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
		XmlNode position = theirData.SelectSingleNode(POSITION);
		if (position != null) {
			toSet.transform.position = MathData.GetVector(position);
		}
		toSet.transform.parent = gameObject.transform;
	}
	
	private void genBoard() {
		int edgeIndex = 0;
		Ending[] endings = Ending.findEndings(data).OrderBy<Ending, float>(e => e.difficulty).ToArray();
		foreach (XmlNode sourceData in data.SelectNodes(SOURCE)) {
			GameObject cur = Instantiate(source) as GameObject;
			setPos(cur, sourceData);
		}
		foreach (XmlNode mirrorData in data.SelectNodes(MIRROR)) {
			GameObject cur = Instantiate(mirror) as GameObject;
			setPos(cur, mirrorData);
		}
		foreach (XmlNode sinkData in data.childNodes(SINK).OrderBy<XmlNode, int>(MathData.GetInt)) {
			GameObject cur = Instantiate(sink) as GameObject;
			setPos(cur, sinkData);
			if (edgeIndex < endings.Length) {
				cur.GetComponent<Sink>().Edge = endings[edgeIndex++];
			}
			else {
				Debug.LogWarning("Not enough endings!");
			}
		}
		foreach (XmlNode splitterData in data.SelectNodes(SPLITTER)) {
			GameObject cur = Instantiate(splitter) as GameObject;
			setPos(cur, splitterData);
		}
	}
}
