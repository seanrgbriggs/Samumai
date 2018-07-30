using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPrompt : MonoBehaviour {

    public enum SlashStages {
        NeitherHit, LeftHit
    }

    public GameObject left;
    public GameObject right;
    public SlashStages slashStage;

    public GameObject toNotify;

    public float slashDuration = 0.1f;
    private float elapsedSlashTime = 100;

	// Update is called once per frame
	void Update () {
		if(elapsedSlashTime < slashDuration) {
            elapsedSlashTime += Time.deltaTime;
            if(elapsedSlashTime > slashDuration) {
                slashStage = SlashStages.NeitherHit;
            }
        }
	}

    public void EndMe() {
        var top = transform.GetChild(0);
        var bot = transform.GetChild(1);
        top.parent = bot.parent = null;

        top.gameObject.AddComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 5;
        bot.gameObject.AddComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 5;

        if(toNotify)
            toNotify.SendMessage("OnArrowSlash", SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }

    public void OnDetectCollision(GameObject detector) {
        if(slashStage == SlashStages.NeitherHit) {
            if(detector == left) {
                slashStage = SlashStages.LeftHit; //slash has begun
                elapsedSlashTime = 0;
            }
        } else if(slashStage == SlashStages.LeftHit) {
            if(detector == right) {
                EndMe(); //slash successful
            }else if(detector == left) {
                slashStage = SlashStages.NeitherHit; //slash unsuccessful
            }
        }
    }
}
