using UnityEngine;
using System.Collections;

public class howToScript : MonoBehaviour {

	public void backToStart() {
		Debug.Log ("start game");
		Application.LoadLevel("startscene");
	}
}
