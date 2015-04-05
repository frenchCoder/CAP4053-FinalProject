using UnityEngine;
using System.Collections;

public class explosionController : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//destroy this object after it has animated once
		if (animator.GetCurrentAnimatorStateInfo(0).IsTag("done"))
		{
			print ("destroy explode sprite");
			Destroy (this.gameObject);
		}

	}
}