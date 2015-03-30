using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	[System.Serializable]
	public class Cannon
	{
		public int attackPower;
		
		public Cannon(int ap)
		{
			attackPower = ap;	
		}		
	}

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
	public Cannon leftCannon;
	public Cannon rightCannon;

	public float points;
	public float health;
	public State state;

	//states a ship can be in at any time
	public enum State
	{
		Looting,
		Shopping,//in harbor
		Roaming,
		Waiting
	};

	public enum Upgrade
	{
		Speed,
		MaxGold,
		AttackPower,
		Hp
	};


	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
		leftCannon = new Cannon (1);
		rightCannon = new Cannon (1);

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
	}

	//increase a ship's property
	public void upgrade (Ship.Upgrade up)
	{
		switch (up)
		{
			case Upgrade.Speed:
				maxSpeed++;
				break;
			case Upgrade.AttackPower:
				//TODO: determine how to increase attack power
				leftCannon.attackPower++;
				rightCannon.attackPower++;
				break;
			case Upgrade.Hp:
				health += 5;//TODO: determine final value
				break;
			case Upgrade.MaxGold:
				maxGold += 25;
				break;
			default:
				print ("no upgrades given");
				break;
		}
	}

}
