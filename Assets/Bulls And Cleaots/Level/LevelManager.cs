using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BullsAndCleots.Mechanics;
using BullsAndCleots.Gui;

namespace BullsAndCleots.Level
{
	public class LevelManager : MonoBehaviour, IEventListener
	{

		// State and Init Data
		private LevelData initData = null;

		public LevelData InitData {
			get {
				if (initData != null) {
					return initData;
				}
				throw new System.NullReferenceException (
				"Bulls and Cleots level data has not been initialized!");
			}
			set {
				if (initData != null) {
					Debug.LogWarning ("Level data already set!");
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
		private EnergyBarManager barManager;
		public Rect attemptButtonRect;

		// Events
		static string onSnapName = typeof(DraggableOnSnapEvent).ToString ();
		static string snapOnEnterName = typeof(SnappableOnEnterEvent).ToString ();
		static string snapOnExitName = typeof(SnappableOnExitEvent).ToString ();
		static string onMoveName = typeof(DraggableOnMoveEvent).ToString ();
		private string[] handledEventNames = {
        onSnapName,
        onMoveName,
        snapOnEnterName,
        snapOnExitName,

    };

	#region Init Methods

		private void initStateData ()
		{
			hasWon = false;
			attemptCount = 0;
			slnManager = initData.SolutionManager;
		}

		private void initPlayerPieces ()
		{
			testBlocks = Instantiate (testBlocks) as GameObject;

			testBlocks.transform.parent = transform;

			testBlocks.GetComponent<BlockManager> ().Choices = initData.Choices;

			inputPane = Instantiate (inputPane) as GameObject;
			inputPane.GetComponent<InputPanel> ().solutionLength = slnManager.Length;
			inputPane.transform.parent = transform;
		}

		private void initGUI ()
		{
			GameObject gui = new GameObject ("Bulls and Cleots GUI");
			gui.transform.parent = transform;
			barManager = gui.AddComponent<EnergyBarManager> ();
			barManager.slnCount = slnManager.Count;
			barManager.slnLength = slnManager.Length;
		}

		private void initEvents ()
		{
			// Set the instance of the event manager to be a child of
			// the current scene so it gets cleaned up properly.
			EventManager.instance.transform.parent = transform;

			// register as listener for desired events
			foreach (string eventName in handledEventNames) {
				EventManager.instance.RegisterListener (this, eventName);
			}
		}

		void Start ()
		{
			initStateData ();
			initPlayerPieces ();
			initGUI ();
			initEvents ();
		}

	#endregion


		void OnGUI ()
		{
			Matrix4x4 originalMatrix;
			GuiUtilities.Scale (out originalMatrix);

			// Draw gui elements
			if (!hasWon && GUI.Button (attemptButtonRect, "Attempt")) {
				PlayerAttemptSolve ();
			} else if (hasWon && GUI.Button (attemptButtonRect, "You Win!")) {
				PlayerEndGame ();
			}


			// Reset the GUI matrix to what it was so as not to
			// invalidate any other minigame's assumptions about
			// how they want to use the GUI.
			GUI.matrix = originalMatrix;
		}

		/// <summary>
		/// Updates the Player Bulls and Cleots hint bars.
		/// </summary>
		void doGUIAccounting ()
		{

			int[] perSlnBulls = slnManager.GetBullsCount ();
			int[] perSlnCleots = slnManager.GetCleotsCount ();
			foreach (int index in Enumerable.Range(0, slnManager.Count)) {
				int cleots = perSlnCleots [index];
				int bulls = perSlnBulls [index];

				barManager.SetBulls (index, bulls);
				barManager.SetCleots (index, cleots);
			}

		}


		/// <summary>
		/// Called when the player hits the "Attempt" button.
		/// </summary>
		void PlayerAttemptSolve ()
		{

			if (slnManager.HasBlankGuesses) {
				return;
			}

			attemptCount++;

			doGUIAccounting ();

			if (slnManager.Solved) {
				hasWon = true;
				return;
			}

			ResetGuesses ();
		}

		void PlayerEndGame ()
		{
			int slnLength = slnManager.Length;
			SolutionMixMatrix hackyAwefulDataStore = TempDataStore.MixMatrix;

			MatrixRow curMix;
			MatrixRow slnMix = slnManager.SolutionMix;

			if (hackyAwefulDataStore.ContainsKey (slnLength)) {
				curMix = hackyAwefulDataStore [slnLength];
			} else {
				hackyAwefulDataStore [slnLength] = slnMix;
				// Drain semantics, baby!
				goto SkipMatrixUpdate;
			}

			foreach (string key in slnMix.Keys) {
				if (curMix.ContainsKey (key)) {
					curMix [key] += slnMix [key];
				} else {
					curMix [key] = slnMix [key];
				}
			}


		SkipMatrixUpdate:

			MiniGameController.endMiniGame (initData.EndEdge);


		}


		/// <summary>
		///  Accounting for a block being dropped to the guess pane.
		/// </summary>
		void GuessDropped (GameObject obj)
		{
			Draggable draggable = obj.GetComponent<Draggable> ();
			Snappable snappable = draggable.currentSnappable.GetComponent<Snappable> ();

			GameBlock solutionBlock = obj.GetComponent<GameBlock> ();
			SnapArea dropArea = draggable.currentSnappable.GetComponent<SnapArea> ();

			int index = dropArea.Index;
			object guess = solutionBlock.data;


			if (snappable.isAlreadyOccupied) {

				object temp = slnManager.GetGuess (index);
				draggable.Reset ();
				slnManager.SetGuess (index, temp);


			} else {
				slnManager.SetGuess (index, guess);

			}


		}

		/// <summary>
		/// Called when a the guess (GameBlock) exits a SolutionInput either
		/// by the player dragging it out or being moved by code.
		/// </summary>
		void GuessVacate (GameObject obj)
		{
			Draggable d = obj.GetComponent<Draggable> ();
			GameBlock solutionBlock = obj.GetComponent<GameBlock> ();

			SnapArea dropArea = d.currentSnappable.GetComponent<SnapArea> ();
			int index = dropArea.Index;

			object vacatingGuess = solutionBlock.data;

			object currentGuess = slnManager.GetGuess (index);

			// Guess vacating because snaparea already occupied so
			// keep current guess.
			if (currentGuess != vacatingGuess) {
				return;
			}

			// Otherwise it means that the snap area will be empty
			// after the guess vacates so set it to NOT_GUESSED
			slnManager.ResetGuess (index);

		}


		/// <summary>
		/// Move the guess blocks back to their starting positions and
		/// reset the guesses internally back to "NOT_GUESSED".
		/// </summary>
		void ResetGuesses ()
		{
			GetComponentInChildren<BlockManager> ().Reset ();
			// Might be belt and suspenders to do this as well
			// as the prevous reset since the blocks moving outside of the
			// input blocks will cause a reset event.
			slnManager.ResetGuesses ();
		}


		/// <summary>
		/// EventManager callback.
		/// </summary>
		public bool HandleEvent (IEvent evnt)
		{
			string eventName = evnt.GetName ();
			object eventData = evnt.GetData ();

			if (eventName == onSnapName) {

				GuessDropped (eventData as GameObject);
			} else if (eventName == onMoveName) {
				// ignore
			} else if (eventName == snapOnEnterName) {
				// ignore
			} else if (eventName == snapOnExitName) {

				GuessVacate (eventData as GameObject);

			} else {
				Debug.LogWarning ("Unexpected Event in BC: " + eventName);
				return false;
			}
			return true;
		}

	}
}