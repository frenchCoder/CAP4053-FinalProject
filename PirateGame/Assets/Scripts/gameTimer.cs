using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class gameTimer : MonoBehaviour {

	private float timer;
	private float totalGameTime, startTime;
	public Text timerText;
	private bool gameOver;
	private bool gameStarted;

	private Text scoreText;

	public Ship redShip;
	public Ship purpleShip;
	public Ship yellowShip;
	public Ship playerShip;

	// Use this for initialization
	void Start () 
	{
		totalGameTime = timer = 120.0f;
		gameOver = false;
		gameStarted = false;

		//init score text and make invisible
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		scoreText.text = "";
		scoreText.enabled = false;

		//get ships
		redShip = GameObject.Find ("RedShip").GetComponent<Ship> ();
		purpleShip = GameObject.Find ("PurpleShip").GetComponent<Ship> ();
		yellowShip = GameObject.Find ("YellowShip").GetComponent<Ship> ();
		playerShip = GameObject.Find ("PlayerShip").GetComponent<Ship> ();


		Vector3 temp = playerShip.transform.rotation.eulerAngles;
		temp.y += 180.0f;
		playerShip.transform.rotation = Quaternion.Euler(temp);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver && gameStarted)
		{
			runTimer();
		}
		else if(!gameStarted)
		{
			//game timer starts when player has exited shop first time
			if (playerShip.state == Ship.State.Roaming)
			{
				gameStarted = true;
				redShip.gameStarted = true;
				purpleShip.gameStarted = true;
				yellowShip.gameStarted = true;
				playerShip.gameStarted = true;
				startTime = Time.realtimeSinceStartup;
			}
		}
		else 
		{
			endGame ();
		}
	}

	public void runTimer()
	{
		timer = totalGameTime - (Time.realtimeSinceStartup - startTime);		
		
		if ( timer < 0 )
		{
			gameOver = true;
		}
		
		//update timer display
		int minutes, seconds;		
		minutes = (int) (timer / 60);
		seconds = (int) (timer % 60);	
		timerText.text = (seconds > 9) ? (minutes + " : " + seconds) : (minutes + " : 0" + seconds);

		if (minutes == 0 && seconds < 11)
		{
			timerText.color = Color.red;
			timerText.fontSize = 35;
		}
	}


	public void endGame ()
	{
		string points = "\nPlayer Ship: " + playerShip.goldTotal + "\nRed Ship: " + redShip.goldTotal
			+ "\nPurple Ship: " + purpleShip.goldTotal + "\nYellow Ship: " + yellowShip.goldTotal;

		if (playerShip.goldTotal == maxGold(playerShip, redShip, purpleShip, yellowShip))
		{
			scoreText.text = "You Won!\n";
		}
		else
		{
			scoreText.text = "You Lost!\n";
		}

		scoreText.text += points;

		DontDestroyOnLoad(scoreText.transform.parent);

		Application.LoadLevel("endscene");
	}

	//return ship with max gold
	private int maxGold(Ship a, Ship b, Ship c, Ship d)
	{
		return Mathf.Max (a.goldTotal, Mathf.Max (b.goldTotal, Mathf.Max (c.goldTotal, d.goldTotal)));
	}

}
