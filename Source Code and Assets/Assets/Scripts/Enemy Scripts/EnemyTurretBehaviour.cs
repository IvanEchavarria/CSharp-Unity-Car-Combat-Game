using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTurretBehaviour : GameManagerScript {

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject MuzzleFlashParent;
	[SerializeField] Transform  RotatorParent;
	[SerializeField] Transform  bulletSpawner;
	[SerializeField] Transform playerObj;
	[SerializeField] Transform Cannon;
	[SerializeField] float rotationSpeed = 45;
	[SerializeField] Text turretCount;
	[SerializeField] AudioClip shootSound;
	[SerializeField] Slider EnemyHealth;
	[SerializeField] GameObject explosion;


	public float healthBarLength = 50.0f;
	float health = 100;
	static int turretNumber;
	float lockAxis = 0f;
	float distanceToPlayer;
	float bulletForce;
	int numberOfBullets;
	bool  Reloading;
	bool  Fire;
	bool  seesPlayer;
	Vector3 directionVector;
	Vector3 cannonDirectionVector;
	Vector3 target;
	Quaternion rotationDestination;
	Quaternion cannonRotDestination;
	AudioSource aud;
	RaycastHit hit;


	void Awake()
	{
		
		turretNumber++;
		if(turretNumber >=difficulty)
		{
			turretNumber = difficulty;
		}
		turretCount.text = "Turrets Left: " + turretNumber ;
	}

	void Start()
	{
		Reloading = false;
		Fire = true;
		seesPlayer = false;
		MuzzleFlashParent.SetActive (false);
		numberOfBullets = 8;
		bulletForce = 240.0f;
		aud = this.GetComponent<AudioSource> ();
	}


	// Update is called once per frame
	void Update () 
	{
		Vector3 forward = bulletSpawner.TransformDirection (Vector3.forward);

		Debug.DrawRay (RotatorParent.position, forward * 100.0f, Color.red);

		if (Physics.Raycast (RotatorParent.position, forward, out hit, 100.0f)) 
		{
			if (hit.collider.tag == "Player") 
			{
				seesPlayer = true;
			} 
			else
			{
				seesPlayer = false;			
			}
		}
	
		target = playerObj.position;
		directionVector = target - transform.position;
		distanceToPlayer = directionVector.magnitude;

		if(distanceToPlayer <= 120.0f )
		{

			cannonDirectionVector = directionVector;

			directionVector.y = lockAxis;

			rotationDestination = Quaternion.LookRotation (directionVector);
			cannonRotDestination = Quaternion.LookRotation (cannonDirectionVector);

		    transform.rotation = Quaternion.RotateTowards (transform.rotation, rotationDestination, rotationSpeed * Time.deltaTime);
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);

			Cannon.rotation = Quaternion.RotateTowards (Cannon.rotation, cannonRotDestination, rotationSpeed * Time.deltaTime);
			Cannon.eulerAngles = new Vector3 (Cannon.eulerAngles.x + 0.5f,transform.eulerAngles.y, 0.0f);

			if (Fire && !Reloading && seesPlayer) 
			{
				Fire = false;

				numberOfBullets--;

				RotatorParent.Rotate (0.0f, 0.0f, 30.0f); // Actually makes the object rotate by specified float each time is called.

				GameObject bulletInst = Instantiate (bulletPrefab, bulletSpawner.position, bulletSpawner.rotation) as GameObject;

				aud.PlayOneShot (shootSound);

				Vector3 fwd = bulletSpawner.TransformDirection (Vector3.forward);

				//bulletInst.GetComponent<Rigidbody> ().AddForce (fwd * bulletForce * Time.deltaTime);

				bulletInst.GetComponent<Rigidbody> ().velocity = bulletForce * fwd;

				//Destroy (bulletInst, 5.0f);

				MuzzleFlashParent.SetActive (true);

				Invoke ("AllowFire", 0.2f);

				if (numberOfBullets <= 0) 
				{
					Reloading = true;
					Invoke("coolDown", 2.0f );
				}

			}		

		}


	}

	public void AddjustCurrentHealth(int adj) {

		health -= adj;
		EnemyHealth.value = health;

		if (health <= 0)
		{			
			health = 0;
			this.transform.parent.parent.GetComponent<BoxCollider>().enabled = false;
			EnemyHealth.value = health;
			turretNumber--;
			if(turretNumber <= 0)
			{
				SceneManager.LoadScene("GameWon");
			}
			turretCount.text = "Turrets Left: " + turretNumber ;
			GameObject explosionInst = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
			Destroy (explosionInst, 0.6f);
			Destroy (gameObject);
		}

	}


	void AllowFire()
	{
		MuzzleFlashParent.SetActive (false);
		Fire = true;
	}

	void coolDown()
	{
		numberOfBullets = 8;
		Reloading = false;
	}

	public void resetTurretCount()
	{
		turretNumber = 0;
	}

	public int getTurretNumber()
	{
		return turretNumber;
	}
}
