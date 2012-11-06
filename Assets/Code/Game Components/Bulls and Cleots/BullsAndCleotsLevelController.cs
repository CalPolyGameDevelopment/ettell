using UnityEngine;
using System.Collections;
using System;

public class BullsAndCleotsLevelController : MonoBehaviour {
 
    public int[] winDigits;
	
    public void UserAttemptSolve(){
        BroadcastMessage("PlayerWinEvent");    
    }
    
    public void UserEndGame(){
        MiniGameController.endMiniGame("bullsAndCleotsWinEdge");
    }
    
    
    public void DigitDropped(GameObject obj){
     
        Dragable dragable = obj.GetComponent(typeof(Dragable)) as Dragable;
        NumericalDigit number = obj.GetComponent(typeof(NumericalDigit)) as NumericalDigit;
        
        DigitDropArea dropArea = dragable.currentSnapArea.GetComponent(typeof(DigitDropArea)) as DigitDropArea;
        
        int index = dropArea.index;
    
        int winDigit = this.winDigits[index];
        int guessDigit = number.digit;
        
        if (winDigit == guessDigit){
            BroadcastMessage("FoundBull");
        }
        else if (Array.IndexOf(winDigits, winDigit) > 0){
            
            BroadcastMessage("FoundCleot");
        }
        
        
    }
}
