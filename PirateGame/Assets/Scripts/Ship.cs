using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	public float maxSpeed;
	public float minSpeed;
	public float curSpeed;
	public float turnSpeed;
	//amount of gold looted per tick
	public float lootingSpeed;
	
	private float lootingTime;
	
	//gold player has in their harbor
	public int goldInHarbor;
	//gold player is carrying on their ship
	public int goldInShip;
	//gold player has overall
	public int goldTotal;
	//max amount of gold a ship can carry
	public int maxGold;

	//cannons on each side of ship, 3 per side
	public List<Cannon> leftCannons;
	public List<Cannon> rightCannons;

	public float points;
	public float health;
	public State state;

	//states a ship can be in at any time
	public enum State
	{
		Looting,
		Shopping,//in harbor
		Roaming
	};


	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check state and call function to perform action based on that state
		if (state == State.Looting)
		{
			//TODO: call loot()
		}
		else if (state == State.Shopping)
		{
			//TODO: TBD
		}
		else if (state == State.Roaming)
		{
			//handled by user or AI object
		}
	}

	//attack by shooting cannons from side of ship
	//"L" for left, "R" for right
	public void attack (string side)
	{
		//TODO: add proper functionality
	}

	//increase gold on ship each tick
	public void loot ()
	{
		if(lootingTime < 0)
		{
			if(goldInShip < maxGold)
			{
				goldInShip++; 
			}
			lootingTime = lootingSpeed;
		}
		
		else
		{
			lootingTime -= Time.deltaTime;
		}
		//TODO: add proper functionality
		//if max gold is reached then return to free roam state
	}

	void OnTriggerEnter(Collider hit)
	{
		print ("hit a trigger");		

		/*TODO: oncollision for cannonhits, 
			if state is not roaming, then cant be hit
			health -= cannon.attackpower
			if heath=0 then ship returns to harbor, enemy ship will receive its gold
			destroy cannon(here or have cannon object handle it)
		*/
	}

	//increase a ship's property
	public void upgrade ()
	{
		//TODO: add functionality
	}

	//return true if ship is within range on left side
	public bool enemyShipOnLeft(int range)
	{
		return false;
		//TODO: add functionality, use adj/pie sensor
	}

	//return true if ship is within range on right side
	public bool enemyShipOnRight(int range)
	{
		return false;
		//TODO: add functionality, use adj/pie sensor
	}

}
