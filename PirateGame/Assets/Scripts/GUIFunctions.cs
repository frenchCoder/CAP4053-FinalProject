﻿using UnityEngine;
using System.Collections;

public class GUIFunctions : MonoBehaviour {
	public Ship ship;

	public GameObject shoppingGUI;
	public GameObject maxMessage;
	public GameObject noLootMessage;

	private string menuText;

	public bool open;


	// Use this for initialization
	void Start () {
		ship = (Ship) GameObject.Find("PlayerShip").GetComponent(typeof(Ship));
		shoppingGUI.SetActive (false);
		maxMessage.SetActive (false);
		noLootMessage.SetActive (false);
		menuText = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(ship.state == Ship.State.Shopping && !open){
			shoppingGUI.SetActive(true);
			open = true;
		}
	}

	//The following are functions for the Shop GUI buttons
	public void CannonUpgrade(){
		//If the cannons are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		//TODO: figure out cannon attackpower stuff
		if (ship.leftCannons[0].attackPower == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldTotal < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the cannon and subtract from loot each time.
		else {
			ship.goldTotal -= 100;
			ship.upgrade (Ship.Upgrade.AttackPower);
			menuText = "Your attack power is now " + ship.leftCannons[0].attackPower + "!";
		}
		
	}
	
	//upgrade the health
	public void HullUpgrade(){
		//If the hull is already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.health == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldTotal < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the hull and subtract from loot each time.
		else {
			ship.goldTotal -= 100;
			ship.upgrade(Ship.Upgrade.Hp);
			menuText = "Your hull strength is now " + ship.health + "!";
		}
	}
	
	//upgrade the speed
	public void SailUpgrade(){
		//If the sails are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		//TODO: change speed value to compare to
		if (ship.maxSpeed == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldTotal < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the sails and subtract from loot each time.
		else {
			ship.goldTotal -= 100;
			ship.upgrade(Ship.Upgrade.Speed);
			menuText = "Your max speed is now " + ship.maxSpeed + "!";
		}
	}

	public void CrateUpgrade(){
		//If the sails are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		//TODO: change speed value to compare to
		if (ship.maxGold == 600) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldTotal < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the sails and subtract from loot each time.
		else {
			ship.goldTotal -= 100;
			ship.upgrade(Ship.Upgrade.MaxGold);
			menuText = "You can now hold " + ship.maxGold + " gold!";
		}
	}
	
	//return to roaming state on main board from shopping in harbor
	public void ReturnToSea(){
		//Close the shop GUI and change the ship's state to Roaming
		shoppingGUI.SetActive(false);
		
		//turn ship around
		Transform playerObject = GameObject.Find ("PlayerShip").transform;
		Vector3 temp = playerObject.rotation.eulerAngles;
		temp.y += 180.0f;
		playerObject.rotation = Quaternion.Euler(temp);
		playerObject.position += transform.up * ship.curSpeed * Time.deltaTime;
		ship.state = Ship.State.Roaming;
		menuText = "";
		
		print ("done shopping");
	}
	
	//called when returning to main menu after 'unable to buy' messages
	public void ReturnToMenu(){
		//Hide the other messages and show the shop GUI again
		maxMessage.SetActive(false);
		noLootMessage.SetActive(false);
		shoppingGUI.SetActive(true);
		menuText = "";

		open = false;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (300,0,190,800), menuText);
	}
}
