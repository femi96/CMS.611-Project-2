using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	// Enemy: Controller for controlling an enemy


	// Enemy variables:
	[Header("Enemy")]
	public GameObject target;

	// Movement variables:
	[Header("Movement")]
	public float speed = 2f;

	private CharacterController moveController;
	private GameObject model;

	private Vector3 fallMove = new Vector3(0, -0.1f, 0);


	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;
	}

	void Start () {}
	
	void Update () {
		// Called every frame

		// Movement updates
		MoveUpdate();
	}

	// Moves enemy each frame
	void MoveUpdate() {

		Vector3 moveDirection = Vector3.zero;

		if(target != null) {
			moveDirection = target.transform.position - transform.position;
			moveDirection = moveDirection.normalized;
		}

		moveDirection *= speed;
		moveDirection *= Time.deltaTime;

		model.transform.forward = moveDirection;

		moveDirection += fallMove;
		moveController.Move(moveDirection);
	}
}
