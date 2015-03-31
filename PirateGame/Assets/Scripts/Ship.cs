﻿using UnityEngine;
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
		maxSpeed = 1;
		minSpeed = 0.2f;
		turnSpeed = 50;
		health = 5;
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

	
	//attack by shooting cannons from side of ship
	//"L" for left, "R" for right
	public void attack (string side)
	{		
		float buffer = 0.000001f;
		RaycastHit hit;
		int sensorRange = 1;
		
		//right side
		if (side.Equals("R"))
		{
			//get nearest ship in shooting range for each cannon
			Vector3 rightdir = transform.right;
			rightdir.Normalize ();
			
			//TODO: add enemy tags to AI ships
			//TODO: do this for all 3 cannons
			if (Physics.Raycast (transform.position, rightdir, out hit, sensorRange + buffer) && hit.transform.tag.Equals("Enemy"))
			{
				//get enemy ship component and implement damage
				GameObject enemy = hit.transform.gameObject;
				Ship enemyShip = (Ship) enemy.GetComponent(typeof(Ship));
				enemyShip.takeDamage(rightCannon);
				
				Debug.DrawRay(transform.position, rightdir * hit.distance, Color.red, 10);
				print ("hit ship");
				
			}
			else 
			{
				Debug.DrawRay(transform.position, rightdir * sensorRange, Color.red, 10);
			}
		}
		//left side
		else if (side.Equals("L"))
		{
			//get nearest ship in shooting range for each cannon
			Vector3 leftdir = -transform.right;
			leftdir.Normalize ();
			
			//TODO: add enemy tags to AI ships
			//TODO: do this for all 3 cannons
			if (Physics.Raycast (transform.position, leftdir, out hit, sensorRange + buffer) && hit.transform.tag.Equals("Enemy"))
			{
				//get enemy ship component and implement damage
				Ship enemyShip = (Ship) hit.transform.GetComponent(typeof(Ship));
				enemyShip.takeDamage(leftCannon);
				
				Debug.DrawRay(transform.position, leftdir * hit.distance, Color.red, 10);
				print ("hit ship: " + hit.transform.name);				
			}
			else 
			{
				Debug.DrawRay(transform.position, leftdir * sensorRange, Color.red, 10);
			}	
		}
	}
	
	public void takeDamage(Cannon cannon)
	{
		//reduce health based on attack power of the attacking cannon
		//ship can only be attacked when roaming
		if (state == State.Roaming)
		{
			health -= cannon.attackPower;
			print ("I, " + this.transform.name + ", have been hit!!");
		}
	}

}
