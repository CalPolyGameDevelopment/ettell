using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StockController : MonoBehaviour {
	
	public float spawnTimer;
	public int startStocks;
	public int maxStocks;
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
		symbols = symbols.Where(s => 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.badStock || 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.goodStock
		).OrderBy<Symbol, float>(s => s.changePct).ToList();
	}
	
	void spawnSymbol() {
		Symbol s = new Symbol();
		s.x = Random.Range(1, SnakeGame.Width - 1);
		s.y = Random.Range(1, SnakeGame.Height - 1);
		s.name = symbolsNames[Random.Range(0, symbolsNames.Count)];
		s.changePct = Random.Range(-5f, 5f);
		symbols.Add(s);
		SnakeGame.Singleton[s.x, s.y] = s.changePct > 0f ? SnakeGame.Singleton.goodStock : SnakeGame.Singleton.badStock;
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
