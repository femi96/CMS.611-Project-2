using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSpawner : MonoBehaviour {
	// PizzaSpawner: Controller for spawning pizza


	// Spawns variables:
	[Header("Spawns")]
	public float spawnTime;
	public float spawnInterval = 5;
	public int spawnIndex = 0;
	public static int pizzaLimit = 3;
	public Transform[] spawnLocations;
	public GameObject pizzaPrefab;
	public PizzaGuy pizzaGuy;


	void Start () {

		// Set spawn variables to starting value
		spawnTime = 0;
	}
	
	void Update () {

		spawnTime += Time.deltaTime;
		if(spawnTime > spawnInterval) { SpawnPizza(); spawnTime -= spawnInterval; }
	}

	void SpawnPizza() {

		if(pizzaGuy.CanPizza(pizzaLimit)) {
			pizzaGuy.SpawnedPizza();

			GameObject go = Instantiate(pizzaPrefab);
			go.transform.parent = transform;
			go.transform.position = spawnLocations[spawnIndex].position;
			go.transform.position += Vector3.up * 0.4f;
			go.GetComponent<Pizza>().pizzaGuy = pizzaGuy;

			spawnIndex += 1;
			if(spawnIndex >= spawnLocations.Length) { spawnIndex = 0; }
		}
	}
}
