using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSetter : MonoBehaviour {
	// HouseSpawner: Controller for spawning desired delivery location


	// Spawns variables:
	[Header("Spawns")]
	public float setTime;
	public float setInterval = 5;
	public int setIndex = 0;
	public static int houseLimit = 2;
	public House[] houses;
	public PizzaGuy pizzaGuy;


	void Start () {

		// Set spawn variables to starting value
		setTime = 0;
		setIndex = (int)Random.Range (0, houses.Length);
	}

	void Update () {

		setTime += Time.deltaTime;
		if(setTime > setInterval) { SetHouse(); setTime -= setInterval; }
	}

	void SetHouse() {

		if (pizzaGuy.CanHouse (houseLimit)) {
			pizzaGuy.SetHouse ();

			houses [setIndex].setDesire (true);

			setIndex = (int)Random.Range (0, houses.Length);
		}
	}
}