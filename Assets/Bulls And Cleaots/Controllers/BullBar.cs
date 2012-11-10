using UnityEngine;
using System.Collections;

public class BullsFoundEvent : IEvent {
    private string name;
    int bullCount;

    public BullsFoundEvent(int count) {
        name = this.GetType().ToString();
        bullCount = count;
    }


    string IEvent.GetName() {
        return name;
    }
    object IEvent.GetData() {
        return bullCount;
    }
}


[RequireComponent(typeof(EnergyBar))]
public class BullBar : MonoBehaviour, IEventListener {

    void Start() {
        GetComponent<EnergyBar>().labelText = "Bulls";
        EventManager.instance.RegisterListener(this, typeof(BullsFoundEvent).ToString());
    }

    public bool HandleEvent(IEvent evnt) {
        FoundBulls((int)evnt.GetData());
        return true;
    }

    public void FoundBulls(int numFound) {

        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Value = energyBar.energyIncrement * numFound;
    }
}
