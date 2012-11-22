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
	
	// State and Init Data
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
	
	private bool hasWon;
	private int attemptCount;
    public SolutionManager slnManager;
	
	// Player interaction objects
    public GameObject inputPane;
    public GameObject testBlocks;
    
	// GUI/HUD 
	public EnergyBarManager barManager;
	public Rect attemptButtonRect;	
	
	// Events
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
 
	#region Init Methods
	
	private void initStateData(){
		hasWon = false;
        attemptCount = 0;
		slnManager = initData.SolutionManager;
	}
	
	private void initPlayerPieces(){
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
	}
	
	private void initGUI(){
		GameObject gui = new GameObject("Bulls and Cleots GUI");
		gui.transform.parent = transform;
		barManager = gui.AddComponent<EnergyBarManager>();
		barManager.slnCount = slnManager.Count;
		barManager.slnLength = slnManager.Length;
	}
	
	private void initEvents(){
		// Set the instance of the event manager to be a child of
		// the current scene so it gets cleaned up properly.
		EventManager.instance.transform.parent = transform;
		
        // register as listener for desired events
        foreach (string eventName in handledEventNames) {
            EventManager.instance.RegisterListener(this, eventName);
        }
	}
	void Start() {

        initStateData();
		initPlayerPieces();
		initGUI();
		initEvents();
     
    }
	
	#endregion
	

	void OnGUI() {

        if(!hasWon && GUI.Button(attemptButtonRect, "Attempt")) {
        	PlayerAttemptSolve();
        }
        else if (hasWon && GUI.Button(attemptButtonRect, "You Win!")) {
            PlayerEndGame();
        }

    }
	
	/// <summary>
	/// Updates the Player Bulls and Cleots hint bars.
	/// </summary>
	void doGUIAccounting(){
		
		int[] perSlnBulls = slnManager.GetBullsCount();
		int[] perSlnCleots = slnManager.GetCleotsCount();
		foreach(int index in Enumerable.Range(0, slnManager.Count)){
			int cleots = perSlnCleots[index];
			int bulls = perSlnBulls[index];
			
			barManager.SetBulls(index, bulls);
			barManager.SetCleots(index, cleots);
		}
		
	}
	
 
	/// <summary>
	/// Called when the player hits the "Attempt" button.
	/// </summary>
    void PlayerAttemptSolve() {
		
		if(slnManager.HasBlankGuesses){
			return;
		}
		
		attemptCount++;
		
		doGUIAccounting();
		
		if(slnManager.Solved){
			hasWon = true;
			return;
		}
		
		ResetGuesses();
	}



    void PlayerEndGame() {

		MiniGameController.endMiniGame(
        	BullsAndCleots.ExitEdges.IncreaseDifficulty);

	}

    
    
   
    /// <summary>
    ///  Accounting for a block being dropped to the guess pane.
    /// </summary>
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

   
    /// <summary>
    /// Called when a the guess (SolutionBlock) exits a SolutionInput either
    /// by the player dragging it out or being moved by code. 
    /// </summary>
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
	
	
	/// <summary>
	/// Move the guess blocks back to their starting positions and
	/// reset the guesses internally back to "NOT_GUESSED".
	/// </summary>
	void ResetGuesses(){
		GetComponentInChildren<SolutionBlocks>().Reset();
		// Might be belt and suspenders to do this as well
		// as the prevous reset since the blocks moving outside of the
		// input blocks will cause a reset event.
		slnManager.ResetGuesses();
	}
	

	/// <summary>
	/// EventManager callback.
	/// </summary>
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
            // ignore                                                                  e
        }
        else if (eventName == snapOnExitName) {
  
            GuessVacate(eventData as GameObject);

        }
        else {
            Debug.LogWarning("Unexpected Event in BC: " + eventName);
            return false;
        }
        return true;
    }
	
}
