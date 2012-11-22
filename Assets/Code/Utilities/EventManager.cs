using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public interface IEventListener {
    bool HandleEvent(IEvent evt);
}


public interface IEvent {
    string GetName();
    object GetData();
}

/// <summary>
/// Base Event Object
/// </summary>
public class EventObject : IEvent {
    private string eventName;
    private object eventData;
	
    public EventObject(object data) {
        eventName = this.GetType().ToString();
        eventData = data;
    }

    public EventObject() {
        eventName = this.GetType().ToString();
        eventData = null;
    }

    public virtual string GetName() {
        return eventName;
    }
    public virtual object GetData() {
        return eventData;
    }

}

/// EventManager - A stupid simple event manager class. 
/// Register a listener for a specific event type.
/// 
/// Thoughts:
///  1. The primary extention for this class would be to make 
///     the eventListenerMap handle multiple listeners for the same
///     event type. This is not needed for the current usage.
public class EventManager : MonoBehaviour {

    private static EventManager singleton = null;

    // event-name => event listerer class instance
    private Dictionary<string, IEventListener> eventListenerMap;


    public static bool isReady {
        get {
            return singleton != null;
        }
    }
	
	/// <summary>
	/// Get the EventManager singleton safely.
	/// </summary>
    public static EventManager instance {
        get {
            if (!EventManager.isReady) {
                // Singleton is unitialized so dynamically make a new empty
                // gameobject to which we we can attach the EventManager script.
                // This allows for objects to use the EventManager without worrying
                // about if it is initialized and without having a static instance 
                // always hanging around.
                GameObject attachmentObj = new GameObject("EventManager");

                singleton = attachmentObj.AddComponent<EventManager>();
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
    void Awake() {
        eventListenerMap = new Dictionary<string, IEventListener>();
    }


    public bool RegisterListener(IEventListener listener, string eventName) {

        if (listener == null || eventName == null) {
            Debug.LogError("RegisterLisener(): failed due to listener or event == null");
            return false;
        }
        if (eventListenerMap.ContainsKey(eventName)) {
            Debug.LogWarning("Overwriting listener for " + eventName);
        }

        // One listener type per event! 
        eventListenerMap[eventName] = listener;
        return true;
    }


    bool HasListener(IEvent evnt) {
        return eventListenerMap.ContainsKey(evnt.GetName());
    }
	
    /// <summary>
    /// If there is a listener for evnt then relay the event to 
    /// that listener immediately. 
    /// </summary>
	public bool RelayEvent(IEvent evnt) {
        if (!HasListener(evnt)) {
            Debug.LogWarning("RelayEvent(): No listener for " + evnt.GetName());
            return false;
        }
        DispatchEvent(evnt);
        return true;
    }


    // Calls the listener's HandleEvent() methods for the event
    void DispatchEvent(IEvent evnt) {
        string eventName = evnt.GetName();
        IEventListener listener = eventListenerMap[eventName];
        listener.HandleEvent(evnt);
    }



    public void OnApplicationQuit() {
        eventListenerMap.Clear();
        singleton = null;
    }
}
