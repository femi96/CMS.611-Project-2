using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PizzaGuy : MonoBehaviour {
	// PizzaGuy: Controller for controlling pizza guy


	// Game variables:
	[Header("Game")]
	public GameState gameState;
	public GameState gameStatePrevious;

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
	
	[Header("GameStateUI")]
	public GameObject canvas;

	private GameObject playingUI;
	private GameObject pausedUI;
	private GameObject mainUI;
	private GameObject defeatUI;
	private GameObject victoryUI;
	
	[Header("EndUI")]
	public Text victoryText;
	public Text defeatText;

	[Header("PlayUI")]
	public Text timer;
	public Text scoreText;
	public Text gunNameText;
	public Text ammoText;

	// public GameObject reloadUI;
	public GameObject switchUI;

	public Text healthText;
	public GameObject healthBar;

	public GameObject[] pizzaStacks;
	public GameObject[] pizzaStacksUI;

	[Header("Sounds")]
	public AudioClip pizzaUpSound;
	public AudioClip pizzaDownSound;

	private AudioSource source;

	[Header("TutorialUI")]
	private GameObject tutor;

	private GameObject tutorW;
	private GameObject tutorA;
	private GameObject tutorS;
	private GameObject tutorD;
	private GameObject tutorSpace;
	private GameObject tutorPizza;
	private GameObject tutorDeliver;


	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;

		playingUI = canvas.transform.Find("PlayingUI").gameObject;
		pausedUI = canvas.transform.Find("PausedUI").gameObject;
		mainUI = canvas.transform.Find("MainUI").gameObject;
		defeatUI = canvas.transform.Find("DefeatUI").gameObject;
		victoryUI = canvas.transform.Find("VictoryUI").gameObject;

		source = GetComponent<AudioSource>();

		tutor = playingUI.transform.Find("TutorialUI").gameObject;

		tutorW = tutor.transform.Find("W").gameObject;
		tutorA = tutor.transform.Find("A").gameObject;
		tutorS = tutor.transform.Find("S").gameObject;
		tutorD = tutor.transform.Find("D").gameObject;
		tutorSpace = tutor.transform.Find("Space").gameObject;
		tutorPizza = tutor.transform.Find("Pizza").gameObject;
		tutorDeliver = tutor.transform.Find("Deliver").gameObject;
	}

	void Start () {

		// Set game variables to starting value
		gameState = GameState.Main;
		gameStatePrevious = GameState.Defeat;

		time = matchTime;
		score = 0;
		SwitchGun();
	}
	
	void Update () {
		// Called every frame

		// Game updates
		if(gameStatePrevious != gameState) {
			GameStateChanged();
		}

		GameUpdate();

		// Movement updates
		MoveUpdate();

		// Input updates
		if(gameState == GameState.Playing) {
			if(Input.GetKeyDown(KeyCode.G)) { SwitchGun(); }
			if(Input.GetKeyDown(KeyCode.Space)) { tutorSpace.SetActive(false); guns[currentGunIndex].Fire(); }
			if(Input.GetKeyDown(KeyCode.R)) { guns[currentGunIndex].Reload(); }
		}

		if(Input.GetKeyDown(KeyCode.O)) { Application.LoadLevel(Application.loadedLevel); }
		if(Input.GetKeyDown(KeyCode.P)) { TogglePaused(); }
		if(Input.GetKeyDown(KeyCode.Space) && gameState == GameState.Main) { gameState = GameState.Playing; }
	}

	// ...
	void GameUpdate() {

		time -= Time.deltaTime;
		if(time < 0) { time = 0; gameState = GameState.Victory; }

		swipeTime += Time.deltaTime;

		timer.text = Mathf.CeilToInt(time).ToString();
		scoreText.text = "$"+Mathf.CeilToInt(score*5).ToString();
		ammoText.text = guns[currentGunIndex].GetAmmoText();
		gunNameText.text = guns[currentGunIndex].gunName;

		// reloadUI.SetActive(!guns[currentGunIndex].isEmpty());
		switchUI.SetActive(pizzaCount <= 0);

		int health = gameObject.GetComponent<Health>().currentHealth;
		healthText.text = health.ToString() + "%";
		healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(3*health, 100);

		if(health <= 0) { gameState = GameState.Defeat; }

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

		if(inputX > 0) { tutorD.SetActive(false); }
		if(inputX < 0) { tutorA.SetActive(false); }
		if(inputZ > 0) { tutorW.SetActive(false); }
		if(inputZ < 0) { tutorS.SetActive(false); }

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
		tutorPizza.SetActive(false);
		pizzaCount += 1;
		source.PlayOneShot(pizzaUpSound, 0.5f);
		SwitchGun();
	}

	public bool CanDeliver() {
		return pizzaCount > 0;
	}

	public void Delivered() {
		tutorDeliver.SetActive(false);
		pizzaCount -= 1;
		pizzasInPlay -= 1;
		houseCount -= 1;
		housesInPlay -= 1;
		score += 1;
		source.PlayOneShot(pizzaDownSound, 0.5f);
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

	void TogglePaused() {

		if(gameState == GameState.Playing) {
			gameState = GameState.Paused;
		}
		if(gameState == GameState.Paused) {
			gameState = GameState.Playing;
		}
	}

	void GameStateChanged() {
		gameStatePrevious = gameState;
		if(gameState == GameState.Playing) {
			Time.timeScale = 1;
		} else {
			Time.timeScale = 0;
		}

		playingUI.SetActive(gameState == GameState.Playing || gameState == GameState.Paused);
		pausedUI.SetActive(gameState == GameState.Paused);

		mainUI.SetActive(gameState == GameState.Main);
		defeatUI.SetActive(gameState == GameState.Defeat);
		victoryUI.SetActive(gameState == GameState.Victory);


		victoryText.text = "Congrats!\nYou survived with $"+(score*5).ToString()+" :D";
		defeatText.text = "R.I.P.\nYou died with $"+(score*5).ToString()+" :(";
	}
}
