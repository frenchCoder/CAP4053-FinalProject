using UnityEngine;
using System.Collections;

public class AIShipAgent : MonoBehaviour {

	Ship ship;

	//determines ship strategical behavior
	public int aggressionLevel;

	//current to seek
	public Vector3 target;

	private Transform lootIsland;
	
	public Transform[] enemyShips = new Transform[3];
	
	NavMeshAgent nav;


	// Use this for initialization
	void Start () 
	{
		ship = GetComponent<Ship>();
		ship.state = Ship.State.Roaming;
		nav = GetComponent<NavMeshAgent>();
		lootIsland = GameObject.Find("LootIsland").transform;		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ship.state == Ship.State.Roaming) 
		{
			if(ship.goldInShip < ship.maxGold)
			{
				target = lootIsland.position;
			}
			
			else
			{
				target = ship.harbor.position;
			}
			
			nav.destination = target;
			//if ship.state is roaming
			//update target based on location and aggression level
			//seek(target)

			//ship.attack() 
			//if enemy ship is within a hittable range
			//if ship.enemyShipOnRight || ship.enemyShipOnLeft 
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
			ship.state = Ship.State.Roaming;
		}
		
		
	}

	void OnTriggerEnter(Collider hit)
	{
		print ("hit a trigger");		
		
		if(hit.transform == lootIsland)
		{
			ship.state = Ship.State.Looting;
			nav.speed = 0;
		}
		
		if(hit.transform == ship.harbor)
		{
			ship.goldInHarbor += ship.goldInShip;
			ship.goldInShip = 0;
			ship.state = Ship.State.Shopping;				
		}

	}


}
