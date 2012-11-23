using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BullsAndCleots.Mechanics {

public class Solution {

	private ArrayList solutionList;
	private string dataSource;

	public Solution(string source, ArrayList slnList) {
		dataSource = source;
		solutionList = ArrayList.ReadOnly(new ArrayList(slnList)) as ArrayList;
	}

	/// <summary>
	/// Gets the length of the solution.
	/// </summary>
	public int Length {
		get{ return solutionList.Count; }
	}

	public string Source {
		get{ return dataSource; }
	}

	public IEnumerable<object> SolutionList {
		get{ return solutionList.Cast<object>();}
	}

	public bool IsCleot(int index, object guess) {

		if (IsBull(index, guess)) {
			return false;
		}

		return solutionList.Contains(guess);
	}

	public bool IsBull(int index, object guess) {
		return solutionList[index] == guess;

	}


	/// <summary>
	/// Gets ths number of bulls.
	/// </summary>
	public int GetBullsCount(IEnumerable<object> guesses) {
		int count = 0;
		int index = 0;
		foreach (object guess in guesses) {
			if (IsBull(index, guess)) {
				count++;
			}
			index++;
		}
		return count;
	}

	/// <summary>
	/// Gets the number of cleots.
	/// </summary>
	public int GetCleotsCount(IEnumerable<object> guesses) {
		int count = 0;
		int index = 0;
		foreach (object guess in guesses) {
			if (IsCleot(index, guess)) {
				count++;
			}
			index++;
		}
		return count;
	}

}
}