using UnityEngine;
using System.Collections;

public class SolutionMixEvent : IEvent {
      private string name;
    int digitCount;
    
    public SolutionMixEvent(int count){
        name = this.GetType().ToString();
        digitCount = count;
    }

    
    string IEvent.GetName(){
        return name;
    }
    object IEvent.GetData() {
        return digitCount;
    }
}
    
[RequireComponent(typeof(EnergyBar))]
public class SolutionMixBar : MonoBehaviour, IEventListener {
 
    void Start(){
        GetComponent<EnergyBar>().labelText = "Solution Mix";
        EventManager.instance.RegisterListener(this, typeof(SolutionMixEvent).ToString());
    }
    
    public bool HandleEvent(IEvent evnt){
        SetSolutionMix((int)evnt.GetData());
        return true;
    }
    
    public void SetSolutionMix(int numFound){
        
        EnergyBar energyBar = gameObject.GetComponent<EnergyBar>();
        energyBar.Value = energyBar.energyIncrement * numFound;
    }
   
}

