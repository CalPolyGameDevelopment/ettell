using UnityEngine;
using System.Collections;

namespace BullsAndCleots.Mechanics {

public class SnapArea : MonoBehaviour {

	private int index = -1;

	public int Index {
		get {
			return index;
		}
		set {
			if (index == -1 && value >= 0) {
				index = value;
			}
		}
	}

}
}