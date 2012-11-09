using UnityEngine;
using System.Collections;

public class DestroyOnProjectile : MonoBehaviour {
 
    const string PROJECTILE_TAG = "Projectile";
    
    void OnTriggerEnter(Collider other){
        if (!other.gameObject.CompareTag(PROJECTILE_TAG))
            return;
        
        Destroy(gameObject);
        
    }
}
