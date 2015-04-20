using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	RectTransform health_bar; 
	RectTransform background;
	GameObject text;
	int max_health = 5;
	// Use this for initialization
	void Start () 
	{
		health_bar = GameObject.Find("Health_Rect").GetComponent<RectTransform>();
		background = GameObject.Find("HealthBarBoarder").GetComponent<RectTransform>();
		text = GameObject.Find("HealthBarText");
		background.localScale = new Vector3(1f, 1f, 1f);
		health_bar.localScale = new Vector3(1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void changeText(int newTopValue)
	{
		text.GetComponent<Text>().text = "" + newTopValue + "/" + (max_health*20);
	}

	public void decreaseHealth(int newHealthValue)
	{
		health_bar.localScale = new Vector3(((float)newHealthValue)/max_health, 1f, 1f);
		changeText (newHealthValue*20);
	}

	public void updateMaxHealth()
	{
		max_health += 1;
		resetHealth ();
	}

	public void resetHealth()
	{
		float length = (float)(max_health*20)/(5*20);
		health_bar.localScale = new Vector3(length, 1f, 1f);
		background.localScale = new Vector3(length, 1f, 1f);
		changeText (max_health*20);
	}
}
