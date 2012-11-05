using UnityEngine;
using System.Collections;

/// <summary>
/// Dragable.
/// Adapted from: http://wiki.unity3d.com/index.php?title=DragObject
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Dragable : MonoBehaviour
{
 
    public int normalCollisionCount = 1;
    public float moveLimit = .5f;
    public float collisionMoveFactor = .01f;
    public float addHeightWhenClicked = 0.0f;
    public bool freezeRotationOnDrag = true;
    public Camera cam;
    
    public const string SNAP_AREA_TAG = "Snap Area";
    
    private Rigidbody myRigidbody ;
    private Transform myTransform  ;
    private bool canMove = false;
    private float yPos;
    private bool gravitySetting ;
    private bool freezeRotationSetting ;
    private float sqrMoveLimit ;
    private int collisionCount = 0;
    private Transform camTransform ;
    private Vector3 snapPosition;
    private bool doSnap;
    

    void Start ()
    {
        snapPosition = Vector3.zero;
        doSnap = false;
        
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
        sqrMoveLimit = moveLimit * moveLimit;   // Since we're using sqrMagnitude, which is faster than magnitude
    }
    
    void OnTriggerEnter(Collider other) {
  
        Snappable snappable = 
                other.gameObject.GetComponent(typeof(Snappable)) as Snappable;
        
        
        // Ignore extraneous collisions and collisions while 
        // moving.
        if (snappable == null)
            return;
   
            
        
        doSnap = true;
        snapPosition = other.gameObject.transform.position;
    }
    
    void OnTriggerStay(Collider other) {
        //doSnap=true;
    }
    
    void OnTriggerExit(Collider other) {

        // Ignore extraneous collisions and collisions while 
        // moving.
        if (other.gameObject.tag != SNAP_AREA_TAG) 
            return;
    
        doSnap = false;
    }
    
    
 
    void OnMouseDown ()
    {
        canMove = true;
        myTransform.Translate (Vector3.up * addHeightWhenClicked);
        gravitySetting = myRigidbody.useGravity;
        freezeRotationSetting = myRigidbody.freezeRotation;
        myRigidbody.useGravity = false;
        myRigidbody.freezeRotation = freezeRotationOnDrag;
        yPos = myTransform.position.y;
    }
 
    void OnMouseUp ()
    {
        canMove = false;
        myRigidbody.useGravity = gravitySetting;
        myRigidbody.freezeRotation = freezeRotationSetting;
        
        if (!myRigidbody.useGravity) {
            Vector3 pos = myTransform.position;
            pos.y = yPos - addHeightWhenClicked;
            myTransform.position = pos;

        }
    }
 
    void moveSnap(){

        snapPosition.y = gameObject.transform.position.y;
        myRigidbody.MovePosition(snapPosition);
        
        doSnap = false;
        SendMessageUpwards("Snapped");
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
        if (collisionCount > normalCollisionCount) {
            move = move.normalized * collisionMoveFactor;
        } else if (move.sqrMagnitude > sqrMoveLimit) {
            move = move.normalized * moveLimit;
        }
 
        myRigidbody.MovePosition(myRigidbody.position + move);
    }
    
    
    void FixedUpdate ()
    {
        if (canMove) {       
            moveNormal();
        }
        else if (doSnap){
            moveSnap();
        }
    }
}
