using UnityEngine;
using System.Collections;

public class DestroyOnProjectile : MonoBehaviour {
 
    const string PROJECTILE_TAG = "Projectile";
    
    public bool destroyesProjectile = true;
    
    void OnTriggerEnter(Collider other){
        if (!other.gameObject.CompareTag(PROJECTILE_TAG))
            return;
        if (destroyesProjectile)
            Destroy(other.gameObject);
        
        Destroy(gameObject);
        
    }
}
