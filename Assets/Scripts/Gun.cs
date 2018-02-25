using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform[] bulletSpawns;

	public float bulletSpeed;

	public int capacity;
	public int ammo;

	public float reloadTime;
	public float time;

	void Start () {
		ammo = capacity;
	}
	
	void Update () {
		
	}
	
	bool CanShoot() {
		return ammo > 0;
	}

	public void Fire() {

		if(CanShoot()) {
			foreach(Transform bulletSpawn in bulletSpawns) {

				GameObject bullet = Instantiate(
						bulletPrefab,
						bulletSpawn.position,
						bulletSpawn.rotation);

				// Add velocity to the bullet
				bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

				// Destroy the bullet after 2 seconds
				Destroy(bullet, 2.0f);
				ammo -= 1;
			}
		}
	}

	public void Reload() {
		ammo = capacity;
	}
}
