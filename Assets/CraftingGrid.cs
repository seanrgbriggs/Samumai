using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CraftingGrid : MonoBehaviour {

    [System.Serializable]
    public struct Recipe {
        public IngredientType[] recipe;
        public IngredientType output;
    }

    public bool isCrafting;



    public VRTK.VRTK_SnapDropZone[] zones;
    public MeshFilter display;
    public IngredientGallery gallery;
    public PlatingMinigame miniGamePrefab;
    public Collider rope;

    public List<Recipe> recipes;
    public Recipe currentRecipe;

    // Use this for initialization
    void Start () {
		foreach(var zone in zones) {
            zone.ObjectSnappedToDropZone += (obj, e) => { if(e.snappedObject.GetComponent<Ingredient>() == null) { ((VRTK_SnapDropZone)obj).ForceUnsnap(); } };
            zone.ObjectSnappedToDropZone += UpdateCrafter;// (obj, e) => print(2);
        }
    }
	
    public void OnDetectCollision(GameObject go) {
        BeginCraft();
    }

    public void BeginCraft() {
        if(currentRecipe.output == IngredientType.NULL) { return; }

        foreach(var zone in zones) {
            var obj = zone.GetCurrentSnappedObject();
            if(obj!= null)
                Destroy(obj.GetComponent<VRTK_InteractableObject>());
        }

        var mini = Instantiate(miniGamePrefab, transform.position + Vector3.up, Quaternion.identity);
        mini.grid = this;

        isCrafting = true;
        rope.enabled = false;
    }

    public void FinishCraft() {
        isCrafting = false;
        rope.enabled = true;


        foreach(var zone in zones) {
            var obj = zone.GetCurrentSnappedObject();
            if(obj != null)
                Destroy(obj.gameObject);
        }

        Instantiate(gallery.Lookup(currentRecipe.output).prefab, display.transform.position, display.transform.rotation);

        ResetData();

    }

    public void UpdateCrafter(object zoneobj, SnapDropZoneEventArgs e) {
        foreach(var recipe in recipes) { //look at each recipe
            bool matchesRecipe = true;
            for(int i = 0; i < 9; i++) { //look at each snap zone
                var snappedObj = zones[i].GetCurrentSnappedObject();
                if(recipe.recipe[i] == IngredientType.NULL) { //if the zone should be empty

                    if( snappedObj != null) { //and it isn't, abort
                        matchesRecipe = false;
                        break;
                    }
                    
                } else { //if it shouldn't be empty
                    if(snappedObj == null || recipe.recipe[i] != snappedObj.GetComponent<Ingredient>().ingredient) { //and it is, or it's the wrong ingredient, abort
                        matchesRecipe = false;
                        break;
                    }
                }
            }

            if(matchesRecipe) { //if you're right, update the recipe data and the holo-display
                currentRecipe = recipe;
                var output = gallery.Lookup(recipe.output);
                display.mesh = output.prefab.GetComponent<MeshFilter>().sharedMesh;
                return;
            }
        }

        //if nothing matches, set everything to null
        ResetData();
    }

    void ResetData() {
        currentRecipe = new Recipe { output = IngredientType.NULL };
        display.mesh = null;
    }
}
