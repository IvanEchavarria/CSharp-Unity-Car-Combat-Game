using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScript : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerUI>().damage(1000.0f);
		}
	}
}
