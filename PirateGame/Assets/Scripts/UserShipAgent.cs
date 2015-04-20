using UnityEngine;
using System.Collections;

public class UserShipAgent : MonoBehaviour {
	
	Ship ship;
	public GUIFunctions gui;
	private bool tutorialMode = true;
	private Transform island;
	private Collider curhit;
	private float coins;
	private float count;

	// Use this for initialization
	void Start () 
	{
		coins = 8f;
		count = 0f;
		
		ship = GetComponent<Ship>();
		gui = ((GameObject)GameObject.Find("GUI_Manager")).GetComponent<GUIFunctions>();
		ship.state = Ship.State.Shopping;
		island = GameObject.Find ("LootIsland").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		boundaryCheck ();

		//move/attack according to user if roaming
		if (ship.state == Ship.State.Roaming)
		{
			//Rotate on input
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * ship.turnSpeed * Time.deltaTime);			
			//Always move forward
			transform.position += transform.up * ship.curSpeed * Time.deltaTime;

			//Increase/decrease speed on input
			ship.curSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			ship.curSpeed = Mathf.Clamp(ship.curSpeed, ship.minSpeed, ship.maxSpeed);

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
			count += 1/ship.lootingSpeed;
			if(count >= (coins + (5*ship.lootingSpeed)))
			{
				count = 0;
				gui.ShowNextCoin();
			}

			if(tutorialMode)
				gui.DisplayText(0);
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
			if(tutorialMode)
				gui.DisplayText(0);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				gui.ReturnToSea ();
				print ("done shopping");
			}

			//Note that, if the "shoppingGUI.SetActive(true);" line
			//was put here, then the ShopGUI would constantly be on the
			//screen while the ship's state is set to Shopping. This
			//means that the shop GUI would still be visible and clickable
			//underneath the messages for maxed upgrades and no loot.

		}

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
		curhit = hit;

		//if player hit the island
		if (hit.name.Equals(island.name) && (ship.state == Ship.State.Roaming))
		{
			ship.state = Ship.State.Waiting;
			if(tutorialMode)
				gui.DisplayText(1);
		}

		//if player hit the harbor
		else if (hit.name.Equals(ship.harbor.name) && (ship.state == Ship.State.Roaming))
		{
			ship.state = Ship.State.Waiting;
			if(tutorialMode)
				gui.DisplayText(2);
		}
	}

	void OnTriggerExit(Collider hit)
	{
		if(tutorialMode)
			gui.DisplayText(0);

		if (ship.state == Ship.State.Waiting) 
		{
			ship.state = Ship.State.Roaming;		
		}
	}	

	void boundaryCheck()
	{
		Vector3 left = new Vector3 (-20, 0, 0);
		Vector3 right = new Vector3 (20, 0, 0);
		Vector3 up = new Vector3 (0, 0, 15);
		Vector3 down = new Vector3 (0, 0, -15);
		

		//ocean is 20x by 14z
		//leaving left border
		if (transform.position.x <= -10) 
		{
			transform.position += right;//* ship.curSpeed * Time.deltaTime;
		}
		//leaving right border
		else if (transform.position.x >= 10)
		{
			transform.position += left;
		}

		//leaving down border
		if (transform.position.z <= -7.5f) 
		{
			transform.position += up;
		}
		//leaving up border
		else if (transform.position.z >= 7.5) 
		{
			transform.position += down;
		} 
	}
}
