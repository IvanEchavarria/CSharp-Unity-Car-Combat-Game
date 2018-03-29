using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour {

	[SerializeField] GameObject[] rocketArrSpawners;
	[SerializeField] GameObject Rocket;
	float rocketVelocity = 40f;

	// Use this for initialization
	void Start () {
		
		InvokeRepeating ("SpawnRandomRocket", 0.0f, 4.0f);
	}
	
	void SpawnRandomRocket()
	{
		Vector3 forwardDir = transform.TransformDirection (Vector3.forward);

		for (int i = 0; i < rocketArrSpawners.Length; i++) 
		{
			GameObject RockerInst = Instantiate (Rocket, rocketArrSpawners[i].transform.position, rocketArrSpawners[i].transform.rotation) as GameObject;

			RockerInst.GetComponent<Rigidbody> ().velocity = rocketVelocity * forwardDir;
		}
	}
}
