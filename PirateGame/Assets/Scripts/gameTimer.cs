using UnityEngine;
using System.Collections;

public class gameTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void endGame (){
		Debug.Log ("end game");
		Application.LoadLevel("endscene");
	}
}
