using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	float max_health = 5f;
	// Use this for initialization
	void Start () 
	{
		this.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void decreaseHealth(float newHealthValue)
	{
		this.GetComponent<RectTransform>().localScale = new Vector3(newHealthValue/max_health, 1f, 1f);
	}

	public void updateMaxHealth(float value)
	{
		max_health += value;
	}

	public void resetHealth()
	{
		float length = max_health/5f;
		this.GetComponent<RectTransform>().localScale = new Vector3(length, 1f, 1f);
	}
}
