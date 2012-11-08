using UnityEngine;
using System.Collections;

public class CleotsFoundEvent : IEvent {
      private string name;
    int cleotCount;
    
    public CleotsFoundEvent(int count){
        name = this.GetType().ToString();
        cleotCount = count;
    }

    
    string IEvent.GetName(){
        return name;
    }
    object IEvent.GetData() {
        return cleotCount;
    }
}
    
[RequireComponent(typeof(EnergyBar))]
public class CleotBar : MonoBehaviour, IEventListener {
 
    void Start(){
        EventManager.instance.RegisterListener(this, typeof(CleotsFoundEvent).ToString());
    }
    
    public bool HandleEvent(IEvent evnt){
        FoundCleots((int)evnt.GetData());
        return true;
    }
    
    public void FoundCleots(int numFound){
        
        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Value = energyBar.energyIncrement * numFound;
    }
   
}
