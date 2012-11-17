using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StockController : MonoBehaviour {
	
	private const int stockPlaceTries = 10;
	
	public float spawnTimer;
	public int startStocks;
	public int maxStocks;
	public float badStockDiffusionChance;
	public List<string> symbolsNames;
	
	private struct Symbol {
		public int x;
		public int y;
		public string name;
		public float changePct;
	}
	
	private List<Symbol> symbols;
	private float t;
	
	void Start() {
		symbols = new List<Symbol>();
		t = spawnTimer * startStocks;
	}
	
	void checkSymbols() {
		foreach (Symbol s in symbols) {
			if (SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.BadStock && Random.Range(0f, 1f) < badStockDiffusionChance) {
				SnakeGame.Singleton[s.x, s.y] = SnakeGame.EMPTY_COLOR;
			}
		}
		// This will remove symbols that have been eaten or diffused from the ticker
		symbols = symbols.Where(s => 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.BadStock || 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.GoodStock
		).OrderBy<Symbol, float>(s => s.changePct).ToList();
	}
	
	void spawnSymbol() {
		Symbol s = new Symbol();
		int x = 0, y = 0;
		for (int i = 0; i < stockPlaceTries; i++) {
			x = Random.Range(1, SnakeGame.Width - 1);
			y = Random.Range(1, SnakeGame.Height - 1);
			if (SnakeGame.Singleton[x, y] == SnakeGame.EMPTY_COLOR) {
				break;
			}
			if (i == stockPlaceTries - 1) {
				Debug.Log("Couldn't place a stock");
				return;
			}
		}
		s.x = x;
		s.y = y;
		symbolsNames = symbolsNames.OrderBy<string, float>(a => Random.Range(0f, 1f)).ToList();
		s.name = symbolsNames.Where(a => !symbols.Any(b => b.name == a)).ElementAt(0);
		s.changePct = Random.Range(-5f, 5f);
		symbols.Add(s);
		SnakeGame.Singleton[s.x, s.y] = s.changePct > 0f ? SnakeGame.Singleton.GoodStock : SnakeGame.Singleton.BadStock;
	}
	
	void Update() {
		t += Time.deltaTime;
		while (t > spawnTimer) {
			t -= spawnTimer;
			checkSymbols();
			if (symbols.Count < maxStocks) {
				spawnSymbol();
			}
		}
	}
	
	void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width * 0.8f, 0f, Screen.width * 0.2f, Screen.height));
		foreach (Symbol s in symbols) {
			GUILayout.Label(s.name);
		}
		GUILayout.EndArea();
	}
}
