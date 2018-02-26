using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

	public PizzaGuy pizzaGuy;

	public static float deliverDistance = 1.5f;

	public bool OnTimer = false;
	public bool wantPizza = false;
	public float pizzaDesire;
	public float pizzaDesireLimit = 5;

	public GameObject porchLight;

	public void setDesire(bool desire){
		wantPizza = desire;
	}

	void Start() {
		pizzaDesire = -Random.Range(-5, 5);
	}

	void Update() {

		if (OnTimer) {
			pizzaDesire += Time.deltaTime;
			wantPizza = pizzaDesire >= pizzaDesireLimit;
		}

		porchLight.SetActive(wantPizza);

		float guyDist = pizzaGuy.DistanceTo(transform.position);
		if(guyDist <= deliverDistance) {
			TryDeliverPizza();
		}
	}

	// Try is bad but whatever
	void TryDeliverPizza() {

		if(pizzaGuy.CanDeliver() && wantPizza) {

			pizzaGuy.Delivered();
			wantPizza = false;
			pizzaDesire = -Random.Range(0, 10);
		}

	}
}
