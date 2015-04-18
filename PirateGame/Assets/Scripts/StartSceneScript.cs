using UnityEngine;
using System.Collections;

public class StartSceneScript : MonoBehaviour {

	public void playGame() {
		Debug.Log ("start game");
		Application.LoadLevel("main");
	}

	public void viewHowTo() {
		Debug.Log ("how to play");
		Application.LoadLevel("howToPage");
	}
}
