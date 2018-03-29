using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusicScript : MonoBehaviour {

	[SerializeField] AudioClip[] backgroundClips;
	AudioSource aud;
	int randomClip;

	// Use this for initialization
	void Start () {

		randomClip = Random.Range (0, 2);
		aud = GetComponent<AudioSource> ();
		aud.clip = backgroundClips [randomClip];
		aud.Play ();	
		Invoke ("songCycle", backgroundClips [randomClip].length);
	}
	
	// Update is called once per frame
	void songCycle()
	{
		if (randomClip == 0) 
		{
			aud.clip = backgroundClips [1];
			aud.Play ();
			randomClip = 1;
			Invoke ("songCycle", backgroundClips [randomClip].length);
		}
		else 
		{
			aud.clip = backgroundClips [0];
			aud.Play ();
			randomClip = 0;
			Invoke ("songCycle", backgroundClips [randomClip].length);
		}
	}

}
