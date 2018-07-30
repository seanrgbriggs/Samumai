using UnityEngine;

public class Ingredient : MonoBehaviour {
    public IngredientType ingredient;
    public IngredientType onChop;

    void OnCollisionEnter(Collision col) {
        var collider = col.collider;
        print(collider.name);
        if(onChop != IngredientType.NULL && collider.CompareTag("Finish")) {
            var choppedObj = Instantiate(FindObjectOfType<IngredientGallery>().Lookup(onChop).prefab, transform.position, Quaternion.identity);
            var velocity = choppedObj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            choppedObj.transform.position += velocity * Time.deltaTime;
            Destroy(gameObject);
        }
    }
}
