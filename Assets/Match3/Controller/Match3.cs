using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Match3 : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			genBoard();
		}
	}
	
	public GameObject tile;
	
	private static Dictionary<string, int> winReqs;
	
	public static void recordDestruction(string edge, int num) {
		winReqs[edge] -= num;
		if (winReqs[edge] <= 0) {
			string winner = "";
			foreach (string possibleEdge in winReqs.Keys) {
				if (winReqs[possibleEdge] <= 0) {
					continue;
				}
				if (winner == "") {
					winner = possibleEdge;
					continue;
				}
				return;
			}
			MiniGameController.endMiniGame(winner);
		}
	}
	
	private void genBoard() {
		winReqs = new Dictionary<string, int>();
		XmlUtilities.EdgeColor[] colors = XmlUtilities.getDataFromNode<XmlUtilities.EdgeColor>(data, XmlUtilities.color, XmlUtilities.parseColor).ToArray();
		foreach (XmlUtilities.EdgeColor color in colors) {
			winReqs[color.edgeId] = 0;
		}
		int width = int.Parse(XmlUtilities.getDatumFromNode<string>(data, XmlUtilities.width, XmlUtilities.getData));
		int height = int.Parse(XmlUtilities.getDatumFromNode<string>(data, XmlUtilities.height, XmlUtilities.getData));
		Tile.BOARD_HEIGHT = height;
		Tile[,] tiles = new Tile[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject go = Instantiate(tile) as GameObject;
				go.transform.position = new Vector3((float)x, (float)y, 0f);
				go.transform.parent = gameObject.transform;
				Tile t = go.GetComponent<Tile>();
				tiles[x, y] = t;
				t.identity = colors[Random.Range(0, colors.Length)];
				winReqs[t.identity.edgeId]++;
				
				if (x > 0) {
					tiles[x-1, y].right = t;
					t.left = tiles[x-1, y];
				}
				if (y > 0) {
					tiles[x, y-1].up = t;
					t.down = tiles[x, y-1];
				}
			}
		}
	}
}
