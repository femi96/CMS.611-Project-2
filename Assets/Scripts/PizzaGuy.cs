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

	// Movement variables:
	[Header("Movement")]
	public float speed = 4f;

	public float inputX;
	public float inputZ;

	private CharacterController moveController;
	private GameObject model;

	private Vector3 fallMove = new Vector3(0, -0.1f, 0);
	
	// UI variables:
	[Header("UI")]
	public Text timer;

	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;
	}

	void Start () {

		// Set game variables to starting value
		time = matchTime;
	}
	
	void Update () {
		// Called every frame

		// Game updates
		GameUpdate();

		// Movement updates
		MoveUpdate();
	}

	// ...
	void GameUpdate() {

		time -= Time.deltaTime;
		if(time < 0) { time = 0; }

		timer.text = Mathf.CeilToInt(time).ToString();
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
}
