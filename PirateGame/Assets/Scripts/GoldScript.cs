using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldScript : MonoBehaviour 
{
	public GoldScript.State state;
	private GameObject GoldCoinShip;
	private GameObject GoldCoinHarbor;
	private GameObject GoldBarText;
	// Use this for initialization7
	void Start () 
	{
		state = GoldScript.State.Harbor;
		GoldCoinShip = (GameObject)GameObject.Find("Gold Coin Ship");
		GoldCoinShip.SetActive(false);
		GoldCoinHarbor = (GameObject)GameObject.Find("Gold Coin Harbor");
		GoldBarText = (GameObject)GameObject.Find("GoldBarText");
		updateValue(0);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void updateValue(int value)
	{
		GoldBarText.GetComponent<Text>().text = ""+value;
	}

	public void changeState(int value)
	{
		if(state == State.Harbor)
		{
			state = State.Ship;
			GoldCoinHarbor.SetActive(false);
			GoldCoinShip.SetActive(true);
		}
		else
		{
			state = State.Harbor;
			GoldCoinHarbor.SetActive(true);
			GoldCoinShip.SetActive(false);
		}
		updateValue (value);
	}
	
	public enum State{Ship, Harbor};
}
