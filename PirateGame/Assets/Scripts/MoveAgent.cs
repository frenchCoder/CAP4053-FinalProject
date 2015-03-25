using UnityEngine;
using System.Collections;

public class MoveAgent : MonoBehaviour {
	
	public float turnSpeed;
	public float maxShipSpeed;
	public float curShipSpeed;
	
	
	// Use this for initialization
	void Start () {
		turnSpeed = 50f;
		maxShipSpeed = 1f;
		curShipSpeed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.red);
		
		//print(transform.up);
		transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);


		transform.position -= transform.forward * curShipSpeed * Time.deltaTime;
		
		curShipSpeed += Input.GetAxis("Vertical") * Time.deltaTime;
		
		curShipSpeed = Mathf.Clamp(curShipSpeed, 0.5f, maxShipSpeed);

	}
	
	void OnTriggerEnter(Collider hit)
	{
		print("hit a trigger");
		
	}
}
