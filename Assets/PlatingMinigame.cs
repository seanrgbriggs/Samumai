using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatingMinigame : MonoBehaviour {

    public ArrowPrompt arrowPrefab;
    public int count = 5;
    public CraftingGrid grid;

    private ArrowPrompt currentArrow;
    [SerializeField] private float radius = 2f;

    // Use this for initialization
    void Start () {
        SpawnArrow();		
	}
	
    public void OnArrowSlash() {
        count--;
        if(count > 0) {
            SpawnArrow();
        } else {
            grid.FinishCraft();
            Destroy(gameObject);
        }
    }

    private void SpawnArrow() {

        var position = transform.position + (Vector3) ( radius * Random.insideUnitCircle);
        var rotation = Quaternion.Euler(Vector3.forward * Random.Range(0,360));

        currentArrow = Instantiate(arrowPrefab, position, rotation, transform);
        currentArrow.toNotify = gameObject;

    }
}
