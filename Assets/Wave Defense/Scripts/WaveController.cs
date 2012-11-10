using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An event for when all of the waves have been spawned
/// and their components destroyed.
/// </summary>
public class WaveLevelCompletedEvent : EventObject { }

/// <summary>
/// Wave Controller
///
/// Manages mulitple spawn waves.
/// </summary>
public class WaveController : MonoBehaviour, IEventListener {


    public GameObject[] waves;

    // The length of "waves"
    private int waveCount;

    // The index of the inprogress wave
    private int waveIndex = 0;


    public bool HasMoreWaves() {
        return waveIndex < waveCount;
    }

    // Instantiate and return the next wave if it exists.
    // Otherwise, return null.
    Wave GetNextWave() {

        if (!HasMoreWaves())
            return null;

        GameObject waveObect = waves[waveIndex];
        GameObject clone = Instantiate(waveObect) as GameObject;

        clone.transform.parent = gameObject.transform;
        Wave nextWave = clone.GetComponent<Wave>();

        waveIndex++;

        return nextWave;
    }


    void SpawnNextWave() {

        Wave nextWave = GetNextWave();

        if (nextWave != null) {
            // The wave exists so go ahead and spawn it.
            Debug.Log("Spawning Wave " + waveIndex.ToString());
            nextWave.Spawn(this.gameObject);
        }
        else {
            // null means no more waves so go ahead and
            // send out an event letting other obejcts know.
            Debug.Log("Waves Complete!");
            EventManager.instance.RelayEvent(new WaveLevelCompletedEvent());
        }

    }

    // This class only listens for WaveCompletedEvents in which case
    // we spawn the next wave.
    bool IEventListener.HandleEvent(IEvent evnt) {
        SpawnNextWave();

        return true;
    }


    void Start() {

        EventManager.instance.RegisterListener(this, typeof(WaveCompletedEvent).ToString());

        waveCount = waves.GetLength(0);

        if (waveCount == 0) {
            Debug.LogWarning("No Waves specified!");
            return;
        }
        SpawnNextWave();

    }



}
