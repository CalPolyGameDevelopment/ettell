using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

 
    public void Snapped(GameObject obj){
        BroadcastMessage("AddPower", obj);
    }
    
}
