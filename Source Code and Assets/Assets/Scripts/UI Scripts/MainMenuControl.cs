using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour {

    [SerializeField] Text GameTitle;
    [SerializeField] GameObject UIButtons;
    [SerializeField] GameObject HowToPlayObj;
	[SerializeField] GameObject DifficultyObj;
	[SerializeField] Image backgroundTitleImg;
	[SerializeField] Text backToMain;
	[SerializeField] Text DifficultyText;

    Color32 textColor;
    float timeToChange = 0.1f;
	bool inHowToPlayMenu = false;


    // Use this for initialization
    void Start () {
		
		backToMain.text = "Exit Game";
	}
	
	// Update is called once per frame
	void Update () {

        timeToChange -= Time.deltaTime;

        if(timeToChange <= 0)
        {
            timeToChange = 0.1f;
            generateColor();
            GameTitle.color = textColor;
        }
        		
	}    

    public void DoSceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void enableHowtoPlay()
    {
		//backgroundTitleImg.enabled = false;
		DifficultyObj.SetActive(false);
		UIButtons.SetActive (false);
		HowToPlayObj.SetActive (true);
		inHowToPlayMenu = true;
		backToMain.text = "Back to Main";
    }


	public void backToMainMenu()
	{
		if (inHowToPlayMenu) {
			//backgroundTitleImg.enabled = true;
			UIButtons.SetActive (true);
			HowToPlayObj.SetActive (false);
			inHowToPlayMenu = false;
			backToMain.text = "Exit Game";
		} 
		else 
		{
			Debug.Log ("Application quit");
			Application.Quit (); ///Change doesnt work
		}
	}

	public void ShowDifficultyOptions()
	{
		DifficultyObj.SetActive(true);
	}

	public void EasyDifficulty()
	{
		DifficultyText.text = "Destroy one turret to win the game.";
	}

	public void NormalDifficulty()
	{
		DifficultyText.text = "Destroy nine turrets to win the game.";
	}

	public void HardDifficulty()
	{
		DifficultyText.text = "Destroy all the turrets to win the game.";
	}

    void generateColor()
    {
        textColor = new Color32( (byte)Random.Range(0, 255),
                                 (byte)Random.Range(0, 255), 
                                 255, 255);

    }


}
