using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class NumberedBlocks : MonoBehaviour {


    public Texture[] blockTextures;
    public GameObject blockPrefab;


    public Transform startTransform;

    private float layoutSpacing = 1.2f;

    private List<GameObject> blocks;
 
    private int[] possibleNumbers;
    private int numberCount;
    
    public int[] PossibleNumbers  {
        set{
            numberCount = value.GetLength(0);
            possibleNumbers = new int[numberCount];
            Array.Copy(value, possibleNumbers, numberCount);
        }
    }

    // Use this for initialization
    void Start() {

        Vector3 startPos = startTransform.position;

        Func<int, Vector3> LayoutObject = c => new Vector3(
            startPos.x + (c * layoutSpacing), startPos.y, startPos.z);




        blocks = new List<GameObject>();


        for (int index = 0; index < numberCount; index++) {
            Texture newTexture = blockTextures[index];
            int digit = possibleNumbers[index];
            
            GameObject newBlock = Instantiate(blockPrefab) as GameObject;
			newBlock.transform.parent = transform;
            SolutionBlock solutionBlock = newBlock.GetComponent<SolutionBlock>();

            newBlock.renderer.material.mainTexture = newTexture;
            //newBlock.transform.parent = gameObject.transform;
            newBlock.transform.position = LayoutObject(index);
            newBlock.transform.rotation = startTransform.rotation;

            solutionBlock.data = digit;


            blocks.Add(newBlock); 

        }



    }


    public void Reset() {
        IEnumerable<Draggable> draggables = Enumerable.Select<GameObject, Draggable>(
            blocks, b => b.GetComponent<Draggable>());

        foreach (Draggable d in draggables) {
            d.Reset();
        }

    }

   
}


