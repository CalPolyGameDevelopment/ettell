using UnityEngine;
using System.Collections;
using System;

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
public class Dragable : MonoBehaviour
{
    
    public enum ItemState {
        None = 0x0,
        Stopped = 0x1,
        Moving = 0x2,
        Free = 0x4,
        InSnapArea = 0x8,
        BetweenSnapAreas = 0x10,
       
    };
 
    public float addHeightWhenClicked = 0.0f;
    public bool freezeRotationOnDrag = true;
    public Camera cam;
    public GameObject currentSnapArea;
    
    private Rigidbody myRigidbody ;
    private Transform myTransform  ;
    private float yPos;
    private bool gravitySetting ;
    private bool freezeRotationSetting ;
    private Transform camTransform ;



    private ItemState dragState;
    
    
    
    void Start ()
    {
     
        dragState = ItemState.Stopped | ItemState.Free;
        
        myRigidbody = rigidbody;
        myTransform = transform;
        
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
    void OnTriggerEnter(Collider other) {
  
        Snappable snappable = 
                other.gameObject.GetComponent(typeof(Snappable)) as Snappable;
        
        
        // Ignore extraneous collisions with non snappables.
        if (snappable == null)
            return;
   
       
        if (dragState == (ItemState.Moving | ItemState.Free)) {
            // Moved into a snap area
            currentSnapArea = other.gameObject;
            
            dragState = ItemState.Moving;
            dragState |= ItemState.InSnapArea;
            return;
        } 
        else if (dragState == (ItemState.Moving | ItemState.InSnapArea)){
            // Already moving and in a Snap area and we hit another
            // trigger so we now must be between snap areas.
            currentSnapArea = other.gameObject;

            
            dragState = ItemState.Moving;
            dragState |= ItemState.BetweenSnapAreas;
            return;
        }
        else {
            // OMGWTFBBQ catch all.
            Debug.LogWarning("Encountered unexpected state.");   
        }
    
          

    }

    
    /// <summary>
    /// Catches the trigger event when a draggable exits a snapable.
    /// </summary>
    void OnTriggerExit(Collider other) {
          
        Snappable snappable = 
                other.gameObject.GetComponent(typeof(Snappable)) as Snappable;
        
 
        // Ignore extraneous collisions and collisions while 
        // moving.
        if (snappable == null)
            return;
     
        if (dragState == (ItemState.Moving | ItemState.InSnapArea)) {
            dragState = ItemState.Moving;
            dragState |= ItemState.Free;
            currentSnapArea = null;
        }
        else if (dragState == (ItemState.Moving | ItemState.BetweenSnapAreas)) {
            dragState = ItemState.Moving;
            dragState |= ItemState.InSnapArea;
        }
        else {
            Debug.LogWarning("Encountered unexpected state.");   
        }
        
    }
    
    
 
    void OnMouseDown ()
    {

        myTransform.Translate (Vector3.up * addHeightWhenClicked);
        gravitySetting = myRigidbody.useGravity;
        freezeRotationSetting = myRigidbody.freezeRotation;
        myRigidbody.useGravity = false;
        myRigidbody.freezeRotation = freezeRotationOnDrag;
        yPos = myTransform.position.y;
        
        if (dragState == (ItemState.Stopped | ItemState.Free)){
            dragState = ItemState.Moving;
            dragState |= ItemState.Free;
        }
        else if(dragState == (ItemState.Stopped | ItemState.InSnapArea)) {
            dragState = ItemState.Moving;
            dragState |= ItemState.InSnapArea;
        }
        else {
            Debug.LogWarning("Encountered unexpected state.");
        }
        
    }
 
    
    
    void OnMouseUp ()
    {

        myRigidbody.useGravity = gravitySetting;
        myRigidbody.freezeRotation = freezeRotationSetting;
        
        if (!myRigidbody.useGravity) {
            Vector3 pos = myTransform.position;
            pos.y = yPos - addHeightWhenClicked;
            myTransform.position = pos;

        }
        
        if (dragState == (ItemState.Moving | ItemState.Free)) {
            dragState = ItemState.Stopped;
            dragState |= ItemState.Free;
        }
        
        else if (dragState == (ItemState.Moving | ItemState.InSnapArea) ||
                dragState == (ItemState.Moving | ItemState.BetweenSnapAreas)){
               
            dragState = ItemState.Stopped;
            dragState |= ItemState.InSnapArea;
            moveSnap();
        }
        else {
            Debug.LogWarning("Encountered unexpected state.");
        }
        
        
        
    }
 
    void moveSnap(){
        Vector3 snapPosition = currentSnapArea.transform.position;
        
        
        snapPosition.y = gameObject.transform.position.y;
        myRigidbody.MovePosition(snapPosition);
        SendMessageUpwards("Snapped", gameObject);
    }
    
    
    void moveNormal(){
        Vector3 pos = myTransform.position;
        pos.y = yPos;
  
        myTransform.position = pos;

        
        Vector3 mousePos = Input.mousePosition;
        Vector3 move = cam.ScreenToWorldPoint(
                new Vector3 (mousePos.x, mousePos.y, 
                    camTransform.position.y - myTransform.position.y)) - myTransform.position;
        move.y = 0.0f;
        
 
        myRigidbody.MovePosition(myRigidbody.position + move);
    }
 
    public void FixedUpdate(){
        if (ItemState.Moving == (dragState & ItemState.Moving)) {
            moveNormal();
        }
    
    }
}
