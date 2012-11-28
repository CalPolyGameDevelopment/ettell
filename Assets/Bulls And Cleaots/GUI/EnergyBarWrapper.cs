using UnityEngine;
using System;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace BullsAndCleots.Gui {

public struct EnergyBarWrapper {
	private EnergyBar bullsBar;
	private EnergyBar cleotsBar;
	TextInfo myTI;

	public EnergyBarWrapper(EnergyBar bBar, EnergyBar cBar) {
		bullsBar = bBar.GetComponent<EnergyBar>();
		cleotsBar = cBar.GetComponent<EnergyBar>();
		// Take that h-bar.
		myTI  = new CultureInfo("en-US",false).TextInfo;
	}

	public void SetBulls(int bulls, int slnLength) {
		bullsBar.SetFullness(bulls, slnLength);
	}

	public void SetCleots(int cleots, int slnLength) {
		cleotsBar.SetFullness(cleots, slnLength);
	}

	public void SetLabel(string label) {

		bullsBar.labelText = myTI.ToTitleCase(label);
	}
}
}