using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsBarScript : MonoBehaviour 
{
	Text[] playersTotalGold;
	Text[] playersShipGold;
	Text[] playersHealthText;
	
	// Use this for initialization
	void Start () 
	{
		playersTotalGold = new Text[4];
		playersTotalGold[0] = GameObject.Find ("RedTotalGoldText").GetComponent<Text>();
		playersTotalGold[1] = GameObject.Find ("WhiteTotalGoldText").GetComponent<Text>();
		playersTotalGold[2] = GameObject.Find ("YellowTotalGoldText").GetComponent<Text>();
		playersTotalGold[3] = GameObject.Find ("PurpleTotalGoldText").GetComponent<Text>();
		
		playersShipGold = new Text[4];
		playersShipGold[0] = GameObject.Find ("RedShipGoldText").GetComponent<Text>();
		playersShipGold[1] = GameObject.Find ("WhiteShipGoldText").GetComponent<Text>();
		playersShipGold[2] = GameObject.Find ("YellowShipGoldText").GetComponent<Text>();
		playersShipGold[3] = GameObject.Find ("PurpleShipGoldText").GetComponent<Text>();
		
		playersHealthText = new Text[4];
		playersHealthText[0] = GameObject.Find ("RedHealthBarText").GetComponent<Text>();
		playersHealthText[1] = GameObject.Find ("WhiteHealthBarText").GetComponent<Text>();
		playersHealthText[2] = GameObject.Find ("YellowHealthBarText").GetComponent<Text>();
		playersHealthText[3] = GameObject.Find ("PurpleHealthBarText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SetTotalGold(Ship.ShipColor color, int value)
	{
		playersTotalGold[((int)color)].text = ""+value;
	}
	
	public void SetShipGold(Ship.ShipColor color, int value)
	{
		playersShipGold[((int)color)].text = ""+value;
	}
	
	public void SetHealthText(Ship.ShipColor color, int value)
	{
		playersHealthText[((int)color)].text = ""+(value*20);
	}

	public void RetrieveValues()
	{
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			Ship ship = obj.GetComponent<Ship>();
			SetTotalGold(ship.color, ship.goldTotal);
			SetShipGold(ship.color, ship.goldInShip);
			SetHealthText(ship.color, ship.health);
		}
		Ship playership = ((GameObject)GameObject.FindGameObjectWithTag("Player")).GetComponent<Ship>();
		SetTotalGold(playership.color, playership.goldTotal);
		SetShipGold(playership.color, playership.goldInShip);
		SetHealthText(playership.color, playership.health);

	}
}
