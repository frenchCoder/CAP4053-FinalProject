using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class gameTimer : MonoBehaviour {

	private float timer;
	private float totalGameTime, startTime;
	public Text timerText;
	private bool gameOver;
	private bool gamePaused;

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
		gamePaused = true;

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
		if (!gameOver && !gamePaused)
		{
			runTimer();

			//game timer stops when player has entered shop 
			if (playerShip.state == Ship.State.Shopping)
			{
				gamePaused = true;
				redShip.gamePaused = true;
				purpleShip.gamePaused = true;
				yellowShip.gamePaused = true;
				playerShip.gamePaused = true;
			}
		}
		else if(gamePaused)
		{
			//game timer starts when player has exited shop 
			if (playerShip.state == Ship.State.Roaming)
			{
				gamePaused = false;
				redShip.gamePaused = false;
				purpleShip.gamePaused = false;
				yellowShip.gamePaused = false;
				playerShip.gamePaused = false;
				startTime = Time.realtimeSinceStartup - (totalGameTime-timer);
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
		scoreText.text = redShip.goldTotal+","+playerShip.goldTotal+","+yellowShip.goldTotal+","+purpleShip.goldTotal;
		DontDestroyOnLoad(scoreText.transform.parent);

		Application.LoadLevel("endscene");
	}

	//return ship with max gold
	private int maxGold(Ship a, Ship b, Ship c, Ship d)
	{
		return Mathf.Max (a.goldTotal, Mathf.Max (b.goldTotal, Mathf.Max (c.goldTotal, d.goldTotal)));
	}

}
