using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ColoredBlocks : MonoBehaviour {


    public GameObject blockPrefab;
    public Transform startTransform;
    public static Color[] blockColors = {
        Color.red,
        Color.green,
        Color.blue,
        Color.magenta,
        Color.cyan,
        Color.yellow,
        Color.white,
        Color.black,
    };
    
    private List<GameObject> blocks; 
    private float layoutSpacing = 1.2f;
    private Color[] possibleColors;
    private int colorCount;
    
    public Color[] PossibleColors  {
        set{
            colorCount = value.GetLength(0);
            possibleColors = new Color[colorCount];
            Array.Copy(value, possibleColors, colorCount);
        }
    }

    // Use this for initialization
    void Start() {

        Vector3 startPos = startTransform.position;

        Func<int, Vector3> LayoutObject = c => new Vector3(
            startPos.x + (c * layoutSpacing), startPos.y, startPos.z);



        blocks = new List<GameObject>();
  
        for (int index = 0; index < colorCount; index++){
            Color newColor = possibleColors[index];
            GameObject newBlock = Instantiate(blockPrefab) as GameObject;
			newBlock.transform.parent = transform;
            SolutionBlock solutionBlock = newBlock.GetComponent<SolutionBlock>();

            newBlock.renderer.material.color = newColor;
            newBlock.transform.parent = gameObject.transform;
            newBlock.transform.position = LayoutObject(index);

            solutionBlock.data = newColor;

            blocks.Add(newBlock);

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
