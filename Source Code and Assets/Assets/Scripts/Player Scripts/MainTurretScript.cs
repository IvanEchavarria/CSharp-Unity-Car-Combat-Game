using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTurretScript : MonoBehaviour {

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject muzzleFlashLeft;
	[SerializeField] GameObject muzzleFlashRight;
	[SerializeField] Transform bulletspawnerLeft;
	[SerializeField] Transform bulletspawnerRight;
	[SerializeField] float bulletForce;
	[SerializeField] float turreRotationSpeed;
	[SerializeField] AudioClip shootSound;
	AudioSource aud;


	float gunRotCenter = 0f;
	float gunRotRangeUp = 3.0f;
	float gunRotRangeDown = 0f;
	float gunRotval = 0.05f;

	bool fireRate;

	// Use this for initialization
	void Start () {

		fireRate = true;
		turreRotationSpeed = 40f;
		muzzleFlashLeft.SetActive (false);
		muzzleFlashRight.SetActive (false);
		aud = this.GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		gunsMovement ();
		shootBullets ();
	}

	void gunsMovement()
	{
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			if(gunRotCenter < gunRotRangeDown)
			{
				gunRotCenter += gunRotval;
				this.transform.Rotate (Vector3.right * -turreRotationSpeed * Time.deltaTime , Space.Self);
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			if (gunRotCenter > -gunRotRangeUp) 
			{	
				gunRotCenter -= gunRotval;			
				this.transform.Rotate (Vector3.right * turreRotationSpeed * Time.deltaTime, Space.Self);
			}

		}

	}

	void shootBullets()
	{
		if (Input.GetKey (KeyCode.Space) && fireRate) 
		{
			aud.PlayOneShot (shootSound);

			GameObject bulletInstLeft = Instantiate (bulletPrefab, bulletspawnerLeft.position, bulletspawnerLeft.rotation) as GameObject;
			Vector3 forwardDirection = bulletspawnerLeft.TransformDirection (Vector3.forward);
			bulletInstLeft.GetComponent<Rigidbody> ().AddForce (forwardDirection * bulletForce * Time.deltaTime);
			Destroy (bulletInstLeft, 4f);

			GameObject bulletInstRight = Instantiate (bulletPrefab, bulletspawnerRight.position, bulletspawnerRight.rotation) as GameObject;
			Vector3 forwardDir = bulletspawnerRight.TransformDirection (Vector3.forward);
			bulletInstRight.GetComponent<Rigidbody> ().AddForce (forwardDir * bulletForce * Time.deltaTime);
			Destroy (bulletInstRight, 4f);

			fireRate = false;
			muzzleFlashLeft.SetActive (true);
			muzzleFlashRight.SetActive (true);

			Invoke ("allowFire", 0.2f);
		}
	}

	void allowFire()
	{
		muzzleFlashLeft.SetActive (false);
		muzzleFlashRight.SetActive (false);
		fireRate = true;
	}

}
