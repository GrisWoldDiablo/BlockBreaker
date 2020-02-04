using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireShot : MonoBehaviour {

	[SerializeField] private float speed = 10.0f;
	
	// Use this for initialization
	void Start () {
	   GetComponent<Rigidbody2D>().AddForce(Vector3.up * speed); ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnCollisionEnter2D(Collision2D col)
	{

		if (!col.gameObject.CompareTag("Brick"))
		{
			Destroy(gameObject); 
		}
	}
}
