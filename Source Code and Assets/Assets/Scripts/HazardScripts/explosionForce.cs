using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionForce : MonoBehaviour {

	[SerializeField] GameObject explosionAnimation;
	[SerializeField] float hazzardMainPower;
	[SerializeField] float hazzardMainDamage;
	AudioSource aud;
	Rigidbody rb;
	Rigidbody thisRB;
	float power = 15000.0f;
	float thisRepelPower = 20f;
	float radius = 5.0f;
	float upForce = 1.0f;

	// Use this for initialization
	void Start () {

		aud = this.GetComponent<AudioSource> ();
		thisRB = this.GetComponent<Rigidbody> ();
	}



	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {
			
			rb = other.gameObject.GetComponent<Rigidbody> ();
			rb.AddExplosionForce (power + hazzardMainPower, transform.position, radius, upForce, ForceMode.Impulse);
			other.gameObject.GetComponent<PlayerUI>().damage(hazzardMainDamage);

			thisRB.AddExplosionForce (thisRepelPower, transform.position, radius, upForce, ForceMode.Impulse);
			GameObject explosionInst = Instantiate (explosionAnimation, transform.position, transform.rotation) as GameObject;

			aud.PlayOneShot (aud.clip);
			Destroy (explosionInst, 0.5f);
			Destroy (gameObject, 0.8f);
		} 
		else if (this.gameObject.tag == "rocket") 
		{
			GameObject explosionInst = Instantiate (explosionAnimation, transform.position, transform.rotation) as GameObject;
			aud.PlayOneShot (aud.clip);
			thisRB.AddExplosionForce (thisRepelPower, transform.position, radius, upForce, ForceMode.Impulse);
			Destroy (explosionInst, 0.5f);
			Destroy (gameObject, 0.8f);
		} 
		else if (this.gameObject.tag == "OilDrum" && other.gameObject.tag != "ArenaTerrain" ) 
		{			
			GameObject explosionInst = Instantiate (explosionAnimation, transform.position, transform.rotation) as GameObject;
			aud.PlayOneShot (aud.clip);
			thisRB.AddExplosionForce (thisRepelPower + 3000.0f, transform.position, radius, upForce + 1000.0f, ForceMode.Impulse);
			Destroy (explosionInst, 0.5f);
			Destroy (gameObject, 0.8f);

		}

	}
}
