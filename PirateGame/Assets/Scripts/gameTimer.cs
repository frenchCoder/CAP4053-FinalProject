using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour {

	private float timer;
	private float totalGameTime, startTime;
	public Text timerText;
	private bool gameOver;

	// Use this for initialization
	void Start () 
	{
		totalGameTime = timer = 15.0f;
		startTime = Time.realtimeSinceStartup;
		gameOver = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!gameOver)
		{
			runTimer();
		}
		else 
		{
			//TODO:get all player points
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
		Debug.Log ("end game");
		Application.LoadLevel("endscene");
	}
}
