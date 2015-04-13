﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	[System.Serializable]
	public class Cannon
	{
		public int attackPower;//health decremented from hit ship 
		
		public Cannon(int ap)
		{
			attackPower = ap;	
		}		
	}

	public Transform harbor;
	public Transform explosion;

	public float maxSpeed;
	public float minSpeed;
	public float curSpeed;
	public float turnSpeed;
	public float lootingSpeed; //lootingTime max
	
	private float lootingTime;
	

	public int goldInHarbor; //gold player has in their harbor
	public int goldInShip; //gold player is carrying on their ship
	public int goldTotal; //gold player has overall
	public int maxGold; //max amount of gold a ship can carry
	public GoldScript goldGUI;

	//cannons on each side of ship
	public Cannon leftCannon;
	public Cannon rightCannon;

	public float health;
	public HealthBarScript healthBar;
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
		Hp,
		LootingSpeed
	};


	// Use this for initialization
	void Start () 
	{
		healthBar = null;
		goldGUI = null;
		harbor = ((GameObject)GameObject.Find(this.name.Replace("Ship","Harbor"))).transform;
		if(this.name.Contains ("Player"))
		{
			healthBar = (HealthBarScript)((GameObject)GameObject.Find("Health_Rect")).GetComponent<HealthBarScript>();
			goldGUI = (GoldScript)((GameObject)GameObject.Find("GoldBar")).GetComponent<GoldScript>();
		}

		maxSpeed = 1;
		minSpeed = 0.2f;
		curSpeed = 0f;
		turnSpeed = 100;

		goldTotal = 100;
		goldInHarbor = goldTotal;
		maxGold = 100;

		leftCannon = new Cannon (1);
		rightCannon = new Cannon (1);

		health = 5;

		lootingSpeed = 1.5f;
		lootingTime = lootingSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(goldGUI != null)
		{
			if(state == Ship.State.Roaming && goldGUI.state == GoldScript.State.Harbor)
				goldGUI.changeState (goldInShip);
			
			if(state == Ship.State.Shopping && goldGUI.state == GoldScript.State.Ship)
				goldGUI.changeState (goldInHarbor);
		}
		if (state == Ship.State.Shopping && goldInShip > 0)
		{
			goldInHarbor += goldInShip;
			goldTotal += goldInShip;//this variable keeps track of total gold collected, so it is never decreased by upgrade purchases
			goldInShip = 0;

			if(goldGUI != null)
				goldGUI.updateValue(goldInHarbor);
		}

		//respawn in harbor if health is zero
		if (health == 0) 
		{
			transform.position = harbor.position;
			health = 5;
			print("ship respawned in harbor");
			state = Ship.State.Shopping;
			if(healthBar  != null )
				healthBar.resetHealth();

		}
	}

	//increase gold on ship each tick
	public void loot ()
	{
		if(lootingTime < 0)
		{
			if(goldInShip < maxGold)
			{
				goldInShip += 25;
				if(goldGUI != null)
					goldGUI.updateValue (goldInShip);
			}
			lootingTime = lootingSpeed;
		}
		
		else
		{
			lootingTime -= Time.deltaTime * (1/lootingSpeed);
		}
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
				health += 2;//TODO: determine final value
				if(healthBar != null)
				{
					healthBar.updateMaxHealth(2f);
					healthBar.resetHealth();
				}
				break;
			case Upgrade.MaxGold:
				maxGold += 25;
				break;
			case Upgrade.LootingSpeed:
				lootingSpeed-=0.5f;
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

			if (Physics.Raycast (transform.position, rightdir, out hit, sensorRange + buffer) && (hit.transform.tag.Equals("Enemy") || hit.transform.tag.Equals("Player")))
			{
				//get enemy ship component and implement damage
				GameObject enemy = hit.transform.gameObject;
				Ship enemyShip = (Ship) enemy.GetComponent(typeof(Ship));
				int gold = enemyShip.goldInShip;
				enemyShip.takeDamage(rightCannon);
				
				Debug.DrawRay(transform.position, rightdir * hit.distance, Color.red, 10);
				Vector3 rotation = new Vector3(90,0,0);
				Instantiate(explosion, new Vector3 (enemyShip.transform.position.x, 2, enemyShip.transform.position.z), Quaternion.Euler(rotation));
				print ("hit ship");

				//take gold if ship has been destroyed
				if (enemyShip.health == 0)
				{
					int newamount = (goldInShip + gold) % (maxGold+1);
					print ("collect gold from ship: " + newamount );
					goldInShip = newamount;
				}
				
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
				Vector3 rotation = new Vector3(90,0,0);
				Instantiate(explosion, new Vector3 (enemyShip.transform.position.x, 2, enemyShip.transform.position.z), Quaternion.Euler(rotation));
				print ("hit ship: " + hit.transform.name);	

				//take gold if ship has been destroyed
				if (enemyShip.health == 0)
				{
					int newamount = (goldInShip + enemyShip.goldInShip) % maxGold;
					print ("collect gold from ship: " + newamount );
					goldInShip = (goldInShip + enemyShip.goldInShip) % maxGold;
				}
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
			health = (health <= cannon.attackPower) ? 0 : (health - cannon.attackPower);
			if(healthBar != null)
				healthBar.decreaseHealth(health);
			print ("I, " + this.transform.name + ", have been hit!!");
		}
	}

}
