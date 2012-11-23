using UnityEngine;
using System.Collections;

namespace BullsAndCleots.Mechanics {

public class SnappableOnEnterEvent : IEvent {
	private string name;
	private GameObject eventSnappable;

	public SnappableOnEnterEvent(GameObject eSnappable) {
		name = this.GetType().ToString();
		eventSnappable = eSnappable;

	}

	string IEvent.GetName() {
		return name;
	}

	object IEvent.GetData() {
		return eventSnappable;
	}
}

public class SnappableOnExitEvent : IEvent {
	private string name;
	private GameObject eventSnappable;

	public SnappableOnExitEvent(GameObject eSnappable) {

		name = this.GetType().ToString();
		eventSnappable = eSnappable;

	}

	string IEvent.GetName() {
		return name;
	}

	object IEvent.GetData() {
		return eventSnappable;
	}
}

public class Snappable : MonoBehaviour {

	private int occupierCount;

	public bool isAlreadyOccupied {
		get {
			return occupierCount > 1;
		}
	}

	void Start() {
		occupierCount = 0;
	}

	void OnTriggerEnter(Collider other) {

		Draggable draggable = other.GetComponent<Draggable>();
		if (draggable == null) {
			return;
		}

		occupierCount++;
		EventManager.instance.RelayEvent(new SnappableOnEnterEvent(draggable.gameObject));


	}

	void OnTriggerExit(Collider other) {
		Draggable draggable = other.GetComponent<Draggable>();
		if (draggable == null) {
			return;
		}

		occupierCount--;
		EventManager.instance.RelayEvent(new SnappableOnExitEvent(draggable.gameObject));

	}

}
}