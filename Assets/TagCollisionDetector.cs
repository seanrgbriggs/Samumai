using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCollisionDetector : MonoBehaviour {

    public GameObject messageTarget; //The game object notified when the tagged object collides with this
    public string requiredTag; //The tag to detect collisions with
    
    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag(requiredTag)) {
            messageTarget.SendMessage("OnDetectCollision", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

}
