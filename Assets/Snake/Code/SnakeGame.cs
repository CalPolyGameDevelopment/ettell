using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class SnakeGame : MonoBehaviour, MiniGameAPI.IMiniGame {
	
	private XmlNode data;
	public XmlNode Data {
		set {
			data = value;
			genBoard();
		}
	}
	
	private const string SNAKE_LENGTH_THRESHOLD = "snakeLengthThreshold";
	private const string NODES_PER_ETTELL_LERP = "nodesPerEttellLerp";
	
	private struct Square {
		public Color color;
		public SnakeTileController tile;
	}
	
	public static Color EMPTY_COLOR = Color.white;
	
	private const int INITIAL_TILE_EXTRAS = 100;
	private const int MAX_INSTANTIATE_PER_FRAME = 5;
	
	private static SnakeGame singleton;
	public static SnakeGame Singleton {
		get {
			return singleton;
		}
	}
	
	public GameObject tile;
	public GameObject ettellSnake;
	public GameObject stockController;
	
	private const string BORDER = "border";
	public Color BorderColor {
		get {
            
            return MaterialData.GetColor(data.SelectSingleNode(BORDER));
		}
	}
	
	private const string GOOD_STOCK = "goodStock";
	public Color GoodStock {
		get {
			return MaterialData.GetColor(data.SelectSingleNode(GOOD_STOCK));
		}
	}
	
	private const string BAD_STOCK = "badStock";
	public Color BadStock {
		get {
			return MaterialData.GetColor(data.SelectSingleNode(BAD_STOCK));
		}
	}
	
	
	private const string ETTELL_START = "ettellStart";
	public Color EttellStart {
		get {
			return MaterialData.GetColor(data.SelectSingleNode(ETTELL_START));;
		}
	}
	
	
	private const string ETTELL_LEFP = "ettellLerp";
	public Color[] EttellLerp {
        get {
			return data.childNode(ETTELL_LEFP).GetColors();
		}
	}
	
	private int width;
	public static int Width {
		get {
			return singleton.width;
		}
	}
	
	private int height;
	public static int Height {
		get {
			return singleton.height;
		}
	}
	
	private int winThreshold;
	public static int WinThreshold {
		get {
			return singleton.winThreshold;
		}
	}
	
	private int nodesPerLerp;
	public static int NodesPerLerp {
		get {
			return singleton.nodesPerLerp;
		}
	}
	
	private int waitingForTiles;
	private Queue<SnakeTileController> freeTiles;
	private Square[,] filledSpaces;
	private Ending[] endings;
	
	public Color this[int x, int y] {
		get {
			return filledSpaces[x, y].color;
		}
		set {
			if (filledSpaces[x, y].color == value) {
				return;
			}
			Square t = filledSpaces[x, y];
			t.color = value;
			if (t.tile != null) {
				t.tile.color = value;
				if (value == EMPTY_COLOR) {
					freeTiles.Enqueue(filledSpaces[x, y].tile);
					t.tile = null;
				}
				filledSpaces[x, y] = t;
			}
			else {
				filledSpaces[x, y] = t;
				if (value != EMPTY_COLOR) {
					colorTile(x, y);
				}
			}
		}
	}
	
	private void colorTile(int x, int y) {
		if (!freeTiles.Any()) {
			StartCoroutine(waitNewTile(x, y));
		}
		else {
			finishNewTile(x, y);
		}
	}
	
	private IEnumerator<int> waitNewTile(int x, int y) {
		waitingForTiles += 1;
		while (!freeTiles.Any()) {
			yield return 0;
		}
		finishNewTile(x, y);
	}
	
	private void finishNewTile(int x, int y) {
		Square t = filledSpaces[x, y];
		t.tile = freeTiles.Dequeue();
		Vector3 tilePos = t.tile.transform.position;
		tilePos.x = (float)x;
		tilePos.y = (float)y;
		t.tile.transform.position = tilePos;
		t.tile.color = filledSpaces[x, y].color;
		filledSpaces[x, y] = t;
	}
	
	private void genBoard() {
		singleton = this;
		
		endings = Ending.findEndings(data).OrderBy<Ending, float>(e => e.difficulty).ToArray();
		if (endings.Length != 2) {
			Debug.Log("This game is designed to support 2 endings (in order of ascending difficulty): lose and stockCycle");
		}
		winThreshold = MathData.GetInt(endings[1].otherData(SNAKE_LENGTH_THRESHOLD));
		nodesPerLerp = MathData.GetInt(data.SelectSingleNode(NODES_PER_ETTELL_LERP));
		
		width = MathData.GetInt(data.SelectSingleNode(XmlUtilities.WIDTH));
		height = MathData.GetInt(data.SelectSingleNode(XmlUtilities.HEIGHT));
		freeTiles = new Queue<SnakeTileController>();
		
		spawnTiles(width * 2 + height * 2 + INITIAL_TILE_EXTRAS - 4);
		
		filledSpaces = new Square[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				filledSpaces[x, y] = new Square();
				this[x, y] = (x == 0 || x == width - 1 || y == 0 || y == height - 1) ? BorderColor : EMPTY_COLOR;
			}
		}
		
		GameObject ettell = Instantiate(ettellSnake) as GameObject;
		ettell.transform.parent = transform;
		GameObject stockCtrl = Instantiate(stockController) as GameObject;
		stockCtrl.transform.parent = transform;
	}
	
	private void spawnTiles(int num) {
		for (; num > 0; --num) {
			SnakeTileController spawn = (Instantiate(tile) as GameObject).GetComponent<SnakeTileController>();
			spawn.color = EMPTY_COLOR;
			spawn.transform.parent = transform;
			freeTiles.Enqueue(spawn);
		}
	}
	
	void Update() {
		if (waitingForTiles > 0) {
			int toSpawn = Mathf.Min(waitingForTiles, MAX_INSTANTIATE_PER_FRAME);
			spawnTiles(toSpawn);
			waitingForTiles -= toSpawn;
		}
	}
	
	public static void ettellLose() {
		if (MiniGameController.Current == singleton.gameObject) {
			MiniGameController.endMiniGame(singleton.endings[0].edgeId);
		}
	}
	
	public static void cycleEnd() {
		if (MiniGameController.Current == singleton.gameObject) {
			MiniGameController.endMiniGame(singleton.endings[1].edgeId);
		}
	}
}