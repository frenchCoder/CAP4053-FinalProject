using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndSceneScript : MonoBehaviour {
	int[] scores;//{red, white, yellow, purple}

	void Start()
	{
		Text scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
		string[] text = scoreText.text.Split (new char[]{','});
		scores = new int[4];
		for(int i=0; i<text.Length; i++)
			scores[i] = int.Parse(text[i]);

		SetScores();
		SetPlaces(Judge ());
	}

	void Update ()
	{

	}

	public void startOver() {
		Application.LoadLevel("startscene");
	}

	//Assigns the score to the text object that displays the ship's score
	private void SetScores()
	{
		((GameObject)GameObject.Find ("redscore")).GetComponent<Text>().text = "" + scores[0];
		GameObject.Find ("whitescore").GetComponent<Text>().text = "" + scores[1];
		GameObject.Find ("yellowscore").GetComponent<Text>().text = "" + scores[2];
		GameObject.Find ("purplescore").GetComponent<Text>().text = "" + scores[3];
	}
	
	private void SetPlaces(int[] places)
	{
		GameObject red = GameObject.Find("Red"+places[0]);
		GameObject white = GameObject.Find ("White"+places[1]);
		GameObject yellow = GameObject.Find("Yellow"+places[2]);
		GameObject purple = GameObject.Find("Purple"+places[3]);

		for(int i = 1; i <=4; i++)
		{
			GameObject.Find("Red"+i).SetActive(false);
			GameObject.Find ("White"+i).SetActive(false);
			GameObject.Find("Yellow"+i).SetActive(false);
			GameObject.Find("Purple"+i).SetActive(false);
		}
		
		red.SetActive(true);
		white.SetActive(true);
		yellow.SetActive(true);
		purple.SetActive(true);
	}
	
	//returns the places each player came in
	private int[] Judge()
	{
		int[] ret = new int[4];
		
		List<int> scoresList = new List<int>();
		scoresList.Add(scores[0]);
		for(int i=1; i<scores.Length; i++)
		{
			if(!scoresList.Contains(scores[i]))
			{
				scoresList.Add(scores[i]);
			}
		}
		scoresList.Sort();
		scoresList.Reverse();
		
		for(int i=0; i<scoresList.Count; i++)
		{
			if(scoresList[i].Equals(scores[0]))
				ret[0] = i+1;
			if(scoresList[i].Equals(scores[1]))
				ret[1] = i+1;
			if(scoresList[i].Equals(scores[2]))
				ret[2] = i+1;
			if(scoresList[i].Equals(scores[3]))
				ret[3] = i+1;
		}
		return ret;
	}
}
