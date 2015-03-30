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

	//gold player has in their harbor
	public float goldInHarbor;
	//gold player is carrying on their ship
	public float goldInShip;
	//gold player has overall
	public float goldTotal;
	//max amount of gold a ship can carry
	public float maxGold;

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
		Roaming,
		Waiting
	};

	public enum Upgrade
	{
		Speed,
		MaxGold,
		LeftAttackPower,
		RightAttackPower,
		Hp
	};


	// Use this for initialization
	void Start () 
	{
		//TODO: init variables
		Vector3 temp = transform.forward;
		//temp.x += 90.0f;
		temp.z += 90.0f;
		Debug.DrawRay (transform.position, temp);
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


		//TODO:figure out cannon rays

		Vector3 rightmiddle = transform.right;
		rightmiddle.Normalize ();
		Vector3 leftmiddle = -transform.right;
		leftmiddle.Normalize ();

		Vector3 canDist = new Vector3 (0.2f, 0, 0);
		//Debug.DrawRay (transform.position, rightmiddle, Color.red);
		//Debug.DrawRay (transform.position-canDist, rightmiddle, Color.red);
		//Debug.DrawRay (transform.position+canDist, rightmiddle, Color.red);

		//Debug.DrawRay (transform.position, leftmiddle, Color.red);
		////Debug.DrawRay (transform.position-canDist, leftmiddle, Color.red);
		//Debug.DrawRay (transform.position+canDist, leftmiddle, Color.red);

	}


	//increase gold on ship each tick
	public void loot ()
	{
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
			case Upgrade.LeftAttackPower:
				//TODO: determine how to increase attack power
				leftCannons[0].attackPower++;
				leftCannons[1].attackPower++;
				leftCannons[2].attackPower++;
				break;
			case Upgrade.RightAttackPower:
				//TODO: determine how to increase attack power
				rightCannons[0].attackPower++;
				rightCannons[1].attackPower++;
				rightCannons[2].attackPower++;
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

	/*
	//return true if ship is within range on left side
	public bool enemyShipOnLeft(int range)
	{
		//TODO: add functionality, use ray sensor
	}

	//return true if ship is within range on right side
	public bool enemyShipOnRight(int range)
	{
		//TODO: add functionality, use ray sensor
	}
	*/
}
