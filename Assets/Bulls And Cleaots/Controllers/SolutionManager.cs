using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public class SolutionManager {



    public static object NOT_GUESSED = null;
    
    private int solutionLength = 0;
    
    private int[] solutionDigits;
    private Color[] solutionColors;
    private object[] solutionGuesses;
    

    public SolutionManager(int length) {

        solutionLength = length;

        solutionColors = new Color[solutionLength];
        solutionDigits = new int[solutionLength];
        solutionGuesses = new object[solutionLength];

    }



    public int SolutionLength{
        get {
            return solutionLength;
        }
      

    }


    public int[] Digits {
        
        set {

            Array.Copy(value, solutionDigits, solutionLength);
        }
    }

    public Color[] Colors {
        
        set {
            Array.Copy(value, solutionColors, solutionLength);
        }
    }

    public object[] Guesses {
        get {
            object[] retval = new object[solutionLength];
            Array.Copy(solutionGuesses, retval, solutionLength);
            return retval;
        }
    }

    public void SetGuessAt(int index, object guess) {
        if (index < 0 || index > solutionLength) {
            Debug.LogError("Cannot set guess: index out of range.");
            return;
        }
        if (solutionGuesses[index] != NOT_GUESSED)
            Debug.LogWarning("Overwriting guess at index: " + index.ToString());

        solutionGuesses[index] = guess;
    }
  
    // Checks for guess slots that have not had a number
    // dropped into them.
    public bool HasBlankGuesses {
        get {
            return solutionGuesses.Any(elem => elem == NOT_GUESSED);
        }
    }

    bool GuessIsCleot(object guess) {
        
        if (GuessIsBull(guess))
            return false;

        Type guessType = guess.GetType();

        if (guessType == typeof(int))
            return Array.IndexOf<int>(solutionDigits, (int)guess) > -1;

        if (guessType == typeof(Color))
            return Array.IndexOf<Color>(solutionColors, (Color)guess) > -1;
        
        return false;
    }

  
  

    bool GuessIsBull(object guess) {
        int index = Array.IndexOf(solutionGuesses, guess);

        Type guessType = guess.GetType();

        if (guessType == typeof(int))
            return solutionDigits[index] == (int)guess;
        if (guessType == typeof(Color))
            return solutionColors[index] == (Color)guess;
       
        return false;
    }


    public int CleotsCount() {

        return solutionGuesses.Count<object>(eleme => GuessIsCleot(eleme));
    }

    public int BullsCount() {
        return solutionGuesses.Count<object>(elem => GuessIsBull(elem));

    }

    public int DigitCount() {
        return solutionGuesses.Count<object>(
            o => o != null && o.GetType() == typeof(int));
    }
    public int ColorCount() {
        return solutionGuesses.Count<object>(
            o => o != null && o.GetType() == typeof(Color));
    }

    public void ResetGuessAt(int index) {
        if (index < 0 || index > solutionLength)
        {
            Debug.LogError("Cannot reset guess: index out of range.");
            return;
        }

        solutionGuesses[index] = NOT_GUESSED;
    }

    public void ResetGuesses() {
        for (int index = 0; index < solutionLength; index++)
            ResetGuessAt(index);   
        
    }


    public override string ToString() {
        string retval = "";

        foreach (object guess in solutionGuesses) {
            if (guess != NOT_GUESSED)
                retval += guess.ToString();
            else
                retval += "?";
        }
        return retval;
    }
    
}
