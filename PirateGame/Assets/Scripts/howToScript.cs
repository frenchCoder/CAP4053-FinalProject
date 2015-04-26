using UnityEngine;
using System.Collections;

public class howToScript : MonoBehaviour {

	public void backToStart() {
		Debug.Log ("start game");
		DontDestroyOnLoad(GameObject.Find("Start_Background_Music(Clone)"));
		Application.LoadLevel("startscene");
	}
}
