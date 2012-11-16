using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SnakeController : MonoBehaviour {
	
	private enum Direction {
		up,
		down,
		left,
		right
	}
	
	private struct Position {
		public int x;
		public int y;
		
		public Position(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	
	private const int MAX_TAIL_LENGTH = 100;
	private const float FASTEST_TPS = 0.04f;
	
	public float timePerSquare;
	
	private Direction direction;
	private Position goingFrom;
	private Position goingTo;
	
	private float curTime;
	
	private Queue<Position> newlyFilledPositions;
	private Queue<Position> filledPositions;
	private Queue<Position> oldPositions;
	
	private int tailLength;
	
	private bool interactive;
	private Position[] plan;
	private int planIndex;
	
	public void Start() {
		direction = Direction.up;
		goingTo = new Position(SnakeGame.Width / 2, SnakeGame.Height / 2);
		goingFrom = new Position(0, 0);
		curTime = timePerSquare;
		newlyFilledPositions = new Queue<Position>();
		filledPositions = new Queue<Position>();
		interactive = true;
		tailLength = 2;
	}
	
	private void onChompSquare() {
		newlyFilledPositions.Enqueue(goingFrom);
		
		Color eating = SnakeGame.Singleton[goingFrom.x, goingFrom.y];
		Color nextMeal = SnakeGame.Singleton[goingTo.x, goingTo.y];
		if (eating == SnakeGame.EMPTY_COLOR) {
			return;
		}
		
		if (eating == SnakeGame.Singleton.BadStock) {
			if (--tailLength <= 0) {
				SnakeGame.ettellLose();
			}
		}
		else if (eating == SnakeGame.Singleton.GoodStock) {
			tailLength++;
		}
		else if (eating == SnakeGame.Singleton.BorderColor) {
			SnakeGame.ettellLose();
		}
		
		if (nextMeal == SnakeGame.Singleton.EttellStart && interactive) {
			interactive = false;
			IEnumerable<Position> filledRoute = filledPositions.Reverse().TakeWhile(p => p.x != goingTo.x || p.y != goingTo.y).Reverse();
			plan = new Position[filledRoute.Count() + newlyFilledPositions.Count + 1];
			if (plan.Length < 8) {
				SnakeGame.ettellLose();
			}
			plan[0] = goingTo;
			System.Array.Copy(filledRoute.ToArray(), 0, plan, 1, filledRoute.Count());
			System.Array.Copy(newlyFilledPositions.ToArray(), 0, plan, 1 + filledRoute.Count(), newlyFilledPositions.Count());
			planIndex = 0;
		}
	}
	
	private void setPos() {
		while (curTime >= timePerSquare) {
			curTime -= timePerSquare;
			goingFrom.x = goingTo.x;
			goingFrom.y = goingTo.y;
			if (interactive) {
				goingTo.x = goingFrom.x + (direction == Direction.right ? 1 : (direction == Direction.left ? -1 : 0));
				goingTo.y = goingFrom.y + (direction == Direction.up ? 1 : (direction == Direction.down ? -1 : 0));
			}
			else {
				planIndex = (planIndex + 1) % plan.Length;
				goingTo = plan[planIndex];
				
				timePerSquare = Mathf.Max(FASTEST_TPS, timePerSquare / 1.07f);
				if (timePerSquare <= 0.02f) {
					Debug.Log("Party Time");
				}
			}
			
			onChompSquare();
		}
		
		if (Camera.main != null) {
			Vector3 cameraPos = Camera.main.transform.position;
			Vector3 myPos = transform.position;
		
			cameraPos.x = myPos.x = Mathf.Lerp((float)goingFrom.x, (float)goingTo.x, curTime / timePerSquare);
			cameraPos.y = myPos.y = Mathf.Lerp((float)goingFrom.y, (float)goingTo.y, curTime / timePerSquare);
		
			Camera.main.transform.position = cameraPos;
			transform.position = myPos;
		}
	}
	
	private void manageTail() {
		while (newlyFilledPositions.Any()) {
			Position p = newlyFilledPositions.Dequeue();
			SnakeGame.Singleton[p.x, p.y] = SnakeGame.Singleton.EttellStart;
			filledPositions.Enqueue(p);
		}
		if (interactive) {
			while (filledPositions.Count > tailLength) {
				Position p = filledPositions.Dequeue();
				SnakeGame.Singleton[p.x, p.y] = SnakeGame.EMPTY_COLOR;
			}
		}
	}
	
	public void Update() {
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			direction = Direction.left;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			direction = Direction.right;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			direction = Direction.up;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			direction = Direction.down;
		}
		
		curTime += Time.deltaTime;
		
		setPos();
		manageTail();
	}
}