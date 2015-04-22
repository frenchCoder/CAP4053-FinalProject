using UnityEngine;
using System.Collections;

public class AIShipAgent : MonoBehaviour {

	Ship ship;

	//determines ship strategical behavior
	public int aggressionLevel;

	//current to seek
	private Vector3 target;

	private Transform lootIsland;
	
	private Transform[] enemyShips = new Transform[3];
	
	Transform closestShip;
	float shipDist;
	
	NavMeshAgent nav;
	
	public bool debug;
	
	// Use this for initialization
	void Start () 
	{
		ship = GetComponent<Ship>();
		ship.state = Ship.State.Roaming;
		ship.shootingRate = 0.75f;//longer wait time for AI ships than player
		nav = GetComponent<NavMeshAgent>();
		lootIsland = GameObject.Find("LootIsland").transform;
		
		enemyShips[0] = GameObject.Find("PlayerShip").transform;
		
		int i = 1;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if(g.transform != transform)
			{
				enemyShips[i] = g.transform;
				i++;
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		GetClosestShip();
		
		//if roaming
		if (ship.state == Ship.State.Roaming && ship.gameStarted) 
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.right, out hit, 1))
			{
				if(hit.transform.gameObject.layer == 8)
				{
					ship.attack("R");
				}
			}
			
			else if(Physics.Raycast(transform.position, -transform.right, out hit, 1))
			{
				if(hit.transform.gameObject.layer == 8)
				{
					ship.attack("L");
				}
			}
				
			
			//if able to get gold
			if(ship.goldInShip < ship.maxGold)
			{
				//if ship is within attack range
				if(shipDist < aggressionLevel)
				{
					//if the island is closer
					if(Vector3.Distance(lootIsland.position, transform.position) < shipDist)
					{
						target = lootIsland.position;
						if (debug) print("lootisland");
					}
				
					//if an opposing ship is closer
					else
					{
						//if the closest ship has enough gold to warrant an attack
						if(closestShip.GetComponent<Ship>().goldInShip >= ship.maxGold - ship.goldInShip)
						{
							if(debug) print(closestShip.GetComponent<Ship>().goldInShip);
							target = closestShip.position - (-Vector3.Normalize(transform.position + closestShip.position));
							if (debug) print("othership");
						}
						
						else
						{
							target = lootIsland.position;
						}
					}
				}
				
				//if ship is not within agression level then go for the island
				else
				{
					target = lootIsland.position;
					if (debug) print("lootisland");
				}
			}
			
			//return to island when ship is full
			else
			{
				target = ship.harbor.position;
				if (debug) print("harbor");
			}
			
			nav.destination = target;
		}
		
		else if (ship.state == Ship.State.Looting)
		{
			if(ship.goldInShip < ship.maxGold)
			{
				ship.loot();
			}
			
			else
			{
				nav.speed = 0.5f;
				ship.state = Ship.State.Roaming;
			}
		}
		
		else if (ship.state == Ship.State.Shopping)
		{
			//TODO: Upgrade code goes here
			//ship.state = Ship.State.Roaming;
			//target = lootIsland.position;
			
		}
		
		//don't let ship move when it is dying and respawning
		else if (ship.state == Ship.State.Dying)
		{
			nav.speed = 0;
			StartCoroutine(ResetTarget());
		}
		
		
	}
	
	IEnumerator ResetTarget()
	{
		yield return new WaitForSeconds(2);
		nav.speed = 0.5f;
	}
	
	void OnTriggerEnter(Collider hit)
	{		
		if(hit.transform == lootIsland)
		{
			ship.state = Ship.State.Looting;
			nav.speed = 0;
		}
		
		if(hit.transform == ship.harbor)
		{
			if(ship.goldInShip > 0)
			{
				ship.goldInHarbor += ship.goldInShip;
				ship.goldTotal += ship.goldInShip;//this variable keeps track of total gold collected, so it is never decreased by upgrade purchases
				ship.goldInShip = 0;
				ship.state = Ship.State.Roaming;
				target = lootIsland.position;
				//ship.state = Ship.State.Shopping;
			}
		}

	}
	
	/*void OnTriggerStay(Collider hit)
	{
		if(hit.transform == ship.harbor)
		{
			if(ship.goldInShip > 0)
			{
				if(ship.state == Ship.State.Roaming)
				{
					print(transform.name + " " + ship.goldInShip);
					StartCoroutine(Deposit());
				}
			}
		}
	}*/
	
	void GetClosestShip()
	{
		float d = 1000f;
		int i = 0;
		
		for(int j = 0; j < 3; j++)
		{
			if(Vector3.Distance(transform.position, enemyShips[j].position) < d)
			{
				d = Vector3.Distance(transform.position, enemyShips[j].position);
				i = j;
			}
		}
		
		closestShip = enemyShips[i];
		shipDist = d;	
		
	}
	
	IEnumerator Deposit()
	{
		
		
		yield return new WaitForEndOfFrame();
		print(transform.name + " " + ship.goldInShip);
		ship.state = Ship.State.Roaming;
		target = lootIsland.position;
	}


}


