using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BullsAndCleots.Mechanics;

namespace BullsAndCleots.Level {

public class LevelData {
	public SolutionManager slnManager;
	public List<List<Material>> choices;

	public LevelData(SolutionManager mgr, List<List<Material>> chcs) {
		slnManager = mgr;
		choices = chcs;
	}

	public SolutionManager SolutionManager {
		get{ return slnManager;}
	}

	public List<List<Material>> Choices {
		get{ return choices; }
	}
}
}