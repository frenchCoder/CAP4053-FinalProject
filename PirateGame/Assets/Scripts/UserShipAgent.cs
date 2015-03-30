using UnityEngine;
using System.Collections;

public class UserShipAgent : MonoBehaviour {
	
	public Ship ship;
	public Transform harbor;
	public GUIFunctions gui;

	public Transform island;
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
		
		ship = (Ship) this.GetComponent(typeof(Ship));
		gui = (GUIFunctions)GameObject.Find ("PlayerShip").GetComponent (typeof(GUIFunctions));

		screenText = "";
		ship.state = Ship.State.Roaming;
		//init ship's harbor
		harbor = GameObject.Find ("PlayerHarbor").transform;
		island = GameObject.Find ("LootIsland").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move/attack according to user if roaming
		if (ship.state == Ship.State.Roaming)
		{
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * ship.turnSpeed * Time.deltaTime);			
			transform.position += transform.up * ship.curSpeed * Time.deltaTime;
			
			ship.curSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			ship.curSpeed = Mathf.Clamp(ship.curSpeed, 0.5f, ship.maxSpeed);

			//TODO:add keylisteners for attacking
			//'Q' shoot left cannons
			//'E' shoot right rannons */

			//No menus should show when the ship is roaming.

		}

		else if (ship.state == Ship.State.Looting)
		{ 
			//listen for space where user wants to stop and roam again
			if (Input.GetKeyUp(KeyCode.Space))
			{
				Vector3 temp = transform.rotation.eulerAngles;
				temp.y += 180.0f;
				transform.rotation = Quaternion.Euler(temp);
				transform.position += transform.up * ship.curSpeed * Time.deltaTime;
				ship.state = Ship.State.Roaming;

				print ("done looting");
			}
			//TODO

				//display text “press space to stop looting”
				//ship object handles looting

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
		else if (ship.state == Ship.State.Waiting) 
		{
			//listen for space where user wants to start looting or shopping
			if (Input.GetKeyUp(KeyCode.Space))
			{
				//start looting
				if (curhit.name.Equals(island.name))
				{
					ship.state = Ship.State.Looting;
										
					screenText = "Press Space when done looting the island.";
					print ("now looting");
				}
				//start shopping
				else if (curhit.name.Equals(harbor.name))
				{
					ship.state = Ship.State.Shopping;					

					print ("now shopping");
				}
			}
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
			screenText = "Press Space to start looting the island \n and get some gold!";
		}

		//if player hit the harbor
		else if (hit.name.Equals(harbor.name) && (ship.state == Ship.State.Roaming))
		{
			ship.state = Ship.State.Waiting;
			screenText = "Press Space to enter the harbor, drop off your gold, \n and buy upgrades.";
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
	
}
