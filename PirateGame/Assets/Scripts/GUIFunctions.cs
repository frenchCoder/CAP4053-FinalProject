using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GUIFunctions : MonoBehaviour {
	Ship ship;

	//TODO: change from public to private and have Start create instances from the prefabs
	private GameObject lootText;
	private GameObject harborText;
	public GameObject lootingGUI;
	public GameObject shoppingGUI;
	public GameObject maxMessage;
	public GameObject noLootMessage;

	//The following variables keep track of how many upgrades there are so far.
	public int cannonCount = 0;
	public int hullCount = 0;
	public int sailCount = 0;
	public int crateCount = 0;
	public int lootRateCount = 0;

	//Code for changing the button text.
	private GameObject CannonText;
	private GameObject HullText;
	private GameObject SailsText;
	private GameObject CratesText;
	private GameObject LootRateText;

	private GameObject[] lootingCoins;
	private int lootCoinCount = 0;
	private GameObject eventSystem; //Used to make currently selected item no longer selected - needed for "space" to work on shoppingGUI

	bool open;//true if main shopping menu is up

	// Use this for initialization
	void Start () {
		eventSystem = (GameObject)GameObject.Find("EventSystem");
		lootingCoins = FillCoins(10);
		ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();
		lootText = (GameObject)GameObject.Instantiate(Resources.Load("GUI/LootText"));
		harborText = (GameObject)GameObject.Instantiate(Resources.Load("GUI/HarborText"));
		print("start in gui funct");
		//Code for changing the button text.
		CannonText = (GameObject)GameObject.Find("ShopGUI/CannonButton/CannonText");
		HullText = (GameObject)GameObject.Find("HullText");
		SailsText = (GameObject)GameObject.Find("SailsText");
		CratesText = (GameObject)GameObject.Find("CratesText");
		LootRateText = (GameObject)GameObject.Find("LootRateText");

		lootText.SetActive(false);
		harborText.SetActive (false);
		lootingGUI.SetActive (false);
		shoppingGUI.SetActive (false);
		maxMessage.SetActive (false);
		noLootMessage.SetActive (false);

		open = false;
	}
	
	// Update is called once per frame
	void Update () 	
	{
		if(ship.state == Ship.State.Shopping && !open)
		{
			shoppingGUI.SetActive(true);
			open = true;

			//reinit button text b/c they became null when shoppingGUI.setActive(false)
			CannonText = (GameObject)GameObject.Find("ShopGUI/CannonButton/CannonText");
			HullText = (GameObject)GameObject.Find("HullText");
			SailsText = (GameObject)GameObject.Find("SailsText");
			CratesText = (GameObject)GameObject.Find("CratesText");
			LootRateText = (GameObject)GameObject.Find("LootRateText");
		}
		if(ship.state == Ship.State.Looting && !open)
		{
			lootingGUI.SetActive(true);
			ResetCoins ();
			open = true;
		}

		//change button text
		if (ship.state == Ship.State.Shopping && open && shoppingGUI.activeInHierarchy)
		{
			//Code for changing the button text.
			CannonText.GetComponent<Text>().text = "Cannons: +"+cannonCount;
			HullText.GetComponent<Text>().text = "Health: +"+hullCount;
			SailsText.GetComponent<Text>().text = "Sails: +"+sailCount;
			CratesText.GetComponent<Text>().text = "Max Gold: +"+crateCount;
			LootRateText.GetComponent<Text>().text = "Loot Rate: +"+lootRateCount;
		}
	}

	public void DisplayText(int opt)
	{
		switch (opt)
		{
			case 0:
				lootText.SetActive(false);
				harborText.SetActive(false);
				break;
			case 1:
				lootText.SetActive(true);
				break;
			case 2:
				harborText.SetActive(true);
				break;
		}	
	}

	public GameObject[] FillCoins(int numCoins)
	{
		GameObject[] temp = new GameObject[numCoins];
		for(int i=0; i<temp.Length; i++)
		{
			GameObject next = GameObject.Find("Coin"+i);
			temp[i] = next;
		}
		return temp;
	}

	public void ShowNextCoin()
	{
		if(lootCoinCount >= lootingCoins.Length)
		{
			ResetCoins();
		}
		lootingCoins[lootCoinCount].SetActive(true);
		lootCoinCount++;
	}

	public void ResetCoins()
	{
		lootCoinCount = 0;
		for(int i=0; i<lootingCoins.Length; i++)
		{
			lootingCoins[i].SetActive(false);
		}
	}

	//The following are functions for the Shop GUI buttons
	public void CannonUpgrade(){
		//If the cannons are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		//TODO: figure out cannon attackpower stuff
		if (ship.leftCannon.attackPower == 3) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the cannon and subtract from loot each time.
		else {
			cannonCount++;
			ship.goldInHarbor -= 100;
			ship.goldGUI.updateValue(ship.goldInHarbor);
			ship.upgrade (Ship.Upgrade.AttackPower);
		}		
		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(shoppingGUI, null);
	}
	
	//upgrade the health
	public void HullUpgrade(){
		//If the hull is already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.maxHealth == 7) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the hull and subtract from loot each time.
		else {
			hullCount++;
			ship.goldInHarbor -= 100;
			ship.goldGUI.updateValue(ship.goldInHarbor);
			ship.upgrade(Ship.Upgrade.Hp);
		}		
		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(shoppingGUI, null);
	}
	
	//upgrade the speed
	public void SailUpgrade(){
		//If the sails are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		//TODO: change speed value to compare to
		if (ship.maxSpeed == 3) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the sails and subtract from loot each time.
		else {
			sailCount++;
			ship.goldInHarbor -= 100;
			ship.goldGUI.updateValue(ship.goldInHarbor);
			ship.upgrade(Ship.Upgrade.Speed);
		}		
		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(shoppingGUI, null);
	}

	public void CrateUpgrade(){
		//If the crates are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.maxGold == 150) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the crates and subtract from loot each time.
		else {
			crateCount++;
			ship.goldInHarbor -= 100;
			ship.goldGUI.updateValue(ship.goldInHarbor);
			ship.upgrade(Ship.Upgrade.MaxGold);
		}		
		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(shoppingGUI, null);
	}

	public void LootRateUpgrade(){
		//If the looting rate is already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.lootingSpeed == 0.5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the looting rate and subtract from loot each time.
		else {
			lootRateCount++;
			ship.goldInHarbor -= 100;
			ship.goldGUI.updateValue(ship.goldInHarbor);
			ship.upgrade(Ship.Upgrade.LootingSpeed);
		}
		eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(shoppingGUI, null);
	}
	
	//return to roaming state on main board from shopping in harbor
	public void ReturnToSea()
	{
		//Close the shop GUI and change the ship's state to Roaming
		shoppingGUI.SetActive(false);
		lootingGUI.SetActive (false);
		noLootMessage.SetActive(false);
		maxMessage.SetActive(false);
		
		//turn ship around
		Transform playerObject = GameObject.Find ("PlayerShip").transform;
		Vector3 temp = playerObject.rotation.eulerAngles;
		temp.y += 180.0f;
		playerObject.rotation = Quaternion.Euler(temp);
		playerObject.position += transform.up * ship.curSpeed * Time.deltaTime;
		ship.curSpeed = ship.minSpeed;
		ship.state = Ship.State.Roaming;


		open = false;
	}
	
	//called when returning to main menu after 'unable to buy' messages
	public void ReturnToMenu()
	{
		//Hide the other messages and show the shop GUI again
		maxMessage.SetActive(false);
		noLootMessage.SetActive(false);
		shoppingGUI.SetActive(true);
		open = false;
	}
}
