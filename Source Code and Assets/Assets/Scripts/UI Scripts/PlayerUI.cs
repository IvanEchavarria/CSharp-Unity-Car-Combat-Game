using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {

	[SerializeField] float maxHealth;
	[SerializeField] Slider health; 
	[SerializeField] Text healthPercentage;
	[SerializeField] Text timerText;


	private int timerCounter;
	private float currHealth;


	// Use this for initialization
	void Start () {

		timerCounter = 300;
		timerText.text = "Timer: " + timerCounter;
		maxHealth = 100f;
		currHealth = maxHealth;
		health.value = maxHealth;
		healthPercentage.text = health.value + "%";
		InvokeRepeating ("clock", 0f, 1f);
	}


	void clock()
	{
		timerCounter--;
		timerText.text = "Timer: " + timerCounter;
		if(timerCounter <= 0)
		{
			DoSceneChange("GameOver");
		}
	}

	public void damage(float dmg)
	{
		currHealth -= dmg;
		health.value = currHealth;
		healthPercentage.text = health.value + "%";
		if(currHealth <= 0.0f)
		{
			DoSceneChange("GameOver");
		}
	}

	public void DoSceneChange(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
