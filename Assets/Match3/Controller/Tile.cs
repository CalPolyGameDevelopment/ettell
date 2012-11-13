using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Tile : MonoBehaviour {
	
	public static int BOARD_HEIGHT;
	
	public float fallSpeed;
	
	public Ending ending;
	public Tile up;
	public Tile down;
	public Tile left;
	public Tile right;
	
	private bool markGone;
	
	private void addIf(HashSet<Tile> border, HashSet<Tile> explored, Tile node) {
		if (node != null && node.ending.edgeId == ending.edgeId && !explored.Contains(node)) {
			border.Add(node);
		}
	}
	
	private Tile[] getMatchingNeighbors() {
		HashSet<Tile> border = new HashSet<Tile>();
		border.Add(this);
		HashSet<Tile> explored = new HashSet<Tile>();
		
		while (border.Count > 0) {
			Tile cur = border.First();
			border.Remove(cur);
			explored.Add(cur);
			
			addIf(border, explored, cur.up);
			addIf(border, explored, cur.down);
			addIf(border, explored, cur.left);
			addIf(border, explored, cur.right);
		}
		return explored.ToArray();
	}
	
	void Start() {
		renderer.material.color = ending.color;
	}
	
	void Update () {
		if (ClickRaycast.clickedMe(gameObject)) {
			Tile[] matchingNeighbors = getMatchingNeighbors();
			if (matchingNeighbors.Length >= 3) {
				Match3.recordDestruction(ending.edgeId, matchingNeighbors.Length);
				foreach(Tile t in matchingNeighbors) {
					Destroy(t.gameObject);
				}
			}
		}
		if (down != null) {
			transform.position = new Vector3(
				down.transform.position.x,
				Mathf.Max(transform.position.y - fallSpeed * Time.deltaTime, down.transform.position.y + 1f),
				transform.position.z
			);
		}
		else {
			transform.position = new Vector3(
				transform.position.x,
				Mathf.Max(transform.position.y - fallSpeed * Time.deltaTime, 0f),
				transform.position.z
			);
		}
		if (left != null) {
			transform.position = new Vector3(
				Mathf.Max(transform.position.x - fallSpeed * Time.deltaTime, left.transform.position.x + 1f),
				transform.position.y,
				transform.position.z
			);
		}
	}
	
	void ShiftUpSideNeighbors() {
		if (up != null) {
			up.ShiftUpSideNeighbors();
			up.left = left;
			if (left != null) {
				left.right = up;
			}
			up.right = right;
			if (right != null) {
				right.left = up;
			}
		}
		else {
			if (left) {
				left.right = null;
			}
			if (right) {
				right.left = null;
			}
		}
	}
	
	void OnDestroy() {
		ShiftUpSideNeighbors();
		if (up != null) {
			up.down = down;
		}
		if (down != null) {
			down.up = up;
		}
		if (down == null && up == null) {
			Tile curLeft = left;
			Tile curRight = right;
			while (curLeft != null && curRight != null) {
				curLeft.right = curRight;
				curRight.left = curLeft;
				curLeft = curLeft.up;
				curRight = curRight.up;
			}
		}
	}
}