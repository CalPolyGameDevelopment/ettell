using UnityEngine;
using System.Collections;

public class WaveDefenseLevel : MonoBehaviour , IEventListener{
 
    bool hasWon = false;
	// Use this for initialization
	void Start () {
	    EventManager.instance.RegisterListener(this, typeof(WaveCompletedEvent).ToString());
	}
	
    bool IEventListener.HandleEvent(IEvent evnt){
        hasWon = true;
        return true;
    }
    
    void OnGUI(){
        if (hasWon)
            if(GUI.Button(new Rect(400,10,100,100), "YOU WON!")){
                MiniGameController.endMiniGame("firstPuzzleRecur");
            }
    }
    
	// Update is called once per frame
	void Update () {
	
	}
}
