using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BullsAndCleots.Mechanics {

public class SolutionManager {

	public static object NOT_GUESSED = null;
	private List<Solution> solutions;

	// Each solution should have "length" number of elements.
	private int length;
	private object[] guesses;

	public SolutionManager(int slnLength) {
		solutions = new List<Solution>();
		length = slnLength;
		guesses = new object[length];
		ResetGuesses();
	}

	public void AddSolution(Solution sln) {
		if (solutions.Contains(sln)) {
			Debug.LogWarning("Adding duplicate solution.");
		}
		if (sln.Length != length) {
			throw new System.ArgumentException(string.Format(
				"Solution.Length == {0} while this solution manager only accepts " +
				"solutions of Length == {1}.", sln.Length, Length));
		}
		solutions.Add(sln);
	}

	public int Length {
		get {
			return length;
		}
	}

	public IEnumerable<object> Guesses {
		get {
			return guesses.AsEnumerable();
		}
	}

	/// <summary>
	/// Gets the number of cleots for each solution.
	/// </summary>
	public int[] GetCleotsCount() {
		List<int> cleots = new List<int>();

		foreach (Solution sln in solutions) {
			int cleotCount = sln.GetCleotsCount(Guesses);
			cleots.Add(cleotCount);
		}

		return cleots.ToArray();
	}

	/// <summary>
	/// Gets ths number of bulls for each solution
	/// </summary>
	public int[] GetBullsCount() {
		List<int> bulls = new List<int>();

		foreach (Solution sln in solutions) {
			int bullCount = sln.GetBullsCount(Guesses);
			bulls.Add(bullCount);
		}

		return bulls.ToArray();
	}

	public bool HasBlankGuesses {
		get {
			return Guesses.Any(g => g == NOT_GUESSED);
		}
	}

	public bool Solved {
		get {
			int index = 0;
			foreach (object guess in Guesses) {
				if (!solutions.Any(sln => sln.IsBull(index, guess))) {
					return false;
				}
				index++;
			}
			return true;
		}
	}

	public MatrixRow SolutionMix{
		get {
			MatrixRow mixRow = new MatrixRow();
			foreach(Solution sln in solutions){
				mixRow[sln.Source] = sln.GetBullsCount(Guesses);
			}
			return mixRow;
		}
	}

	public int Count {
		get {
			return solutions.Count;
		}
	}

	public object GetGuess(int index) {
		return guesses[index];
	}

	public void SetGuess(int index, object guess) {
		guesses[index] = guess;
	}

	public string GetSolutionSource(int index){
		return solutions[index].Source;
	}

	/// <summary>
	/// Sets guessList[index] = NOT_GUESSED.
	/// </summary>
	public void ResetGuess(int index) {
		if (index < 0 || index > Length) {
			Debug.LogError("Cannot reset guess: index out of range.");
			return;
		}

		guesses[index] = NOT_GUESSED;
	}


	/// <summary>
	/// map(x => NOT_GUESSED, guessList)
	/// </summary>
	public void ResetGuesses() {
		guesses = Guesses.Select<object,object>(x => NOT_GUESSED).ToArray();

	}

}

}