using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BullsAndCleotsLevelController : MonoBehaviour, IEventListener
{
    
    public Rect attemptButtonRect;
    public int[] winDigits;
    private int[] guessDigits;
    private bool hasWon;
    private Rect messagesRect = new Rect (200, 200, 200, 400);
    private int maxMessages = 10;
    private Queue<string> playerMessageQueue;
    private const int DIGIT_NOT_GUESSED = -1;
    private uint attemptCount;
    
    public GameObject numberedBlocks;
    
    static string onSnapName = typeof(DraggableOnSnapEvent).ToString ();
    static string snapOnEnterName = typeof(SnappableOnEnterEvent).ToString ();
    static string snapOnExitName = typeof(SnappableOnExitEvent).ToString ();
    static string onMoveName = typeof(DraggableOnMoveEvent).ToString();
    
    private string[] handledEventNames = {
        onSnapName,
        onMoveName,
        snapOnEnterName,
        snapOnExitName,

    };
    
    void Start ()
    {
    
        // init vars
        hasWon = false;
        attemptCount = 0;
        playerMessageQueue = new Queue<string> (maxMessages);
        
        int length = winDigits.GetLength (0);
        guessDigits = new int[length];
        
        for (int i = 0; i < length; i++) {
            guessDigits [i] = DIGIT_NOT_GUESSED;
        }
        
        
        
        // register event handlers
        foreach (string eventName in handledEventNames) {
            EventManager.instance.RegisterListener (this, (string)eventName);
        }
    
       numberedBlocks = Instantiate(numberedBlocks) as GameObject;
      //  numberedBlocks = Instantiate(Resources.LoadAssetAtPath(
       //         "Bulls And Cleots/Prefabs/NumberedBlocks.prefab", typeof(GameObject)));
    }
    
    public bool HandleEvent (IEvent evnt)
    {
        string eventName = evnt.GetName ();
        object eventData = evnt.GetData ();
        
        if (eventName == onSnapName) {     
            DigitDropped (eventData as GameObject);
        }
        else if (eventName == onMoveName){
            // ignore
        }
        else if (eventName == snapOnEnterName) {
            // ignore
        } else if (eventName == snapOnExitName) {
            DigitVacateDropArea (eventData as GameObject);
        } else {
            Debug.LogWarning ("Unexpected Event: " + eventName);
            return false;
        }
        return true;
    }
    
    // Checks for guess slots that have not had a number
    // dropped into them.
    bool hasBlankGuesses {
        get {
            return Array.IndexOf (guessDigits, DIGIT_NOT_GUESSED) > -1;
        }
    }
    
    //
    bool indexIsCleot (int index)
    {
        return Array.IndexOf (winDigits, guessDigits [index]) > -1;
    }
    
    bool indexIsBull (int index)
    {
        return winDigits [index] == guessDigits [index];
    }
    
    void PlayerAttemptSolve ()
    {
        
        // The length of winDigits and guessDigits should 
        // be equal.
        int length = winDigits.GetLength (0);
        
        int bullCount = 0;
        int cleotCount = 0;
        
        if (hasBlankGuesses) { 
            // We don't allow for there to be any unfilled 
            // guess slots because that would make the game 
            // too easy. So complain to the user.
            AddPlayerMessage("Blank Guesses!");
            return;
            
        }
        
        // Interate through the possible indexes for 
        // winDigits and guessDigits.
        for (int i = 0; i < length; i++) {
            if (indexIsBull (i)) {
                // the value at is equal in both 
                bullCount++;
                continue;
            } else if (indexIsCleot (i)) {
                // the guess value exists in winDigits
                cleotCount++;
                continue;
            }       
        }
        
        
        attemptCount++;
        
        string attemptStatusMessage = "";
        // Example:
        //#1 (1234): 2 Bulls, 1 Cleots 
        attemptStatusMessage = String.Format ("#{0} ({1}{2}{3}{4}): {5} Bulls, {6} Cleots",
                attemptCount,
                guessDigits [0], guessDigits [1], guessDigits [2], guessDigits [3], 
                bullCount, cleotCount);
        
        AddPlayerMessage (attemptStatusMessage);
        
        if (bullCount == length) {
            // winDigits == guessDigits, so the player has guessed
            // the correct number.
            hasWon = true;
            AddPlayerMessage ("You win!");
        }
        
        
        
        // TODO.D: Make this sane. Couple the <x>Bar with the level controller.
        // These do not need to be and should not be events. I got 
        // caught using my shiny new hammer to do something better
        // suited to a screwdriver you might say.
        EventManager.instance.QueueEvent (new BullsFoundEvent (bullCount));
        EventManager.instance.QueueEvent (new CleotsFoundEvent (cleotCount));
   
        numberedBlocks.GetComponent<NumberedBlocks>().Reset();
    }
    
    // Adds the message to the message queue. If there are 
    // more than maxMessageCount messages in the queue 
    // then remove excess messages.
    void AddPlayerMessage (string message)
    {
        playerMessageQueue.Enqueue (message);
        while (playerMessageQueue.Count > maxMessages) {
            playerMessageQueue.Dequeue ();
        }
    }
    
    void PlayerEndGame ()
    {
        // TODO.D: Get rid of naked string
        MiniGameController.endMiniGame ("bullsAndCleotsWinEdge");
    }
    
    
    // Accounts for a number being dropped in the guess pane.
    void DigitDropped (GameObject obj)
    {
     
        Draggable draggable = obj.GetComponent<Draggable> ();
        Snappable snappable = draggable.currentSnappable.GetComponent<Snappable>();

        
        
        NumericalDigit number = obj.GetComponent<NumericalDigit> ();   
        DigitDropArea dropArea = draggable.currentSnappable.GetComponent<DigitDropArea> ();

        int index = dropArea.index;
        
 
                        
        if (snappable.isAlreadyOccupied){
            int temp = guessDigits[index];
            draggable.Reset();
            guessDigits[index] = temp;
            
        }else{
            
             guessDigits[index] = number.digit;
        }
        
        
       
        
    }
    
    // Called when a NumericalDigit exits a DigitDropArea
    // to make sure that the guessDigit gets reset.
    void DigitVacateDropArea (GameObject obj)
    {
        Draggable d = obj.GetComponent<Draggable>();
      
        DigitDropArea dropArea = d.currentSnappable.GetComponent<DigitDropArea> ();
        int index = dropArea.index;
        int guessDigit = obj.GetComponent<NumericalDigit>().digit;
        int currentDigit = guessDigits[index];
        
        // Make sure this isn't due to a Reset().
        if (currentDigit == DIGIT_NOT_GUESSED ||
            guessDigit == currentDigit){
        guessDigits [index] = DIGIT_NOT_GUESSED;
        }
        
   
          
    }
    
    void drawAttemptButton ()
    {
        if (GUI.Button (attemptButtonRect, "Attempt")) {
            PlayerAttemptSolve ();
        }
    }
   
    void drawWinButton ()
    {
        if (GUI.Button (attemptButtonRect, "You Win!")) {
            PlayerEndGame ();
        }
    }
      
    
    // Displays the latest maxMessageCount> messages in a text box 
    // to the player.
    void drawMessages ()
    {
        GUI.TextArea (messagesRect, 
            string.Join ("\n", playerMessageQueue.ToArray ()));
    }
   
    void OnGUI ()
    {
      
        if (!hasWon) {
            drawAttemptButton ();
        } else {
            drawWinButton ();
            
        }
        
        drawMessages ();
    }
}
