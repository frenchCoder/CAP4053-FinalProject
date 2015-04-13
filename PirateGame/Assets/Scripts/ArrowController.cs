using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

	public GameObject target;
	private Renderer rend;

	void Start ()
	{
		target = GameObject.Find("LootIsland");
		rend = GetComponent<Renderer>();

	}
	
	void Update () {
		PositionArrow();        
	}

	//change rotation on y only
	void PositionArrow()
	{

		rend.enabled = false;
		
		Vector3 v3Pos = Camera.main.WorldToViewportPoint(target.transform.position);
		
				
		if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.z >= 0.0f && v3Pos.z <= 1.0f)
			return; // Object center is visible
		
		rend.enabled = true;
		v3Pos.x -= 0.5f;  // Translate to use center of viewport
		v3Pos.y -= 0.5f; 
		v3Pos.z = 0;      // I think I can do this rather than do a 
		//   a full projection onto the plane

		
		float fAngle = Mathf.Atan2 (v3Pos.x, v3Pos.z);
		transform.localEulerAngles = new Vector3(0.0f, -fAngle * Mathf.Rad2Deg, 0.0f);
		
		v3Pos.x = 0.5f * Mathf.Sin (fAngle) + 0.5f;  // Place on ellipse touching 
		v3Pos.z = 0.5f * Mathf.Cos (fAngle) + 0.5f;  //   side of viewport
		v3Pos.y = Camera.main.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
		transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
	}
}
