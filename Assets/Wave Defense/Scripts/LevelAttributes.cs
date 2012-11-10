using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BoxCollider))]
public class LevelAttributes : MonoBehaviour {


    public bool destroyObjectsOnCollision = false;

    void OnTriggerExit(Collider other) {
        if (destroyObjectsOnCollision && !other.CompareTag("Player"))
            Destroy(other.gameObject);

    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }

}
