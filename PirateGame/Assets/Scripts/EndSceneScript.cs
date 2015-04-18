using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour {

	public GameObject scores;
	public Text scoreText;

	void Start()
	{
		scores = (GameObject)GameObject.Find ("Scores");
		scoreText = scores.GetComponent<Text> ();

		scoreText.text = "You Won!\n\nGold coins collected:\nYou: 500 \nRed: 300\nPurple: 200\nYellow: 100";
		//TODO: set the text to game results
	}

	void Update ()
	{

	}

	public void startOver() {
		Debug.Log ("start game");
		Application.LoadLevel("startscene");
	}

}
