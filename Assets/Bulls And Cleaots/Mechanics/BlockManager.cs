using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BullsAndCleots.Mechanics {

public class BlockManager : MonoBehaviour {
 
    
	public GameObject blockPrefab;
	public Transform startTransform;
	public GameObject bounds; 
		
	private List<GameObject> blocks;
	// For some reason how I setup the scene makes the textures upside down
	// so flip them.
	private static Vector3 orientation = new Vector3(0, 0, 180);
	private	List<Material[]> choices = null;
    
	public List<List<Material>> Choices {
		set {
			if (choices != null){
				choices.Clear();
			}
			choices = new List<Material[]>();
				
			foreach(List<Material> mats in value){
				choices.Add (mats.ToArray());			
			}
		}
	}
 

	void layoutRow(int rowIndex, Rect bnds, float blockHeight, List<GameObject> row, int totalRows){
		int rowCount = row.Count;
		
		// Should be a constant for a given row.
		float z = ((bnds.height) / (totalRows+ 1)) * rowIndex + bnds.yMax ;
		int index = 0;
		foreach(GameObject block in row){
			float x = ((bnds.width - blockHeight) / rowCount) * index + bnds.xMax;
				
			block.transform.position = new Vector3(x, startTransform.position.y, z);
			index++;
		}
	}
     
    
	// Use this for initialization
	void Start() {
			
		bounds = Instantiate(bounds) as GameObject;
		Rect layoutRect = bounds.GetComponent<BoundingBox>().Bounds;
			
		blocks = new List<GameObject>();
		int rowIndex = 0;
		foreach (Material[] mats in choices) {
			List<GameObject> row = new List<GameObject>();
			foreach(Material mat in mats){
					
				GameObject newBlock = Instantiate(blockPrefab) as GameObject;
				newBlock.transform.parent = gameObject.transform;
            
				GameBlock solutionBlock = newBlock.GetComponent<GameBlock>();
				newBlock.transform.parent = gameObject.transform;

				newBlock.renderer.material = mat;
				solutionBlock.data = mat;
				blocks.Add(newBlock);
            	row.Add(newBlock);
				}
					
			layoutRow(rowIndex, layoutRect, 1.0f, row, choices.Count);
			rowIndex++;
			row.Clear();
		}
        
		blocks.ForEach(b => b.transform.Rotate(orientation));
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