using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	// Enemy: Controller for controlling an enemy


	// Enemy variables:
	[Header("Enemy")]
	public GameObject target;

	public static float swipeDistance = 1.0f;

	// Movement variables:
	[Header("Movement")]
	public float speed = 2f;

    [Header("Sounds")]
    public AudioClip deathSound;
    public AudioClip growlSound;

    private AudioSource source;

    private CharacterController moveController;
	private GameObject model;

	private Vector3 fallMove = new Vector3(0, -5f, 0);


	void Awake () {

		// Get movement componenets
		moveController = GetComponent<CharacterController>();
		model = transform.Find("Model").gameObject;
        source = GetComponent<AudioSource>();
    }
	
	void Update () {
		// Called every frame

		// Movement updates
		MoveUpdate();

		// Enemy updates
		PizzaGuy pizzaGuy = target.GetComponent<PizzaGuy>();
		if(pizzaGuy != null) {

			float guyDist = pizzaGuy.DistanceTo(transform.position);
			if(guyDist <= swipeDistance) {

				pizzaGuy.Swipe(transform.position);
			}
		}
	}

    void Start() {
    	if(growlSound != null){ source.PlayOneShot(growlSound, 0.01f); }
    }

    // Moves enemy each frame
    void MoveUpdate() {

		Vector3 moveDirection = Vector3.zero;

		if(target != null) {
			moveDirection = target.transform.position - transform.position;
			moveDirection = moveDirection.normalized;
		}

		moveDirection *= speed;

		model.transform.forward = moveDirection;

		moveDirection += fallMove;
		moveDirection *= Time.deltaTime;
		moveController.Move(moveDirection);
	}
}
