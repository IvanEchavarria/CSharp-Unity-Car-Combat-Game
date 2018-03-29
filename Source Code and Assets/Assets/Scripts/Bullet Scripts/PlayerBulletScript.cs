using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour {

	[SerializeField] GameObject explosion;
	[SerializeField] GameObject nukeExplosion;

	bool hitObject = false;

	void FixedUpdate()
	{
		RaycastHit hit;

		Vector3 fwd = transform.TransformDirection (Vector3.forward);

		Debug.DrawRay (transform.position, fwd * 20.0f, Color.red);

		if(Physics.Raycast(transform.position, fwd, out hit , 20.0f ))
		{
			if (hit.collider.tag == "Player") 
			{
				hit.collider.transform.GetComponent<PlayerUI>().damage(2.2f);
				GameObject explosionInst = Instantiate (explosion, hit.point, transform.rotation) as GameObject;
				Destroy (explosionInst, 0.5f);
				Destroy (gameObject);
			} 
			else if (hit.collider.tag == "ConcreteTurret" || hit.collider.tag == "IceTurret" || hit.collider.tag == "GrassTurret" || hit.collider.tag == "SandTurret") 
			{
				hit.collider.transform.GetChild(0).GetChild(9).GetComponent<EnemyTurretBehaviour>().AddjustCurrentHealth(15);
				GameObject explosionInst = Instantiate (explosion, hit.point, transform.rotation) as GameObject;
				Destroy (explosionInst, 0.5f);
				Destroy (gameObject);
			} 
			else if(hit.collider.tag == "rocket" || hit.collider.tag == "OilDrum" || hit.collider.tag == "mine" )
			{
				GameObject explosionInst = Instantiate (nukeExplosion, hit.point, transform.rotation) as GameObject;
				Destroy (explosionInst, 0.5f);
				Destroy(hit.collider.transform.gameObject);
				Destroy (gameObject);
			}
			else if(hit.collider.tag == "ArenaTerrain" || hit.collider.tag == "Wall")
			{
				GameObject explosionInst = Instantiate (explosion, hit.point, transform.rotation) as GameObject;
				Destroy (explosionInst, 0.5f);
				Destroy (gameObject);
			}
		}


	}
		
}
