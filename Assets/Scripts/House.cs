using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

	public PizzaGuy pizzaGuy;

	public static float deliverDistance = 2.0f;

	void Update() {

		float guyDist = pizzaGuy.DistanceTo(transform.position);
		if(guyDist <= deliverDistance) {

			if(pizzaGuy.CanDeliver()) {
				pizzaGuy.Delivered();
				Destroy(gameObject);
			}
		}
	}
}
