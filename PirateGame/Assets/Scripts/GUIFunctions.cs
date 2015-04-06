using UnityEngine;
using System.Collections;

public class GUIFunctions : MonoBehaviour {
	Ship ship;

	//TODO: change from public to private and have Start create instances from the prefabs
	private GameObject lootText;
	private GameObject harborText;
	public GameObject lootingGUI;
	public GameObject shoppingGUI;
	public GameObject maxMessage;
	public GameObject noLootMessage;

	private GameObject[] lootingCoins;
	private int lootCoinCount = 0;

	private string menuText;

	bool open;

	// Use this for initialization
	void Start () {
		lootingCoins = FillCoins(10);
		ship = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship>();
		lootText = (GameObject)GameObject.Instantiate(Resources.Load("GUI/LootText"));
		harborText = (GameObject)GameObject.Instantiate(Resources.Load("GUI/HarborText"));

		lootText.SetActive(false);
		harborText.SetActive (false);
		lootingGUI.SetActive (false);
		shoppingGUI.SetActive (false);
		maxMessage.SetActive (false);
		noLootMessage.SetActive (false);

		open = false;
		menuText = "";
	}
	
	// Update is called once per frame
	void Update () 	
	{
		if(ship.state == Ship.State.Shopping && !open)
		{
			shoppingGUI.SetActive(true);
			open = true;
		}
		if(ship.state == Ship.State.Looting && !open)
		{
			lootingGUI.SetActive(true);
			open = true;
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
			GameObject next = (GameObject)Instantiate(Resources.Load("Coin"));
			next.transform.position += new Vector3(0.5f * i, 0f, 0f);
			next.SetActive (false);
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
		if (ship.leftCannon.attackPower == 5) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the cannon and subtract from loot each time.
		else {
			ship.goldInHarbor -= 100;
			ship.upgrade (Ship.Upgrade.AttackPower);
			menuText = "Your attack power is now " + ship.leftCannon.attackPower + "!";
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
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the hull and subtract from loot each time.
		else {
			ship.goldInHarbor -= 100;
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
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the sails and subtract from loot each time.
		else {
			ship.goldInHarbor -= 100;
			ship.upgrade(Ship.Upgrade.Speed);
			menuText = "Your max speed is now " + ship.maxSpeed + "!";
		}
	}

	public void CrateUpgrade(){
		//If the crates are already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.maxGold == 600) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the crates and subtract from loot each time.
		else {
			ship.goldInHarbor -= 100;
			ship.upgrade(Ship.Upgrade.MaxGold);
			menuText = "You can now hold " + ship.maxGold + " gold!";
		}
	}

	public void LootRateUpgrade(){
		//If the looting rate is already maxed out or there's
		//not enough loot to upgrade it, hide the shop GUI
		//and show the appropriate message.
		if (ship.lootingSpeed == 30) {
			shoppingGUI.SetActive(false);
			maxMessage.SetActive(true);
		} 
		else if (ship.goldInHarbor < 100) {
			shoppingGUI.SetActive(false);
			noLootMessage.SetActive(true);
		}
		
		// Otherwise, upgrade the looting rate and subtract from loot each time.
		else {
			ship.goldInHarbor -= 100;
			ship.upgrade(Ship.Upgrade.LootingSpeed);
			menuText = "Your looting rate is now " + ship.lootingSpeed + "!";
		}
	}
	
	//return to roaming state on main board from shopping in harbor
	public void ReturnToSea()
	{
		//Close the shop GUI and change the ship's state to Roaming
		shoppingGUI.SetActive(false);
		lootingGUI.SetActive (false);
		//turn ship around
		Transform playerObject = GameObject.Find ("PlayerShip").transform;
		Vector3 temp = playerObject.rotation.eulerAngles;
		temp.y += 180.0f;
		playerObject.rotation = Quaternion.Euler(temp);
		playerObject.position += transform.up * ship.curSpeed * Time.deltaTime;
		ship.curSpeed = ship.minSpeed;
		ship.state = Ship.State.Roaming;
		menuText = "";
		
		print ("done shopping");
		open = false;
	}
	
	//called when returning to main menu after 'unable to buy' messages
	public void ReturnToMenu()
	{
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
