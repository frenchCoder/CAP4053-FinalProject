using UnityEngine;
using System.Collections;

public class UserShipAgent : MonoBehaviour {
	
	Ship ship;
	public GUIFunctions gui;

	private Transform island;
	private string screenText;
	private Collider curhit;

	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
		/*
			spawn user in their harbor
			give user enough gold to purchase only 1 upgrade
			done in update: open shop menu for user to purchase item, game timer starts when menu is exited
		 */
		
		ship = GetComponent<Ship>();
		gui = ((GameObject)GameObject.Find("GUI_Manager")).GetComponent<GUIFunctions>();
		screenText = "";
		ship.state = Ship.State.Roaming; //Should probably start out as shopping & facing the harbor so they can get their first free upgrade when the game starts for the first time.
		island = GameObject.Find ("LootIsland").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move/attack according to user if roaming
		if (ship.state == Ship.State.Roaming)
		{
			//Rotate on input
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * ship.turnSpeed * Time.deltaTime);			
			//Always move forward
			transform.position += transform.up * ship.curSpeed * Time.deltaTime;

			//Increase/decrease speed on input
			ship.curSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			ship.curSpeed = Mathf.Clamp(ship.curSpeed, 0.5f, ship.maxSpeed);

			//'Q' shoot left cannons
			if (Input.GetKeyUp(KeyCode.Q))
			{
				ship.attack("L");				
			}
			//'E' shoot right rannons 
			if (Input.GetKeyUp(KeyCode.E))
			{
				ship.attack("R");
			}

		}

		else if (ship.state == Ship.State.Looting)
		{ 
			screenText = "Press Space to set sail.";//TODO:move this to handle by gui && add to shopping state
			//listen for space where user wants to stop and roam again
			if (Input.GetKeyDown(KeyCode.Space) || ship.goldInShip >= ship.maxGold)
			{
				gui.ResetCoins();
				gui.ReturnToSea ();
				print ("done looting");
			}
			
			else
				ship.loot();
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
		//TODO: should be able to move in waiting state so you can move around the island, not just get stopped on one side
		else if (ship.state == Ship.State.Waiting) 
		{
			//listen for space where user wants to start looting or shopping
			if (Input.GetKeyUp(KeyCode.Space))
			{
				//start looting
				if (curhit.name.Equals(island.name))
				{
					ship.state = Ship.State.Looting;
					print ("now looting");
				}
				//start shopping
				else if (curhit.name.Equals(ship.harbor.name))
				{
					ship.state = Ship.State.Shopping;	
					print ("now shopping");
				}
			}


			//allow user to keep moving at a slower pace in case they don't want to stop
			//Rotate on input
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * ship.turnSpeed * Time.deltaTime);			
			//move forward at min speed
			transform.position += transform.up * (ship.minSpeed) * Time.deltaTime;

		}
	}

	void OnTriggerEnter(Collider hit)
	{
		print("hit a trigger: " + hit.name);
		curhit = hit;

		//if player hit the island
		if (hit.name.Equals(island.name) && (ship.state == Ship.State.Roaming))
		{
			ship.state = Ship.State.Waiting;
			screenText = "Press Space to start looting the island and get some gold!";
		}

		//if player hit the harbor
		else if (hit.name.Equals(ship.harbor.name) && (ship.state == Ship.State.Roaming))
		{
			ship.state = Ship.State.Waiting;
			screenText = "Press Space to enter the harbor, drop off your gold, and buy upgrades.";
		}
	}

	void OnTriggerExit(Collider hit)
	{
		screenText = "";
		ship.state = Ship.State.Roaming;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0,0,190,800), screenText);
	}
	
}
