using System.Collections;
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
	public int pizzaCount = 0;
	public int pizzasInPlay = 0;
	public int houseCount = 0;
	public int housesInPlay = 0;

	private float swipeTime;
	private float swipeTimeout = 1;

	// Movement variables:
	[Header("Movement")]
	public float speed = 4f;

	public float inputX;
	public float inputZ;

	private CharacterController moveController;
	private GameObject model;

	private Vector3 fallMove = new Vector3(0, -5f, 0);

	// Gun variables:
	[Header("Gun")]
	public Gun[] guns;
	public int currentGunIndex;
	
	// UI variables:
	[Header("UI")]
	public Text timer;
	public Text scoreText;
	public Text gunNameText;
	public Text ammoText;

	public GameObject reloadUI;
	public GameObject switchUI;

	public Text healthText;
	public GameObject healthBar;

	public GameObject[] pizzaStacks;
	public GameObject[] pizzaStacksUI;


	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;
	}

	void Start () {

		// Set game variables to starting value
		time = matchTime;
		score = 0;
		SwitchGun();
	}
	
	void Update () {
		// Called every frame

		// Game updates
		GameUpdate();

		// Movement updates
		MoveUpdate();

		// Guns updates
		if(Input.GetKeyDown(KeyCode.G)) { SwitchGun(); }
		if(Input.GetKeyDown(KeyCode.Space)) { guns[currentGunIndex].Fire(); }
		if(Input.GetKeyDown(KeyCode.R)) { guns[currentGunIndex].Reload(); }
		if(Input.GetKeyDown(KeyCode.O)) { Application.LoadLevel(Application.loadedLevel); }
	}

	// ...
	void GameUpdate() {

		time -= Time.deltaTime;
		if(time < 0) { time = 0; }

		swipeTime += Time.deltaTime;

		timer.text = Mathf.CeilToInt(time).ToString();
		scoreText.text = "$"+Mathf.CeilToInt(score*5).ToString();
		ammoText.text = guns[currentGunIndex].GetAmmoText();
		gunNameText.text = guns[currentGunIndex].gunName;

		reloadUI.SetActive(!guns[currentGunIndex].CanShoot());
		switchUI.SetActive(pizzaCount <= 0);

		int health = gameObject.GetComponent<Health>().currentHealth;
		healthText.text = health.ToString() + "%";
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(3*health, 100);

		int count = 0;
		foreach(GameObject pizzaStack in pizzaStacks) {

			count += 1;
			pizzaStack.SetActive(count <= pizzaCount);
		}
		count = 0;
		foreach(GameObject pizzaStack in pizzaStacksUI) {

			count += 1;
			pizzaStack.SetActive(count <= pizzaCount);
		}
	}

	// Moves pizzaguy each frame
	void MoveUpdate() {

		inputX = Input.GetAxis("Horizontal");
		inputZ = Input.GetAxis("Vertical");

		// See if vector magnitude is >1 and limit if so

		Vector3 moveDirection = new Vector3(inputX, 0, inputZ);
		moveDirection *= speed;

		if(Mathf.Abs(inputX) + Mathf.Abs(inputZ) > 0.1f) { model.transform.forward = moveDirection; }

		moveDirection += fallMove;
		moveDirection *= Time.deltaTime;
		moveController.Move(moveDirection);
	}

	public bool CanPizza(int pizzaLimit) {
		return pizzasInPlay < pizzaLimit;
	}

	public bool CanHouse(int houseLimit) {
		return housesInPlay < houseLimit;
	}

	public void SpawnedPizza() {
		pizzasInPlay += 1;
	}

	public void SetHouse() {
		housesInPlay += 1;
	}

	public void PickUp() {
		pizzaCount += 1;
		SwitchGun();
	}

	public bool CanDeliver() {
		return pizzaCount > 0;
	}

	public void Delivered() {
		pizzaCount -= 1;
		pizzasInPlay -= 1;
		houseCount -= 1;
		housesInPlay -= 1;
		score += 1;
	}

	public float DistanceTo(Vector3 otherPos) {
		return (transform.position - otherPos).magnitude;
	}

	void SwitchGun() {
		if(pizzaCount > 0) {

			currentGunIndex = 0;
		} else {

			currentGunIndex += 1;
			if(currentGunIndex >= guns.Length) {
				currentGunIndex = 0;
			}
		}

		foreach(Gun gun in guns) {
			if(guns[currentGunIndex] == gun) {
				gun.gameObject.SetActive(true);
			} else {
				gun.gameObject.SetActive(false);
			}
		}
	}

	public void Swipe(Vector3 pos) {

		if(swipeTime >= swipeTimeout) {

			gameObject.GetComponent<Health>().TakeDamage(35);
			swipeTime = 0;

			Vector3 knockback = transform.position - pos;
			knockback = knockback.normalized * 0.5f;
			moveController.Move(knockback);
		}
	}
}
