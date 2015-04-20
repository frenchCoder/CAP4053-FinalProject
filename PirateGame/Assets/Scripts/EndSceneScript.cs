using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour {

	public GameObject scores;
	public GameObject scoreCanvas;

	void Start()
	{
		//make score text visible
		scores = (GameObject)GameObject.Find ("ScoreText");
		scores.GetComponent<Text> ().enabled = true;
	}

	void Update ()
	{

	}

	public void startOver() {
		scores.GetComponent<Text> ().enabled = false;
		Application.LoadLevel("startscene");
	}

}
