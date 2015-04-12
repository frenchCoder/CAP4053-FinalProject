using UnityEngine;
using System.Collections;

public class cannonController : MonoBehaviour {

	public Transform originShip;
	public Vector3 startPoint;
	public Vector3 endPoint;
	public bool willExplode;

	public float shootingRange;//distance cannon must get to from start point before destroying itself
	public float speed;//move speed

	public Transform explosion;

	//states
	private bool isSeeking;
	private bool startSeek;


	void Start () 
	{
		speed = 3f;		
	}
	
	// Update is called once per frame
	void Update () 
	{

		//start shooting
		if (startSeek)
		{
			startSeek = false;
			isSeeking = true;

			//face endpoint
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(endPoint - transform.position), 150*Time.deltaTime);
		}
		//done shooting
		else if (isSeeking)
		{
			float distFromStart = Vector3.Distance(transform.position, startPoint);
			
			//move towards target
			if (distFromStart < shootingRange)
			{
				transform.position += transform.forward * speed * Time.deltaTime;
			}
			//reached destination
			else
			{
				if (willExplode)
				{
					//spawn explode animation on enemy ship
					Vector3 rotation = new Vector3(90,0,0);
					Instantiate(explosion, transform.position + new Vector3(0,1.2f,0), Quaternion.Euler(rotation));
				}

				//destroy cannon
				Destroy (this.gameObject);
			}

		}

	}

	//called by Ship.cs to shoot a cannon
	public void init (Transform start, Vector3 end, bool explode)
	{
		originShip = start;
		startPoint = originShip.position;
		endPoint = end;
		willExplode = explode;
		shootingRange = Vector3.Distance(transform.position, endPoint);
		startSeek = true;
	}
	
}
