using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cam : MonoBehaviour {
	// Cam: Controls camera behavior

	public GameObject focus;

	void LateUpdate () {

		transform.LookAt(focus.transform);
	}
}
