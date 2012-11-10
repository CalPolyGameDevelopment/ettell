using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveCompletedEvent : EventObject{}

public class Wave : MonoBehaviour {
 
    public GameObject spawnObject;
    public int spawnCount;
 
    public Vector3 force;
    
    private bool isActive = false;
    private List<GameObject> spawnList;
    
    public enum SpawnPattern{
        Linear
    }
    
    public void Spawn(GameObject parentObject){
        spawnList = new List<GameObject>();
        Vector3 spawnPoint = gameObject.transform.parent.position;
        float side = 1.0f;
        for(int i = 1; i <= spawnCount; i++){
            
            GameObject spawn = Instantiate(spawnObject) as GameObject;
            
            //spawn.transform.parent = parentObject.transform;
            
            spawn.transform.rigidbody.position = 
                new Vector3(spawnPoint.x + (i*5.0f*side), spawnPoint.y, spawnPoint.z);

            spawn.name = string.Format("({0}) {1}", i, spawnObject);
            spawnList.Add(spawn);
            spawn.rigidbody.AddForce(force);
            side *= -1;
        }
        isActive = true;
    }
    
    bool hasActiveSpawns{
        get{
            var spawns = from spawn in spawnList 
                         where spawn != null select spawn;
            return spawns.Count<GameObject>() > 0;
        }
    }
    
    void Update(){
  
        if (!isActive)
            return;
        
        if (hasActiveSpawns){
            return;
        }
        else{
            EventManager.instance.RelayEvent(new WaveCompletedEvent());
            isActive = false;
        }
        
       
    }
    
    
}

