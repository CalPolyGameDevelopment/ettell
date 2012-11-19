using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// public class Solution<T> where T : IComparable?
public class Solution {
    public static object NOT_GUESSED = null;
    
    private ArrayList solutionList; 
    private ArrayList guessList;
    
    
    public Solution(IList sList, IList gList){
        
        solutionList = ArrayList.ReadOnly(sList) as ArrayList;
        guessList = ArrayList.FixedSize(gList) as ArrayList;

    }
 
    /// <summary>
    /// Gets the length of the solution.
    /// </summary>
    public int Length  {
     get{ return solutionList.Count; }
    }
    
    public IEnumerable<object> SolutionList{
        get{ return solutionList.Cast<object>();}
    }
     
    public IEnumerable<object> GuessList{
        get{ return guessList.Cast<object>(); }
    }
 
    /// <summary>
    /// Gets the number of cleots.
    /// </summary>
    public int CleotsCount {
        get{
   
            return GuessList.Count(elem => isCleot (elem));
        }
    }
 
    /// <summary>
    /// Gets ths number of bulls.
    /// </summary>
    public int BullsCount {
        get {
            return GuessList.Count (elem => isBull(elem));
        }

    }
   
    public bool HasBlankGuesses {
        get {
            return GuessList.Any (elem => elem == NOT_GUESSED);
        }
    }
 
    public bool Solved{
        get{
            return GuessList.All (elem => isBull(elem));
        }
    }

    bool isCleot(object guess) {
        
        if (isBull(guess))
            return false;
        
        return solutionList.Contains(guess);
    }

    bool isBull(object guess) {
        int index = guessList.IndexOf(guess);
  
        return solutionList[index] == guess;
      
    }

    /// <summary>
    /// Sets guessList[index] = NOT_GUESSED.
    /// </summary>
    public void ResetGuessAt(int index) {
        if (index < 0 || index > Length)
        {
            Debug.LogError("Cannot reset guess: index out of range.");
            return;
        }

        guessList[index] = NOT_GUESSED;
    }
 
    /// <summary>
    /// map(x => NOT_GUESSED, guessList)
    /// </summary>
    public void ResetGuesses() {
        for (int index = 0; index < Length; index++)
            ResetGuessAt(index);
        
    }

    
}



public class SolutionManager {
    
    private List<Solution> solutions;

   
    public SolutionManager() {
        solutions = new List<Solution>();
    }

    public void AddSolution(Solution sln){
        if(solutions.Contains(sln)){
            Debug.LogWarning("Adding duplicate solution.");
        }
        solutions.Add(sln);
    }
    
    


}
