﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PizzaGuy : MonoBehaviour {
	// PizzaGuy: Controller for controlling pizza guy


	// Game variables:
	[Header("Game")]
	public float time;
	public float matchTime = 100;

	public int score;
	public int pizzaCount;
	public int pizzasInPlay;

	// Movement variables:
	[Header("Movement")]
	public float speed = 4f;

	public float inputX;
	public float inputZ;

	private CharacterController moveController;
	private GameObject model;

	// Gun variables:
	[Header("Gun")]
	public GameObject currentGun;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public GameObject pelletPrefab;
	public Transform[] pelletTransforms;
	public int numBullets = 5;
	public int numPellets = 25;

	private Vector3 fallMove = new Vector3(0, -0.1f, 0);
	
	// UI variables:
	[Header("UI")]
	public Text timer;
	public Text scoreText;


	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;
	}

	void Start () {

		// Set game variables to starting value
		time = matchTime;
		score = 0;
	}
	
	void Update () {
		// Called every frame

		// Game updates
		GameUpdate();

		// Movement updates
		MoveUpdate();

		// Guns updates
		if(Input.GetKeyDown(KeyCode.G)) {
			SwitchGun();
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			Fire();
		}
	}

	// ...
	void GameUpdate() {

		time -= Time.deltaTime;
		if(time < 0) { time = 0; }

		timer.text = Mathf.CeilToInt(time).ToString();
		scoreText.text = Mathf.CeilToInt(score).ToString();
	}

	// Moves pizzaguy each frame
	void MoveUpdate() {

		inputX = Input.GetAxis("Horizontal");
		inputZ = Input.GetAxis("Vertical");

		// See if vector magnitude is >1 and limit if so

		Vector3 moveDirection = new Vector3(inputX, 0, inputZ);
		moveDirection *= speed;
		moveDirection *= Time.deltaTime;

		if(Mathf.Abs(inputX) + Mathf.Abs(inputZ) > 0.1f) { model.transform.forward = moveDirection; }

		moveDirection += fallMove;
		moveController.Move(moveDirection);
	}

	public bool CanPizza(int pizzaLimit) {
		return pizzasInPlay < pizzaLimit;
	}

	public void SpawnedPizza() {
		pizzasInPlay += 1;
	}

	public void PickUp() {
		pizzaCount += 1;
	}

	public bool CanDeliver() {
		return pizzaCount > 0;
	}

	public void Delivered() {
		pizzaCount -= 1;
		pizzasInPlay -= 1;
		score += 1;
	}

	public float DistanceTo(Vector3 otherPos) {
		return (transform.position - otherPos).magnitude;
	}

	bool CanShoot()
	{
		if (currentGun.name.Equals("Revolver"))
		{
			return numBullets > 0;
		} else
		{
			return numPellets > 0;
		}
	}
	void Fire()
	{
		if (CanShoot())
		{
			if (currentGun.name.Equals("Revolver"))
			{
				print("Creating Bullet");
				// Create the Bullet from the Bullet Prefab
				var bullet = (GameObject)Instantiate(
					bulletPrefab,
					bulletSpawn.position,
					bulletSpawn.rotation);

				// Add velocity to the bullet
				bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

				// Destroy the bullet after 2 seconds
				Destroy(bullet, 2.0f);
				numBullets -= 1;
			} else
			{
				print("Creating Pellets");
				for(int i = 0; i < pelletTransforms.Length; i++)
				{
					var bullet = (GameObject)Instantiate(
					pelletPrefab,
					pelletTransforms[i].position,
					pelletTransforms[i].rotation);

					// Add velocity to the bullet
					bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

					// Destroy the bullet after 2 seconds
					Destroy(bullet, 5.0f);
				}
				numPellets -= 1;
			}
		}
	}
	void SwitchGun()
	{
		if(currentGun.name.Equals("Revolver"))
		{
			print("Gun is now shotgun");
			GameObject nextGun = model.transform.Find("Shotgun").gameObject;
			currentGun = nextGun;
		} else
		{
			print("Gun is now Revolver");
			GameObject nextGun = model.transform.Find("Revolver").gameObject;
			currentGun = nextGun;
		}
	}
}
