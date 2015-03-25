using UnityEngine;
using System.Collections;

public class AIShipAgent : MonoBehaviour {

	public Ship ship;

	//determines ship strategical behavior
	public int agressionLevel;

	//current to seek
	public Vector3 target;

	public Transform harbor;


	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ship.state == Ship.State.Roaming) 
		{
			//if ship.state is roaming
			//update target based on location and aggression level
			//seek(target)

			//ship.attack() 
			//if enemy ship is within a hittable range
			//if ship.enemyShipOnRight || ship.enemyShipOnLeft 
		}
		else if (ship.state == Ship.State.Looting)
		{
			
		}
		else if (ship.state == Ship.State.Shopping)
		{
			
		}
	}

	void OnTriggerEnter(Collider hit)
	{

		/*TODO: island looting trigger
			change ship state
			stay on island given amount of time
		*/
		if (hit.tag.Equals("island"))
		{
			ship.state = Ship.State.Looting;
		}

		/*TODO: harbor shopping trigger
			change ship state
			take gold from ship to harbor
			purchase upgrades based on aggressionLevel
		 */
		else if (hit.name.Equals(harbor.name))
		{			
			ship.state = Ship.State.Roaming;

		}	
	}


}
