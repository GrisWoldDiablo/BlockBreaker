using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerFollowBall : MonoBehaviour {
    [SerializeField] private GameObject theBall;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, theBall.transform.position.y, 0);
	}
}
