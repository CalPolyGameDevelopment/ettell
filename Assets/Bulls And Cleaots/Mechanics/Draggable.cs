using UnityEngine;
using System.Collections;
using System;

public class DraggableOnSnapEvent : IEvent
{
    private string name;
    GameObject dragObject = null;
    
    public DraggableOnSnapEvent (GameObject draggable)
    {
        name = this.GetType ().ToString ();
        dragObject = draggable;
    }
    
    string IEvent.GetName ()
    {
        return name;
    }

    object IEvent.GetData ()
    {
        return dragObject;
    }   
}

public class DraggableOnMoveEvent : IEvent{
    private string name;
    GameObject dragObject = null;
    
    public DraggableOnMoveEvent(GameObject draggable){
        name = this.GetType().ToString();
        dragObject = draggable;
    }
    
    string IEvent.GetName ()
    {
        return name;
    }

    object IEvent.GetData ()
    {
        return dragObject;
    }
}

/// <summary>
/// Dragable.
/// Adapted from: http://wiki.unity3d.com/index.php?title=DragObject
/// 
/// The major issues with this are:
/// 
/// 1. If in the case that a Dragable 
///  has not COMPLETLY exited a Snappable area before intering another
///  one it does not know what to do.
/// 
/// 2. Dragables handling collisions with other dragables.
///    There is no support for this yet.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour
{
    
  
    

    public float addHeightWhenClicked = 0.0f;
    public Camera cam;
    public GameObject currentSnappable;
    
    private Rigidbody myRigidbody ;
    private Transform myTransform;
    private float yPos;
    private Transform camTransform ;
    private bool isMoving;
    private uint snappableCount;
 
    private Vector3 startPosition;
    
    bool inSnappable {
        get {
            return snappableCount > 0;
        }
    }
    
    void Start ()
    {
     
        isMoving = false;
        snappableCount = 0;
        
        myRigidbody = rigidbody;
        myTransform = transform;
        startPosition = myTransform.position;
        
        if (!cam) {
            cam = Camera.main;
        }
        if (!cam) {
            Debug.LogError ("Can't find camera tagged MainCamera");
            return;
        }
 
        camTransform = cam.transform;
    }

    
    
    /// <summary>
    /// Catches the trigger when a Draggable enters a Snappable.
    /// </summary>
    void OnTriggerEnter (Collider other)
    {
  
    
        Snappable snappable = 
                other.gameObject.GetComponent<Snappable> ();
        
        
        // Ignore extraneous collisions with non snappables.
        if (snappable == null)
            return;
   
        
        if (isMoving) {
            // Free and moving into a snappable
            currentSnappable = snappable.gameObject;
            snappableCount++;
        } else {
            
            // OMGWTFBBQ catch all.
            Debug.LogWarning ("Encountered unexpected state.");   
        }
    
          

    }

    
    /// <summary>
    /// Catches the trigger event when a draggable exits a snapable.
    /// </summary>
    void OnTriggerExit (Collider other)
    {
          
        Snappable snappable = 
                other.gameObject.GetComponent (typeof(Snappable)) as Snappable;
        
 
        // Ignore extraneous collisions and collisions while 
        // moving.
        if (snappable == null)
            return;
        
        if (inSnappable) {
            snappableCount--;
        } else {
            Debug.LogWarning ("Encountered unexpected state.");   
        }
        
    }
 
    void OnMouseDown ()
    {

        myTransform.Translate (Vector3.up * addHeightWhenClicked);
        yPos = myTransform.position.y;
 
        if (!isMoving) {
            isMoving = true;
            EventManager.instance.RelayEvent(new DraggableOnMoveEvent(this.gameObject));
        } else {
            Debug.LogWarning ("Encountered unexpected state.");     
        }
    }
    
    void OnMouseUp ()
    {

       

        Vector3 pos = myTransform.position;
        pos.y = yPos - addHeightWhenClicked;
        myTransform.position = pos;


        if (isMoving) {
            isMoving = false;
        } else {
            Debug.LogWarning ("Encountered unexpected state.");
        }
        
        if (inSnappable) {
            moveSnap ();
        }

        
    }
 
    void moveSnap ()
    {
        Vector3 snapPosition = currentSnappable.transform.position;
        
        
        snapPosition.y = gameObject.transform.position.y;
        myRigidbody.MovePosition (snapPosition);
        EventManager.instance.RelayEvent (new DraggableOnSnapEvent (gameObject));
       
    }
    
    public void Reset(){
        myRigidbody.MovePosition(startPosition);
        
    }
    
    void moveNormal ()
    {
        Vector3 pos = myTransform.position;
        pos.y = yPos;
  
        myTransform.position = pos;

        
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = cam.ScreenToWorldPoint (
                new Vector3 (mousePos.x, mousePos.y, 
                    camTransform.position.y - myTransform.position.y)) - myTransform.position;
        move.y = 0.0f;
        
 
        myRigidbody.MovePosition (myRigidbody.position + move);
    }
 
    public void FixedUpdate ()
    {
        if (isMoving) {
            moveNormal ();
        }
    
    }
}
