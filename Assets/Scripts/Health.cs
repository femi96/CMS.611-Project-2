using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public const int maxHealth = 100;
	public int currentHealth = maxHealth;

	public bool destroyOnDeath;
	public bool dead = false;

	public void TakeDamage(int amount) {
		
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			dead = true;

			if(destroyOnDeath) {
				Destroy(gameObject);
			}
		}
	}
}
