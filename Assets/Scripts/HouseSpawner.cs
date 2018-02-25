using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour {
	// HouseSpawner: Controller for spawning desired delivery location


	// Spawns variables:
	[Header("Spawns")]
	public float spawnTime;
	public float spawnInterval = 5;
	public int spawnIndex = 0;
	public static int houseLimit = 1;
	public Transform[] spawnLocations;
	public GameObject housePrefab;
	public PizzaGuy pizzaGuy;


	void Start () {

		// Set spawn variables to starting value
		spawnTime = 0;
	}

	void Update () {

		spawnTime += Time.deltaTime;
		if(spawnTime > spawnInterval) { SpawnHouse(); spawnTime -= spawnInterval; }
	}

	void SpawnHouse() {

		if (pizzaGuy.CanHouse (houseLimit)) {
			pizzaGuy.SpawnedHouse ();

			GameObject go = Instantiate (housePrefab);
			go.transform.parent = transform;
			go.transform.position = spawnLocations [spawnIndex].position;
			go.transform.position += Vector3.up * 0.4f;
			go.GetComponent<House> ().pizzaGuy = pizzaGuy;

			spawnIndex = (int)Random.Range (0, spawnLocations.Length);
		}
	}
}