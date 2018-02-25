using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public const int maxHealth = 100;
	public int currentHealth = maxHealth;

	public bool destroyOnDeath;
	public bool dead = false;

	float regenTime = 0;

	void Update() {

		if(!dead) {
			regenTime += Time.deltaTime;
			if(regenTime > 2) {
				currentHealth += 1;
				if(currentHealth > maxHealth) {
					currentHealth = maxHealth;
				}
				regenTime -= 1;
			}
		}
	}

	public void TakeDamage(int amount) {

		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			regenTime = 0;
			dead = true;

			if(destroyOnDeath) {
				Destroy(gameObject);
			}
		}
	}
}