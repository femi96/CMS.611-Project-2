using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour {

	public PizzaGuy pizzaGuy;

	public static float pickUpDistance = 0.5f;

	void Update() {

		float guyDist = pizzaGuy.DistanceTo(transform.position);
		if(guyDist <= pickUpDistance) {

			pizzaGuy.PickUp();
			Destroy(gameObject);
		}
	}
}
