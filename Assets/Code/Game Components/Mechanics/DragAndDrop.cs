using UnityEngine;
using UnityEditor;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
 
    public bool broadcastSnappedEvent = false;
    public string broadcastEventName; 
    
    public void Snapped (GameObject obj)
    {
        SendMessageUpwards(broadcastEventName, obj);
    }
    
}

