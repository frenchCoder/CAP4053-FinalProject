using UnityEngine;
using System.Collections;

public class UserShipAgent : MonoBehaviour {
	
	public Ship ship;
	public Transform harbor;
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
		{
			transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);			
			transform.position += transform.up * curShipSpeed * Time.deltaTime;
			
			curShipSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			curShipSpeed = Mathf.Clamp(curShipSpeed, 0.5f, maxShipSpeed);

			//TODO:add keylisteners for attacking
			//'Q' shoot left cannons
			//'E' shoot right rannons
		}
		else if (ship.state == Ship.State.Looting)
		{
			//TODO
			/*
				display text “press space to stop looting”
				ship object handles looting
			*/
		}
		else if (ship.state == Ship.State.Shopping)
		{
			//TODO
			/*
				display text “press space to raise anchor”
				automatically unload gold off ship to harbor
				display shop menu
			*/
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

}
