using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BullsAndCleots.Mechanics;

namespace BullsAndCleots.Level {

public class LevelData {
	private SolutionManager slnManager;
	private List<List<Material>> choices;
	private Ending ending; 
		
	public LevelData(SolutionManager mgr, List<List<Material>> chcs, Ending end) {
		slnManager = mgr;
		choices = chcs;
		ending = end;
	}

	public SolutionManager SolutionManager {
		get{ return slnManager;}
	}

	public List<List<Material>> Choices {
		get{ return choices; }
	}
	
	public string EndEdge{
		get{ return ending.edgeId; }
	}
}
}