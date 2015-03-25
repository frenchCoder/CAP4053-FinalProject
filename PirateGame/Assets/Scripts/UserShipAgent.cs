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

		screenText = "";
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move/attack according to user if roaming
		if (ship.state == Ship.State.Roaming)
		{
			transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * ship.turnSpeed * Time.deltaTime);			
			transform.position -= transform.forward * ship.curSpeed * Time.deltaTime;
			
			ship.curSpeed += Input.GetAxis("Vertical") * Time.deltaTime;			
			ship.curSpeed = Mathf.Clamp(ship.curSpeed, 0.5f, ship.maxSpeed);

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
			/*TODO: island looting trigger
			if ship not in looting state
				display text “press space to start looting”
			if "space" 
				looting state => roaming
				roaming state => looting
			*/
		}
			

		else if (hit.name.Equals("Harbor"))
		{
			/*TODO: harbor shopping trigger
			if ship not in shopping state
				display text “press space to drop anchor”
			if "space" 
				shopping state => roaming
				roaming state => shopping
	 		*/
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
