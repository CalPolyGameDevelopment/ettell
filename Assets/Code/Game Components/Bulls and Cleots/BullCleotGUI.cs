using UnityEngine;
using System.Collections;

public class BullCleotGUI : MonoBehaviour {
 
    public Rect attemptButtonRect;
    private bool hasWon;
    
    void Start(){
        hasWon = false;
    }
    
    void drawAttemptButton() {
       if (GUI.Button (attemptButtonRect, "Attempt")) {
         SendMessageUpwards("UserAttemptSolve");
      }
    }
   
    void drawWinButton() {
               if (GUI.Button (attemptButtonRect, "You Win!")) {
         SendMessageUpwards("UserEndGame");
      }
    }
      
    
    public void PlayerWinEvent() {
        hasWon = true;
    }
    
    void OnGUI(){
      
      if (!hasWon)
            drawAttemptButton();
        else 
            drawWinButton();
            
   
    }
	
}
