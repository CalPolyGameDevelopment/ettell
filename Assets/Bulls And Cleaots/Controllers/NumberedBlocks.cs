using UnityEngine;
using System.Collections;

public class NumberedBlocks : MonoBehaviour {

    public void Reset() {
        Draggable[] blocks = GetComponentsInChildren<Draggable>();
        foreach (Draggable block in blocks) {
            block.Reset();
        }
    }
}
