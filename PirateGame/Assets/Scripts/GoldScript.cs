using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldScript : MonoBehaviour 
{
	public GoldScript.State state;
	private GameObject GoldCoinShip;
	private GameObject GoldCoinHarbor;
	private GameObject GoldBarText;
	private Ship ship;
	// Use this for initialization7
	void Start () 
	{
		ship = GameObject.Find("PlayerShip").GetComponent<Ship>();
		state = GoldScript.State.Harbor;
		GoldCoinShip = (GameObject)GameObject.Find("ShipCoin");
		GoldCoinShip.SetActive(false);
		GoldCoinHarbor = (GameObject)GameObject.Find("HarborCoin");
		GoldBarText = (GameObject)GameObject.Find("GoldBarText");
		update_Value(100);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(state == State.Harbor && !GoldBarText.Equals(""+ship.goldInHarbor))
		{
			update_Value (ship.goldInHarbor);
		}
		else if(state == State.Ship && !!GoldBarText.Equals(""+ship.goldInShip))
		{
			update_Value(ship.goldInShip);
		}
	}

	public void update_Value(int value)
	{
		GoldBarText.GetComponent<Text>().text = ""+value;
	}

	public void changeState(int value)
	{
		if(state == State.Harbor)
		{
			GoldCoinHarbor.GetComponent<CanvasRenderer>().gameObject.SetActive(false);
			GoldCoinShip.GetComponent<CanvasRenderer>().gameObject.SetActive(true);
			//GoldCoinHarbor.GetComponent<Image>().enabled = false;
			//GoldCoinShip.GetComponent<Image>().enabled = true;


			state = State.Ship;
		}
		else
		{
			GoldCoinHarbor.GetComponent<CanvasRenderer>().gameObject.SetActive(true);
			GoldCoinShip.GetComponent<CanvasRenderer>().gameObject.SetActive(false);

			//GoldCoinHarbor.GetComponent<Image>().enabled = true;
			//GoldCoinShip.GetComponent<Image>().enabled = false;
			state = State.Harbor;
		}
		update_Value (value);
	}
	
	public enum State{Ship, Harbor};
}
