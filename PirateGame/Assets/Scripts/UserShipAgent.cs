using UnityEngine;
using System.Collections;

public class UserShipAgent : MonoBehaviour {
	
	public Ship ship;
	public Transform harbor;
	public int turnSpeed;

	public int loot = 500;
	public int cannonPower = 1;
	public int hull = 1;
	public int sails = 1;
	public GameObject shoppingGUI;
	public GameObject maxMessage;
	public GameObject noLootMessage;

	private string screenText;

	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
		/*
			spawn user in their harbor
			give user enough gold to purchase only 1 upgrade
			done in update: open shop menu for user to purchase item, game timer starts when menu is exited
		 */
		ship = new Ship ();
		screenText = "";
		ship.state = Ship.State.Roaming;
		//init ship's harbor
		harbor = GameObject.Find ("Harbor").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move/attack according to user if roaming
		if (ship.state == Ship.State.Roaming)
		{/*
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);			
			transform.position += transform.up * curShipSpeed * Time.deltaTime;
			
			curShipSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			curShipSpeed = Mathf.Clamp(curShipSpeed, 0.5f, maxShipSpeed);

			//TODO:add keylisteners for attacking
			//'Q' shoot left cannons
			//'E' shoot right rannons */

			//No menus should show when the ship is roaming.
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(false);
			noLootMessage.SetActive(false);
		}

		else if (ship.state == Ship.State.Looting)
		{ /*
			//TODO

				//display text “press space to stop looting”
				//ship object handles looting
			*/
		}

		else if (ship.state == Ship.State.Shopping)
		{
			//TODO

				//display text “press space to raise anchor”
				//automatically unload gold off ship to harbor
				//display shop menu

			//Note that, if the "shoppingGUI.SetActive(true);" line
			//was put here, then the ShopGUI would constantly be on the
			//screen while the ship's state is set to Shopping. This
			//means that the shop GUI would still be visible and clickable
			//underneath the messages for maxed upgrades and no loot.

		}

		//Testing for Shop GUI
		foreach (char c in Input.inputString) {
			//Press s to open the shop
			if (c == "s"[0]){
				ship.state = Ship.State.Shopping;
				shoppingGUI.SetActive(true);
			}

		}

	}

	void OnTriggerEnter(Collider hit)
	{
		print("hit a trigger");

		if (hit.tag.Equals("island"))
		{
			if (ship.state != Ship.State.Looting)
			{
				screenText = "Press Space to start looting the island \n and get some gold!";
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				ship.state = (ship.state == Ship.State.Looting) ? Ship.State.Roaming : Ship.State.Looting;
			}
		}
			

		else if (hit.name.Equals(harbor.name))
		{
			if (ship.state != Ship.State.Shopping)
			{
				screenText = "Press Space to enter the harbor, drop off your gold, \n and buy upgrades.";
			}
			
			if (Input.GetKeyUp(KeyCode.Space))
			{
				ship.state = (ship.state == Ship.State.Shopping) ? Ship.State.Roaming : Ship.State.Shopping;
			}
		}
	}

	void OnTriggerExit(Collider hit)
	{
		screenText = "";
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0,0,190,800), screenText);
	}


	//The following are functions for the Shop GUI buttons
	public void CannonUpgrade(){
		//If the cannons are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (cannonPower == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (loot < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}

		// Otherwise, upgrade the cannon and subtract from loot each time.
		else {
			cannonPower++;
			loot = loot - 100;
		}
	}
	
	public void HullUpgrade(){
		//If the hull is already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (hull == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (loot < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}

		// Otherwise, upgrade the hull and subtract from loot each time.
		else {
			hull++;
			loot = loot - 100;
		}
	}
	
	public void SailUpgrade(){
		//If the sails are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (sails == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (loot < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}

		// Otherwise, upgrade the sails and subtract from loot each time.
		else {
			sails++;
			loot = loot - 100;
		}
	}

	public void ReturnToSea(){
		//Close the shop GUI and change the ship's state to Roaming
		shoppingGUI.SetActive(false);

		ship.state = Ship.State.Roaming;
	}

	public void ReturnToMenu(){
		//Hide the other messages and show the shop GUI again
		maxMessage.SetActive(false);
		noLootMessage.SetActive(false);
		shoppingGUI.SetActive(true);
	}
}
