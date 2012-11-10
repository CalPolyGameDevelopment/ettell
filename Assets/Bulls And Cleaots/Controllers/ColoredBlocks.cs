using UnityEngine;
using System.Collections;

public class ColoredBlocks : MonoBehaviour {

    public Color[] blockColors;
    public GameObject coloredBlock;

    private GameObject[] blocks;
    private Vector3 spacingVector = new Vector3(1.2f, 0.0f, 0.0f);

    // Use this for initialization
    void Start() {


        int numColors = blockColors.GetLength(0);
        blocks = new GameObject[numColors];


        for (int i = 0; i < numColors; i++) {
            Color newColor = blockColors[i];
            GameObject newBlock = Instantiate(coloredBlock) as GameObject;
            newBlock.renderer.material.color = newColor;

            newBlock.transform.parent = gameObject.transform;



            newBlock.transform.Translate(spacingVector * i);



            blocks[i] = newBlock;

        }



    }


    public void Reset() {
        Draggable[] blocks = GetComponentsInChildren<Draggable>();
        foreach (Draggable block in blocks) {
            block.Reset();
        }
    }

}
