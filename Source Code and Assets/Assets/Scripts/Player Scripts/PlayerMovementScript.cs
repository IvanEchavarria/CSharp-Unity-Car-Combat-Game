using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {


	[SerializeField] Transform fLWheel;
	[SerializeField] Transform fRWheel;
	[SerializeField] Transform rLWheel;
	[SerializeField] Transform rRWheel;
	[SerializeField] float forceFactor;
	[SerializeField] float rotationSpeed;
	[SerializeField] WheelCollider frw;
	[SerializeField] WheelCollider flw;
	[SerializeField] WheelCollider rrw;
	[SerializeField] WheelCollider rlw;
	[SerializeField] AudioClip [] CarSounds;


	AudioSource aud;
	AudioSource skid;
	AudioSource honk;

	WheelCollider[] wheelsArr= new WheelCollider[4];
	GameObject[] concreteTurrets;
	GameObject[] iceTurrets;
	GameObject[] grassTurrets;
	GameObject[] sandTurrets;

	float wheelRotationSpeed;
	float torquePower;
	float brakeForce;
	float idleBreakForce;
	float rotationSteer;
	float maxRotationSteer;
	float horsePower;
	float reverseForce;
	bool reverse;
	bool leftTurn;
	bool rightTurn;

	// Sound Variables
	bool turnCarOnSound;
	bool playRevvingSound;
	bool accelerationSound;
	bool collisionRestAcceleration;
	bool breakingSound;

	Quaternion leftWheelOriginalRot;
	Quaternion rightWheelOriginalRot;
	Vector3 leftWheelRotation;
	Vector3 rightWheelRotation;
	Vector3 leftWheelRotationRight;
	Vector3 rightWheelRotationRight;
	Vector3 leftWheelRotationLeft;
	Vector3 rightWheelRotationLeft;

	WheelHit hitRight;
	WheelHit hitLeft;

	Rigidbody rb;

	void Awake()
	{
		concreteTurrets = GameObject.FindGameObjectsWithTag ("ConcreteTurret");
		iceTurrets = GameObject.FindGameObjectsWithTag ("IceTurret");
		grassTurrets = GameObject.FindGameObjectsWithTag ("GrassTurret");
		sandTurrets = GameObject.FindGameObjectsWithTag ("SandTurret");
		aud = this.GetComponent<AudioSource> ();
	}
	                        
	// Use this for initialization
	void Start () {

		Debug.Log (concreteTurrets.Length);

		wheelsArr[0] = frw;
		wheelsArr[1] = flw;
		wheelsArr[2] = rrw;
		wheelsArr[3] = rlw;

		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0f, -0.5f, 0.3f);
		wheelRotationSpeed = 400.0f;
		torquePower = 1500.0f;
		idleBreakForce = 375.0f;
		brakeForce = 0.0f;
		rotationSteer = 0.0f;
		maxRotationSteer = 45.0f;
		horsePower = 0.0f;
		reverse = false;
		leftTurn = false;
		rightTurn = false;
		turnCarOnSound = true;
		leftWheelOriginalRot = fLWheel.transform.rotation;
		rightWheelOriginalRot = fRWheel.transform.rotation;
		forceFactor = 300f;
		reverseForce = 50f;
		rotationSpeed = 40;


		//Sound Management
		aud.clip = CarSounds [0];
		aud.Play ();
		playRevvingSound = false;
		accelerationSound = false;
		breakingSound = false;
		collisionRestAcceleration = false;
		skid = gameObject.AddComponent<AudioSource>();
		skid.clip = CarSounds [2];
		honk = gameObject.AddComponent<AudioSource>();
		honk.clip = CarSounds [3];
		//leftWheelRotation = new Vector3(fLWheel.transform.localRotation.x,fLWheel.transform.localRotation.y,fLWheel.transform.localRotation.z);
		//rightWheelRotation = new Vector3(fRWheel.transform.localRotation.x,fRWheel.transform.localRotation.y,fRWheel.transform.localRotation.z);
		Invoke("setRevvingsound", 1.8f);
	}
	
	// Update is called once per frame
	void Update () 
	{		
		rightTurn = false;
		leftTurn = false;
		playerMovement ();

		if (playRevvingSound) 
		{		
			playRevvingSoundAtTime ();
		}

		if (accelerationSound) 
		{
			PlayAccelerationSoundAtTime ();
		}

		if (breakingSound) 
		{
			PlayBreakingSoundAtTime ();
		}
	}

	void playRevvingSoundAtTime()
	{	
		if (aud.time >= 2.3f) 
		{
			aud.time = 1.9f;
		}
	}

	void PlayAccelerationSoundAtTime()
	{
		if (aud.time >= 30.0f) 
		{
			aud.time = 23.9f;
		}

		if (rb.velocity.magnitude < 0.04f) 
		{
			aud.time = 6.0f;
		}
	}

	void PlayBreakingSoundAtTime()
	{
		if (aud.time > 3.8f) 
		{
			aud.time = 0.01f;
		}
	}

	void setRevvingsound()
	{
		breakingSound = false;
		accelerationSound = false;
		aud.clip = CarSounds [0];
		aud.time = 1.8f;
		aud.Play ();
		playRevvingSound = true;
		turnCarOnSound = false;
	}

	void setAccelerationSound()
	{
		breakingSound = false;
		playRevvingSound = false;
		aud.clip = CarSounds [1];
		aud.time = 6.0f;
		aud.Play ();
		accelerationSound = true;
	}

	void setBreakSound()
	{
		accelerationSound = false;
		playRevvingSound = false;
		aud.clip = CarSounds [2];
		aud.time = 0.1f;
		aud.Play ();
		breakingSound = true;
	}

	void OnCollisionEnter(Collision other)
	{
		collisionRestAcceleration = true;
		if(!playRevvingSound)
			setRevvingsound ();
	}

	void OnCollisionStay(Collision other)
	{
		collisionRestAcceleration = true;
		if(!playRevvingSound)
			setRevvingsound ();
	}

	void OnCollisionExit(Collision info)
	{
		collisionRestAcceleration = false;
	}


	void playerMovement()
	{
		if (Input.GetKey (KeyCode.W) && !collisionRestAcceleration && (rlw.GetGroundHit(out hitRight ) || rrw.GetGroundHit(out hitLeft))) 
		{				
			if (!accelerationSound)
			{					
				setAccelerationSound ();
			}			

			reverse = false;
			brakeForce = 0.0f;
			horsePower = Input.GetAxis ("Vertical") * torquePower * Time.deltaTime * forceFactor;
			fLWheel.transform.Rotate (Vector3.right * -wheelRotationSpeed * Time.deltaTime);
			fRWheel.transform.Rotate (Vector3.right *  wheelRotationSpeed * Time.deltaTime);
			rLWheel.transform.Rotate (Vector3.right * -wheelRotationSpeed * Time.deltaTime);
			rRWheel.transform.Rotate (Vector3.right *  wheelRotationSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.S)) 
		{  
			
			if (rb.velocity.magnitude > 0.1f && !reverse) 
			{	
				brakeForce = rb.mass * torquePower;
				if (!breakingSound) 
				{				
					setBreakSound ();
				}
			} 
			else
			{
				reverse = true;
				horsePower = Input.GetAxis ("Vertical") * torquePower * Time.deltaTime * reverseForce;
				brakeForce = 0.0f;
				if(!playRevvingSound)
					setRevvingsound ();
			}
		} 

		if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)) 
		{			
			brakeForce = rb.mass * idleBreakForce;
			if(!playRevvingSound)
				setRevvingsound ();
		}

		Debug.DrawRay (transform.position, -Vector3.up * 1.5f, Color.red);

		if (!Physics.Raycast(transform.position, -Vector3.up, 1.5f))
		{	
			if(!playRevvingSound)
				setRevvingsound ();
		}

		if (rb.velocity.magnitude > 0.8f && !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && (rlw.GetGroundHit(out hitRight ) || rrw.GetGroundHit(out hitLeft))) 
		{
			if (!breakingSound) 
			{
				setBreakSound ();
			}
		} 
		else if( rb.velocity.magnitude < 0.8f && !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S))
		{
			
			if (!playRevvingSound && !turnCarOnSound) 
			{				
				setRevvingsound ();
			}
		}

		if (Input.GetKeyDown (KeyCode.H)) 
		{
			honk.Play ();
		}

		//Debug.Log ("horse power: " + horsePower);
		//Debug.Log ("Rigid Velocity: " + rb.velocity.magnitude);


		/*if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.up * -rotationSpeed * Time.deltaTime);

			leftTurn = true;
		} 

		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime);

			rightTurn = true;
		}*/
	}

	void FixedUpdate()
	{
		rotationSteer = Input.GetAxis ("Horizontal") * maxRotationSteer;
		if (Input.GetAxis ("Horizontal") != 0.0f && rb.velocity.magnitude > 15.0f) 
		{
			skid.Play ();
		}
		else 
		{
			skid.Stop ();
		}
		frw.steerAngle = rotationSteer;
		flw.steerAngle = rotationSteer;

		if (brakeForce > 0.0f) 
		{
			frw.brakeTorque = brakeForce;
			flw.brakeTorque = brakeForce;
			rrw.brakeTorque = brakeForce;
			rlw.brakeTorque = brakeForce;
			rrw.motorTorque = 0.0f;
			rlw.motorTorque = 0.0f;
		} 
		else 
		{
			frw.brakeTorque = 0.0f;
			flw.brakeTorque = 0.0f;
			rrw.brakeTorque = 0.0f;
			rlw.brakeTorque = 0.0f;
			rrw.motorTorque = horsePower;
			rlw.motorTorque = horsePower;
		}
				
		

		/*if (!rightTurn && !leftTurn) 
		{
			
			fLWheel.transform.rotation = Quaternion.Lerp (fLWheel.rotation, leftWheelOriginalRot, Time.deltaTime * rotationSpeed);

			fRWheel.transform.rotation = Quaternion.Lerp (fRWheel.rotation, rightWheelOriginalRot, Time.deltaTime * rotationSpeed);
		}
		
		if (rightTurn) 
		{
			fLWheel.transform.Rotate (leftWheelRotationRight);
			fRWheel.transform.Rotate (rightWheelRotationRight);
		}

		if (leftTurn) 
		{
			fLWheel.transform.Rotate (leftWheelRotationLeft);
			fRWheel.transform.Rotate (rightWheelRotationLeft);
		}*/


	}

	void OnTriggerEnter( Collider other)
	{
		//GameObject bulletInstRight = Instantiate (something, transform.position, transform.rotation) as GameObject;


		if (other.gameObject.tag == "ConcreteTerrain") 
		{
			if (iceTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < iceTurrets.Length; i++) {
					iceTurrets [i].SetActive (false);
				}
			}

			if (sandTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < sandTurrets.Length; i++) {
					sandTurrets [i].SetActive (false);
				}
			}

			for (int i = 0; i < concreteTurrets.Length; i++) 
			{
				concreteTurrets [i].SetActive (true);
			}

			for (int i = 0; i < wheelsArr.Length; i++) 
			{
				WheelFrictionCurve forwardCurve = wheelsArr[i].forwardFriction;
				forwardCurve.extremumSlip = 0.4f;
				forwardCurve.extremumValue = 1f;
				forwardCurve.asymptoteSlip = 0.8f;
				forwardCurve.asymptoteValue = 0.5f;
				forwardCurve.stiffness = 1.5f;

				WheelFrictionCurve sidewaysCurve = wheelsArr[i].sidewaysFriction;
				sidewaysCurve.extremumSlip = 1.0f;
				sidewaysCurve.extremumValue = 1.0f;
				sidewaysCurve.asymptoteSlip = 1.0f;
				sidewaysCurve.asymptoteValue = 1.0f;
				sidewaysCurve.stiffness = 2.0f;

				wheelsArr[i].forwardFriction = forwardCurve;
				wheelsArr [i].sidewaysFriction = sidewaysCurve;		
			}


		}

		if (other.gameObject.tag == "IceTerrain") 
		{
			if (concreteTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < concreteTurrets.Length; i++) {
					concreteTurrets [i].SetActive (false);
				}
			}

			if (grassTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < grassTurrets.Length; i++) {
					grassTurrets [i].SetActive (false);
				}
			}

			for (int i = 0; i < iceTurrets.Length; i++) 
			{
				iceTurrets[i].SetActive (true);
			}

			for (int i = 0; i < wheelsArr.Length; i++) 
			{
				WheelFrictionCurve forwardCurve = wheelsArr[i].forwardFriction;
				forwardCurve.extremumSlip = 0.05f;
				forwardCurve.extremumValue = 0.1f;
				forwardCurve.asymptoteSlip = 0.1f;
				forwardCurve.asymptoteValue = 0.5f;
				forwardCurve.stiffness = 2.5f;

				WheelFrictionCurve sidewaysCurve = wheelsArr[i].sidewaysFriction;
				sidewaysCurve.extremumSlip = 10.0f;
				sidewaysCurve.extremumValue = 2.0f;
				sidewaysCurve.asymptoteSlip = 2.0f;
				sidewaysCurve.asymptoteValue = 2.0f;
				sidewaysCurve.stiffness = 1.2f;

				wheelsArr[i].forwardFriction = forwardCurve;
				wheelsArr [i].sidewaysFriction = sidewaysCurve;		
			}

		}


		if (other.gameObject.tag == "GrassTerrain") 
		{
			if (iceTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < iceTurrets.Length; i++) {
					iceTurrets [i].SetActive (false);
				}
			}

			if (sandTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < sandTurrets.Length; i++) {
					sandTurrets [i].SetActive (false);
				}
			}

			for (int i = 0; i < grassTurrets.Length; i++) 
			{
				grassTurrets[i].SetActive (true);
			}

			for (int i = 0; i < wheelsArr.Length; i++) 
			{
				WheelFrictionCurve forwardCurve = wheelsArr[i].forwardFriction;
				forwardCurve.extremumSlip = 0.4f;
				forwardCurve.extremumValue = 1.0f;
				forwardCurve.asymptoteSlip = 0.8f;
				forwardCurve.asymptoteValue = 0.5f;
				forwardCurve.stiffness = 0.9f;

				WheelFrictionCurve sidewaysCurve = wheelsArr[i].sidewaysFriction;
				sidewaysCurve.extremumSlip = 0.1f;
				sidewaysCurve.extremumValue = 1.0f;
				sidewaysCurve.asymptoteSlip = 0.05f;
				sidewaysCurve.asymptoteValue = 1.0f;
				sidewaysCurve.stiffness = 0.9f;

				wheelsArr[i].forwardFriction = forwardCurve;
				wheelsArr [i].sidewaysFriction = sidewaysCurve;		
			}

		}

		if (other.gameObject.tag == "SandTerrain") 
		{
			if (concreteTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < concreteTurrets.Length; i++) {
					concreteTurrets [i].SetActive (false);
				}
			}

			if (grassTurrets [0].activeSelf == true) 
			{
				for (int i = 0; i < grassTurrets.Length; i++) {
					grassTurrets [i].SetActive (false);
				}
			}

			for (int i = 0; i < sandTurrets.Length; i++) 
			{
				sandTurrets[i].SetActive (true);
			}

			for (int i = 0; i < wheelsArr.Length; i++) 
			{
				WheelFrictionCurve forwardCurve = wheelsArr[i].forwardFriction;
				forwardCurve.extremumSlip = 0.4f;
				forwardCurve.extremumValue = 1.0f;
				forwardCurve.asymptoteSlip = 0.8f;
				forwardCurve.asymptoteValue = 0.5f;
				forwardCurve.stiffness = 0.7f;

				WheelFrictionCurve sidewaysCurve = wheelsArr[i].sidewaysFriction;
				sidewaysCurve.extremumSlip = 0.1f;
				sidewaysCurve.extremumValue = 1.0f;
				sidewaysCurve.asymptoteSlip = 0.05f;
				sidewaysCurve.asymptoteValue = 1.0f;
				sidewaysCurve.stiffness = 0.5f;

				wheelsArr[i].forwardFriction = forwardCurve;
				wheelsArr [i].sidewaysFriction = sidewaysCurve;		
			}

		}

	}

}

