using UnityEngine;
using System.Collections;

public class SolutionSnapArea : MonoBehaviour {

    private int index = -1;

    public int Index {
        get {
            return index;
        }
        set {
            if (index == -1 && value >= 0)
                index = value;
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}