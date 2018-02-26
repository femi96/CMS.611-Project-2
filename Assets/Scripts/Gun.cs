using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform[] bulletSpawns;

    [Header("Sounds")]
    public AudioClip shotSound;
    public AudioClip reloadSound;

    private AudioSource source;

    public string gunName;

	public float bulletSpeed;

	public int capacity;
	public int ammo;

	public float reloadTime;
	public float time;

	void Start() {
		ammo = capacity;
        source = GetComponent<AudioSource>();
    }
	
	void Update() {
		
	}
	
	public bool CanShoot() {
		return !source.isPlaying && isEmpty();
	}

	public bool isEmpty() {
		return ammo > 0;
	}

	public void Reload() {
        ammo = capacity;
        source.PlayOneShot(reloadSound, 0.5f);
    }

	public string GetAmmoText() {
		return ""+ammo+"/"+capacity;
	}

	public void Fire() {

		if(CanShoot()) {
			foreach(Transform bulletSpawn in bulletSpawns) {

				GameObject bullet = Instantiate(
						bulletPrefab,
						bulletSpawn.position,
						bulletSpawn.rotation);

                // Add velocity to the bullet
                source.PlayOneShot(shotSound, 0.5f);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

				// Destroy the bullet after 2 seconds
				Destroy(bullet, 2.0f);
				ammo -= 1;
			}
		}
	}
}
