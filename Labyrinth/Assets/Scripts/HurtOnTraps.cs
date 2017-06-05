using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtOnTraps : MonoBehaviour {

	private Rigidbody rigidbody;
	private AudioSource hurtSound;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		hurtSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.transform.parent.tag.Contains("Trap")){
			rigidbody.velocity = Vector3.zero;
			Debug.Log("got hit");
			FindObjectOfType<ChangeHealthBar>().ChangeHBar(-1);
			hurtSound.Play();
		}

		if(collision.gameObject.tag.Contains("Trap")){
			rigidbody.velocity = Vector3.zero;
			hurtSound.Play();
		}

		if(collision.gameObject.tag.Contains("wall")){
			rigidbody.velocity = Vector3.zero;
		}
	}
	void OnCollisionExit(Collision collision){
		if(collision.gameObject.transform.parent.tag.Contains("Trap")){
			rigidbody.velocity = Vector3.zero;
		}

		if(collision.gameObject.tag.Contains("Trap")){
			rigidbody.velocity = Vector3.zero;
		}

		if(collision.gameObject.tag.Contains("wall")){
			rigidbody.velocity = Vector3.zero;
		}
	}
}
