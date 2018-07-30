using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGallery : MonoBehaviour {
    
    [System.Serializable]
    public struct IngredientData {
        public IngredientType ingredient;
        public Ingredient prefab;
    }

    public IngredientData[] ingredients;

    public IngredientData Lookup(IngredientType type) {
        foreach(var ing in ingredients) {
            if(ing.ingredient == type) return ing;
        }

        return new IngredientData { ingredient = IngredientType.NULL };
    }

}
