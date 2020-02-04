using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour {

	private Rigidbody2D rb;
	[SerializeField] private float speed = 300.0f;
	[SerializeField] private float paddleDirBlindSpot = 0.2f;
	[SerializeField] private float vForceMin = 0.6f;
	[SerializeField] private float vForceMultiplier = 2.0f;
	[SerializeField, Range(1, 20)] private float timeAlive = 5.0f;
    [SerializeField] private GameObject deathZoneSplashObject;
    private AudioSource bounceSoundSource;

    // Use this for initialization
    void Start () {
        bounceSoundSource = GetComponent<AudioSource>();
        timeAlive += Time.time;
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce((Vector2.up + (Random.Range(0, 2) == 0 ? Vector2.right * 0.3f : Vector2.left * 0.3f)).normalized * speed);
	}
	
	// Update is called once per frame
	void Update () {
		if (timeAlive < Time.time)
		{
			Destroy(gameObject);
		}
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

	private void OnCollisionExit2D(Collision2D col)
	{
		if (Mathf.Abs(rb.velocity.y) < vForceMin)
		{
			float velX = rb.velocity.x;
			if (rb.velocity.y >= 0)
			{
				rb.velocity = new Vector2(velX, vForceMin * vForceMultiplier);
			}
			else
			{
				rb.velocity = new Vector2(velX, -vForceMin * vForceMultiplier);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
        bounceSoundSource.Play();

        if (col.gameObject.CompareTag("Paddle"))
		{
			float difX = transform.position.x - col.transform.position.x;
			if (difX < -paddleDirBlindSpot)
			{
				// Left side pladdle hit
				rb.velocity = new Vector2(0, 0);
				rb.AddForce(new Vector2(-speed / 2, speed));
				Debug.Log("LEFT SIDE");
			}
			else if (difX > paddleDirBlindSpot)
			{
				// Right side pladdle hit
				rb.velocity = new Vector2(0, 0);
				rb.AddForce(new Vector2(speed / 2, speed));
				Debug.Log("RIGHT SIDE");
			} 
		}
	}
}
