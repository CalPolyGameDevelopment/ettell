using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public interface IEventListener
{
    bool HandleEvent (IEvent evt);
}


public interface IEvent 
{
    string GetName();
    object GetData ();
}


public class EventObject : IEvent{
    private string eventName;
    private object eventData;
    
    
    public EventObject(object data){
        eventName = this.GetType().ToString();
        eventData = data; 
    }
    
    public EventObject(){
        eventName = this.GetType().ToString();
        eventData = null; 
    }
 
    public virtual string GetName(){
        return eventName;
    }
    public virtual object GetData(){
        return eventData;
    }
    
}

/// EventManager - A stupid simple event manager class. 
/// Register a listener for a specific event type. On Update(),
/// if that particular type of event exists in the eventQueue then
/// the HandleEvent() method is called in the listener class for
/// that object.
/// 
/// Thoughts:
///  1. The primary extention for this class would be to make 
///     the eventListenerMap handle multiple listeners for the same
///     event type. This is not needed for the current usage.
public class EventManager : MonoBehaviour
{
    
    private static EventManager singleton = null;
    
    private Queue eventQueue;
 
    // event-name => event listerer class instance
    private Dictionary<string,IEventListener> eventListenerMap;
    
    
    public static bool isReady {
        get {
            return singleton != null;
        }
    }
        
    public static EventManager instance {
        get {
            if (!EventManager.isReady) {
                // Singleton is unitialized so dynamically make a new empty
                // gameobject to which we we can attach the EventManager script.
                // This allows for objects to use the EventManager without worrying
                // about if it is initialized and without having a static instance 
                // always hanging around.
                GameObject attachmentObj = new GameObject ("EventManager");           
                
                singleton = attachmentObj.AddComponent<EventManager> ();
            }
            // Return the new instance of EventManager that is attached
            // to the game object.
            return singleton;
        }
    }
    
    // Awake is called when the base is initialized. Start() 
    // is called before the first update. Initializing the manager 
    // makes sure the singleton is initialized before events start 
    // happening since other objects may be "started" before the eventmanager.
    void Awake ()
    {
        eventQueue = new Queue ();
        eventListenerMap = new Dictionary<string, IEventListener> ();
        
    }
    
    
    public bool RegisterListener (IEventListener listener, string eventName)
    {
                
        if (listener == null || eventName == null) {
            Debug.LogError ("RegisterLisener(): failed due to listener or event == null");
            return false;
        }
        if (eventListenerMap.ContainsKey(eventName)){
            Debug.LogWarning("Overwriting listener for " + eventName);
        }
        
        // One listener type per event! 
        eventListenerMap [eventName] = listener;
        return true;
    }
 
    
    bool HasListener(IEvent evnt){
       return eventListenerMap.ContainsKey (evnt.GetName ());
    }
    
    /// If there is a listener for evnt then relay the event to 
    /// that listener immediately. 
    public bool RelayEvent(IEvent evnt){
        if (!HasListener(evnt)){
            Debug.LogWarning ("RelayEvent(): No listener for " + evnt.GetName ());
            return false;  
        }
        DispatchEvent(evnt);
        return true;
    }
    
    
    /// If there is a listener for evnt then place the event in the queue
    /// otherwise just ignore it. 
    public bool QueueEvent (IEvent evnt)
    {
       
        if (!HasListener(evnt)){
            Debug.LogWarning ("QueueEvent(): No listener for " + evnt.GetName ());
            return false;
        }
  
        eventQueue.Enqueue (evnt);
        return true;
    }
 
    
    // Calls the listener's HandleEvent() methods for the event
    void DispatchEvent (IEvent evnt)
    { 
        string eventName = evnt.GetName ();
        IEventListener listener = eventListenerMap [eventName];
        listener.HandleEvent (evnt);
    }
    
    
    // Only process the event queue on each update.
    void Update ()
    {
 
        IEvent evnt;
        while (eventQueue.Count > 0) {
            evnt = eventQueue.Dequeue () as IEvent;
            DispatchEvent (evnt);
        }
        
    }
    
    /// 
    public void OnApplicationQuit ()
    {
        eventListenerMap.Clear();
        eventQueue.Clear();
        singleton = null;
    }
}
