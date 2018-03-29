using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameManagerScript :  MonoBehaviour {

	public static int difficulty = 9;

	public void easy()
	{
		difficulty = 1;
		Debug.Log("Difficulty set to Easy" + difficulty);
	}

	public void normal()
	{
		difficulty = 9;
		Debug.Log("Difficulty set to normal" + difficulty);
	}

	public void hard()
	{
		difficulty = 19;
		Debug.Log("Difficulty set to hard" + difficulty);
	}


}
