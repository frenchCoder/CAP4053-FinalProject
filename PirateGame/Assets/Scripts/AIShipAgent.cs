using UnityEngine;
using System.Collections;

public class AIShipAgent : MonoBehaviour {

	Ship ship;

	//determines ship strategical behavior
	private int aggressionDistance;

	//current to seek
	private Vector3 target;

	private Transform lootIsland;
	
	private Transform[] enemyShips = new Transform[3];
	
	Transform closestShip;
	float shipDist;
	
	private Ship closestShipBehavior;
	
	NavMeshAgent nav;
	
	public bool debug;
	
	/*public enum Upgrade
	{
		Speed,
		MaxGold,
		AttackPower,
		Hp,
		LootingSpeed
	};*/
	
	public enum AggressionLevel
	{Low, Med, High};

	public AggressionLevel aggressionLevel;
	
	private Ship.Upgrade[] upgrades = new Ship.Upgrade[10];
	private int upgradeIndex = 0;
	
	private bool resettingTarget = false;//only modified in ResetTarget() when ship is respawning
	
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
		
		switch(aggressionLevel)
		{
			case AggressionLevel.Low:
				aggressionDistance = 0;
				upgrades[0] = Ship.Upgrade.LootingSpeed;
				upgrades[1] = Ship.Upgrade.Speed;
				upgrades[2] = Ship.Upgrade.MaxGold;
				upgrades[3] = Ship.Upgrade.LootingSpeed;
				upgrades[4] = Ship.Upgrade.Speed;
				upgrades[5] = Ship.Upgrade.MaxGold;
				upgrades[6] = Ship.Upgrade.Hp;
				upgrades[7] = Ship.Upgrade.Hp;
				upgrades[8] = Ship.Upgrade.AttackPower;
				upgrades[9] = Ship.Upgrade.AttackPower;
				break;
			case AggressionLevel.Med:
				aggressionDistance = 3;
				upgrades[0] = Ship.Upgrade.Speed;
				upgrades[1] = Ship.Upgrade.LootingSpeed;
				upgrades[2] = Ship.Upgrade.MaxGold;
				upgrades[3] = Ship.Upgrade.Hp;
				upgrades[4] = Ship.Upgrade.AttackPower;
				upgrades[5] = Ship.Upgrade.Speed;
				upgrades[6] = Ship.Upgrade.LootingSpeed;
				upgrades[7] = Ship.Upgrade.MaxGold;
				upgrades[8] = Ship.Upgrade.Hp;
				upgrades[9] = Ship.Upgrade.AttackPower;
				break;
			case AggressionLevel.High:
				aggressionDistance = 20;
				upgrades[0] = Ship.Upgrade.Speed;
				upgrades[1] = Ship.Upgrade.AttackPower;
				upgrades[2] = Ship.Upgrade.MaxGold;
				upgrades[3] = Ship.Upgrade.Hp;
				upgrades[4] = Ship.Upgrade.AttackPower;
				upgrades[5] = Ship.Upgrade.Speed;
				upgrades[6] = Ship.Upgrade.Hp;
				upgrades[7] = Ship.Upgrade.MaxGold;
				upgrades[8] = Ship.Upgrade.AttackPower;
				upgrades[9] = Ship.Upgrade.AttackPower;
				break;
		}
		
		nav.speed = 0.75f;
		
		BuyUpgrade(true);
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		GetClosestShip();
						
		//if roaming
		if (ship.state == Ship.State.Roaming && !ship.gamePaused) {

			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.right, out hit, 1)) {
				if (hit.transform.gameObject.layer == 8) {
					ship.attack ("R");
				}
			} else if (Physics.Raycast (transform.position, -transform.right, out hit, 1)) {
				if (hit.transform.gameObject.layer == 8) {
					ship.attack ("L");
				}
			}				
			
			//if able to get gold
			if (ship.goldInShip < ship.maxGold) {
				//if ship is within attack range
				if (shipDist < aggressionDistance) {
					//if the island is closer
					if (Vector3.Distance (lootIsland.position, transform.position) < shipDist && aggressionLevel == AggressionLevel.Med) {
						target = lootIsland.position;
						if (debug)
							print ("lootisland");
					}
				
					//if an opposing ship is closer
					else {
						//if the closest ship has enough gold to warrant an attack
						if (closestShipBehavior.goldInShip > 0) {
							
							if (closestShipBehavior.state == Ship.State.Looting) {
								target = closestShip.position - (Vector3.Normalize (closestShip.transform.up));
							} else {
								if (Vector3.Distance (transform.position, closestShip.position - closestShip.transform.right) < Vector3.Distance (transform.position, closestShip.position + closestShip.transform.right)) {
									target = closestShip.position - (Vector3.Normalize (closestShip.transform.right));
								} else {
									target = closestShip.position + (Vector3.Normalize (closestShip.transform.right));
								}
							}
							
						} else {
							target = lootIsland.position;
						}
					}
				}
				
				//if ship is not within agression level then go for the island
				else {
					target = lootIsland.position;
					if (debug)
						print ("lootisland");
				}
			}
			
			//return to island when ship is full
			else {
				target = ship.harbor.position;
				if (debug)
					print ("harbor");
			}
			
			nav.destination = target;
		} 
		else if (ship.state == Ship.State.Looting) {

			if (aggressionLevel == AggressionLevel.High) {
				if (closestShipBehavior.goldInShip > 0) {
					nav.speed = ship.maxSpeed * .75f;
					ship.state = Ship.State.Roaming;
				}
			}
			
			if (ship.goldInShip < ship.maxGold) {
				ship.loot ();
			} 
			else {
				nav.speed = ship.maxSpeed * .75f;
				ship.state = Ship.State.Roaming;
			}
		}

		//don't let ship move when it is dying and respawning
		else if (ship.state == Ship.State.Dying) {
			nav.speed = 0;
			StartCoroutine (ResetTarget ());
		} 

		//don't let ship move when game pauses while player is shopping
		if (ship.gamePaused) 
		{
			nav.velocity = Vector3.zero;
			nav.speed = 0;
		}
		//let player move again in situation where game is unpaused
		else if (!ship.gamePaused && nav.speed == 0 && !resettingTarget && ship.state == Ship.State.Roaming) 
		{
			nav.speed = ship.maxSpeed*.75f;
			nav.acceleration = 1;
		}
				
	}
		
	IEnumerator ResetTarget()
	{
		resettingTarget = true;
		yield return new WaitForSeconds(2);
		nav.speed = ship.maxSpeed*.75f;
		resettingTarget = false;
	}
	
	void OnTriggerEnter(Collider hit)
	{		
		if(hit.transform == lootIsland)
		{
			ship.state = Ship.State.Looting;
			nav.speed = 0;
			nav.velocity = new Vector3(0,0,0);
		}
		
		if(hit.transform == ship.harbor)
		{
			if(ship.goldInShip > 0)
			{
				

				nav.velocity = new Vector3(0,0,0);
				ship.goldInHarbor += ship.goldInShip;
				ship.goldTotal += ship.goldInShip;//this variable keeps track of total gold collected, so it is never decreased by upgrade purchases
				ship.goldInShip = 0;
				
				while(ship.goldInHarbor >= 100 && upgradeIndex < 10)
				{
					BuyUpgrade(false);
				}
				
				ship.state = Ship.State.Roaming;
				target = lootIsland.position;
				//ship.state = Ship.State.Shopping;
			}
		}

	}
	
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
		closestShipBehavior = enemyShips[i].GetComponent<Ship>();
		shipDist = d;	
		
	}
	
	IEnumerator Deposit()
	{
		
		
		yield return new WaitForEndOfFrame();
		print(transform.name + " " + ship.goldInShip);
		ship.state = Ship.State.Roaming;
		target = lootIsland.position;
	}
	
	void BuyUpgrade(bool beginning)
	{

		if(beginning)
			StartCoroutine("WaitUpgrade");
		
		else
		{
			ship.upgrade(upgrades[upgradeIndex]);
			ship.goldInHarbor-=100;
			
			if(upgrades[upgradeIndex] == Ship.Upgrade.Speed)
			{
				nav.speed = ship.maxSpeed*.75f;
			}
			
			print(transform.name + "bought " + upgrades[upgradeIndex]);
			
			upgradeIndex++;
		}
	}
	
	IEnumerator WaitUpgrade()
	{
		yield return new WaitForEndOfFrame();
		ship.upgrade(upgrades[upgradeIndex]);
		ship.goldInHarbor-=100;
		
		if(upgrades[upgradeIndex] == Ship.Upgrade.Speed)
		{
			nav.speed = ship.maxSpeed*.75f;
		}
		
		print(transform.name + "bought " + upgrades[upgradeIndex]);
		
		upgradeIndex++;
	}


}


