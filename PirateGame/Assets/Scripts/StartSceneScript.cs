using UnityEngine;
using System.Collections;

public class StartSceneScript : MonoBehaviour {

	public void Start()
	{
		if(GameObject.Find("Start_Background_Music(Clone)") == null)
			Instantiate(Resources.Load("Start_Background_Music"));
	}

	public void playGame() {
		Debug.Log ("start game");
		Destroy(GameObject.Find("Start_Background_Music(Clone)"));
		Application.LoadLevel("main");
	}

	public void viewHowTo() {
		Debug.Log ("how to play");
		DontDestroyOnLoad(GameObject.Find("Start_Background_Music(Clone)"));
		Application.LoadLevel("howToPage");
	}
}
