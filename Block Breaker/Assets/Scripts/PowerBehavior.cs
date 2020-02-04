using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerTypes
{
	None,
	Random,
	GrowPaddle,
	Bullets,
	BigBall,
	FloorProtection,
	GlueBall,
	SpeedDownBall,
	ExtraBall,
	ShrinkPaddle,
	ShrinkBall,
	SpeedUpBall,
    LifeUp
}

public class PowerBehavior : MonoBehaviour {

    [SerializeField] private GameObject deathZoneSplashObject;
    [SerializeField] private PowerTypes powerType;
    private int powerValue;
	public int PowerValue { get { return powerValue; } set { powerValue = value; } }
	public PowerTypes PowerType { get { return powerType; } set { powerType = value; } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            GameObject sound = Instantiate(deathZoneSplashObject);
            sound.transform.position = transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Death"))
		{
			Destroy(gameObject);
		}
	}
}
