using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrumSpawner : MonoBehaviour {

	[SerializeField] GameObject oilDrumPrefab;
	float drumVelocity = 65.0f;

	// Use this for initialization
	void Start () {

		InvokeRepeating ("SpawnRandomRocket", 0.0f, 10.0f);
	}

	void SpawnRandomRocket()
	{
		Vector3 forwardDir = transform.TransformDirection (Vector3.forward);

		GameObject oilDrumInst = Instantiate (oilDrumPrefab, transform.position, transform.rotation) as GameObject;

		oilDrumInst.GetComponent<Rigidbody> ().velocity = drumVelocity * forwardDir;
	}
}
