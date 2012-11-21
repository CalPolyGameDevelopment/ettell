using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BCLevelData {
    public SolutionManager slnManager;
    public List<List<Material>> choices;


    public BCLevelData(SolutionManager mgr, List<List<Material>> chcs){
        slnManager = mgr;
        choices = chcs;
    }


    public SolutionManager SolutionManager{
         get{ return slnManager;}
    }
    public List<List<Material>> Choices{
         get{ return choices; }
    }
}


public class BullsAndCleotsLevelController : MonoBehaviour, IEventListener {

    public Rect attemptButtonRect;

    public SolutionManager slnManager;

    public GameObject inputPane;
    public GameObject testBlocks;
    
    static string onSnapName = typeof(DraggableOnSnapEvent).ToString();
    static string snapOnEnterName = typeof(SnappableOnEnterEvent).ToString();
    static string snapOnExitName = typeof(SnappableOnExitEvent).ToString();
    static string onMoveName = typeof(DraggableOnMoveEvent).ToString();
 
    private string[] handledEventNames = {
        onSnapName,
        onMoveName,
        snapOnEnterName,
        snapOnExitName,

    };
 
    private bool hasWon;
	private int attemptCount;
	
    private BCLevelData initData = null;
    public BCLevelData InitData{
		get {
			if( initData != null ){
				return initData;
			}
			throw new System.NullReferenceException(
				"Bulls and Cleots level data has not been initialized!");
		}
        set {
            if (initData != null){
                Debug.LogWarning("Level data already set!");
                return;
            }
            initData = value;
        }
    }
 
    void Start() {
        hasWon = false;
        attemptCount = 0;
        

        slnManager = initData.SolutionManager;
        testBlocks = Instantiate(testBlocks) as GameObject;

        testBlocks.transform.parent = transform;
		List<Material> allMaterials = new List<Material>();
		
		// Look up Aggregate/Accumulate for this instead.
        foreach(List<Material> mats in initData.Choices){
            
            allMaterials.AddRange(mats);
        }
		
		testBlocks.GetComponent<SolutionBlocks>().Choices = allMaterials.ToArray();
		
        inputPane = Instantiate(inputPane) as GameObject;
        inputPane.GetComponent<SolutionInputPanel>().solutionLength = slnManager.Length;
        inputPane.transform.parent = transform;
        
        // register as listener for desired events
        foreach (string eventName in handledEventNames) {
            EventManager.instance.RegisterListener(this, eventName);
        }
     
    }

    public bool HandleEvent(IEvent evnt) {
        string eventName = evnt.GetName();
        object eventData = evnt.GetData();

        if (eventName == onSnapName) {

            GuessDropped(eventData as GameObject);
        }
        else if (eventName == onMoveName) {
            // ignore
        }
        else if (eventName == snapOnEnterName) {
            // ignore
        }
        else if (eventName == snapOnExitName) {
  
            GuessVacate(eventData as GameObject);

        }
        else {
            Debug.LogWarning("Unexpected Event: " + eventName);
            return false;
        }
        return true;
    }

 

    void PlayerAttemptSolve() {
		
		if(slnManager.HasBlankGuesses){
			Debug.Log("blank guesses");
			return;
		}
		
		attemptCount++;
		
		if(slnManager.Solved){
			hasWon = true;
			Debug.Log ("solved");
			return;
		}
		
		
		Debug.Log(attemptCount);
		ResetGuesses();
    }



    void PlayerEndGame() {

		MiniGameController.endMiniGame(
        	BullsAndCleots.ExitEdges.IncreaseDifficulty);

	}

    
    
    // Account for a color block being dropped to the guess pane.
    void GuessDropped(GameObject obj) {
        Draggable draggable = obj.GetComponent<Draggable>();
        Snappable snappable = draggable.currentSnappable.GetComponent<Snappable>();

        SolutionBlock solutionBlock = obj.GetComponent<SolutionBlock>();
        SolutionSnapArea dropArea = draggable.currentSnappable.GetComponent<SolutionSnapArea>();
        
      	int index = dropArea.Index;
      	object guess = solutionBlock.data;
  	
        
        if (snappable.isAlreadyOccupied) {
            
            object temp = slnManager.GetGuess(index);
            draggable.Reset();
            slnManager.SetGuess(index, temp);
             

        }
        else {
            slnManager.SetGuess(index, guess);

        }
        

    }

   
    
    void GuessVacate(GameObject obj) {
        Draggable d = obj.GetComponent<Draggable>();
        SolutionBlock solutionBlock = obj.GetComponent<SolutionBlock>();

        SolutionSnapArea dropArea = d.currentSnappable.GetComponent<SolutionSnapArea>();
        int index = dropArea.Index;

        object vacatingGuess = solutionBlock.data;
        
        object currentGuess = slnManager.GetGuess(index);
            
        // Guess vacating because snaparea already occupied so
        // keep current guess.
        if (currentGuess != vacatingGuess)
            return;

        // Otherwise it means that the snap area will be empty
        // after the guess vacates so set it to NOT_GUESSED
        slnManager.ResetGuess(index);
        
    }

	void ResetGuesses(){
		GetComponentInChildren<SolutionBlocks>().Reset();
		// Might be belt and suspenders to do this as well
		// as the prevous reset since the blocks moving outside of the
		// input blocks will cause a reset event.
		slnManager.ResetGuesses();
	}
	
    void OnGUI() {

        if(!hasWon && GUI.Button(attemptButtonRect, "Attempt")) {
        	PlayerAttemptSolve();
        }
        else if (hasWon && GUI.Button(attemptButtonRect, "You Win!")) {
            PlayerEndGame();
        }

    }
}
