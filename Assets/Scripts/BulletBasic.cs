﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class BulletBasic : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {

		GameObject hit = collision.gameObject;
		Health health = hit.GetComponent<Health>();

		if(health != null) {
			health.TakeDamage(100);
		}

		Destroy(gameObject);
	}
}
