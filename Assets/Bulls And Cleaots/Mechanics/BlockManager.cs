using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BullsAndCleots.Mechanics {

public class BlockManager : MonoBehaviour {
 
    
	public GameObject blockPrefab;
	public Transform startTransform;
	private List<GameObject> blocks;
	private float layoutSpacing = 1.2f;
	// For some reason how I setup the scene makes the textures upside down
	// so flip them.
	private static Vector3 orientation = new Vector3(0, 0, 180);
	private Material[] choices;
	private int count;
    
	public Material[] Choices {
		set {
			count = value.GetLength(0);
			choices = new Material[count];
			Array.Copy(value, choices, count);
		}
	}
 
     
    
	// Use this for initialization
	void Start() {

		Vector3 pos = startTransform.position;

		Func<Vector3, Vector3> LayoutObject = oldPos => new Vector3(
            oldPos.x + layoutSpacing, oldPos.y, oldPos.z);

		blocks = new List<GameObject>();
        
		foreach (Material mat in choices) {
			GameObject newBlock = Instantiate(blockPrefab) as GameObject;
			newBlock.transform.parent = gameObject.transform;
            
			GameBlock solutionBlock = newBlock.GetComponent<GameBlock>();
			newBlock.transform.parent = gameObject.transform;
			newBlock.transform.position = LayoutObject(pos);
			newBlock.transform.Rotate(orientation);

			newBlock.renderer.material = mat;
            
			solutionBlock.data = mat;
			blocks.Add(newBlock);
            
			pos = newBlock.transform.position;
		}


        

	}

	public void Reset() {
		IEnumerable<Draggable> draggables = Enumerable.Select<GameObject,Draggable>(
            blocks, b => b.GetComponent<Draggable>());

		foreach (Draggable d in draggables) {
			d.Reset();
		}

	}

}
	
}