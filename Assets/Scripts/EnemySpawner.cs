using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	// EnemySpawner: Controller for spawning enemies


	// Spawns variables:
	[Header("Spawns")]
	public float spawnTime;
	public float spawnInterval = 2;
	public int spawnIndex = 0;
	public Transform[] spawnLocations;
	public GameObject enemyPrefab;
	public GameObject pizzaGuy;


	void Start () {

		// Set spawn variables to starting value
		spawnTime = 0;
	}
	
	void Update () {

		spawnTime += Time.deltaTime;
		if(spawnTime > spawnInterval) { SpawnEnemy(); spawnTime -= spawnInterval; }
	}

	void SpawnEnemy() {

		GameObject go = Instantiate(enemyPrefab);
		go.transform.parent = transform;
		go.transform.position = spawnLocations[spawnIndex].position;
		go.GetComponent<Enemy>().target = pizzaGuy;

		spawnIndex = (int) Random.Range (0, spawnLocations.Length);
	}
}
