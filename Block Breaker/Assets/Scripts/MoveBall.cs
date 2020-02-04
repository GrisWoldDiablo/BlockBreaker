using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MoveBall : MonoBehaviour {

	[SerializeField] private float speed = 300.0f;
	public float Speed { get { return speed; } set { speed = value; } }
	//private float originalSpeed;
	//public float OriginalSpeed { get { return originalSpeed; } set { originalSpeed = value; } }
	private Rigidbody2D ballRigidbody2D;
	public Rigidbody2D BallRigidbody2D { get { return ballRigidbody2D; } set { ballRigidbody2D = value; } }
	private bool onlyOnce = false;
	private Transform myParent;
	private Vector3 initLocPos;
	private GameManager code;

	[SerializeField] private float paddleDirBlindSpot = 0.2f;
	[SerializeField] private float vForceMin = 0.6f;
	[SerializeField] private float vForceMultiplier = 2.0f;
	private TrailRenderer trail;
	private CircleCollider2D colliderTrigger;

	[SerializeField] private CircleCollider2D colliderMain;
	private Vector3 originalScale;
	private float ballScaler = 5;
	private float trailOriginalWitdh;
	private float trailOriginalTime;
	private ParticleSystem bigBallParticle;

	private PowerTypes currentPower = PowerTypes.None;
	public PowerTypes CurrentPower { get { return currentPower; } set { currentPower = value; } }

	private Vector3 gluePosition;
	public Vector3 GluePosition { get { return gluePosition; } }

    [Header("Audio")]
    [SerializeField] private AudioClip normalBounceClip;
    [SerializeField] private AudioSource bigBallFlameSource;
    [SerializeField] private AudioClip bigBallBounceClip;
    [SerializeField] private AudioClip[] glueBallClip;
    [SerializeField] private GameObject deathZoneSplashObject;
    private AudioSource bounceSoundSource;

    [Header("Trail Color")]
    [SerializeField] private Gradient normalGrad;
    [SerializeField] private Gradient fireGrad;
    [SerializeField] private Gradient glueGrad;



	// Use this for initialization
	public void Start () {
		bigBallParticle = GetComponent<ParticleSystem>();
		code = GameObject.Find("GameManager").GetComponent<GameManager>();
		ballRigidbody2D = GetComponent<Rigidbody2D>();
		myParent = transform.parent;
		initLocPos = transform.localPosition;
		trail = GetComponent<TrailRenderer>();
		colliderTrigger = GetComponent<CircleCollider2D>();
		originalScale = ballRigidbody2D.transform.localScale;
		trailOriginalWitdh = trail.startWidth;
		trailOriginalTime = trail.time;
		gluePosition = initLocPos;
        bounceSoundSource = GetComponent<AudioSource>();
    }

	
	
	// Update is called once per frame
	void Update () {

        if (transform.parent == null && ballRigidbody2D.velocity.x < vForceMin / 6.0f && ballRigidbody2D.velocity.x > -vForceMin / 6.0f)
        {
            ballRigidbody2D.AddForce(Random.Range(0, 2) == 1 ? Vector2.left * vForceMultiplier : Vector2.right * vForceMultiplier);
        }

        if (Input.GetButtonDown("PaddleFire"))
        {
            PaddleFire();
        }

        if (currentPower == PowerTypes.BigBall)
        {
            if (!bigBallFlameSource.isPlaying)
            {
                bigBallFlameSource.Play();
            }
        }
        else
        {
            if (bigBallFlameSource.isPlaying)
            {
                bigBallFlameSource.Stop();
            }
            
        }
	}

    public void PaddleFire()
    {

        if (!onlyOnce)
        {
            colliderMain.enabled = true;
            onlyOnce = true;
            ballRigidbody2D.simulated = true;
            ballRigidbody2D.transform.parent = null;

            if (currentPower == PowerTypes.GlueBall)
            {
                ballRigidbody2D.AddForce(code.PaddleCode.AimLineDirection.normalized * speed);
            }
            else
            {
                ballRigidbody2D.AddForce(Vector2.up * speed);
            }
            code.PaddleCode.AimLine.enabled = false;
            trail.enabled = true; 
        }
    }

    public void Init(bool newBall = false) {

        if (currentPower == PowerTypes.BigBall)
        {
            currentPower = PowerTypes.None;
            code.CurrentPower = PowerTypes.None;
        }
        colliderMain.enabled = false;
        trail.enabled = false;
        transform.parent = myParent;
        if (currentPower == PowerTypes.GlueBall && !newBall)
        {
            transform.localPosition = gluePosition;
            code.PaddleCode.AimLine.enabled = true;
        }
        else
        {
            if (currentPower == PowerTypes.ShrinkBall)
            {
                ShrinkBall(false);
                code.CurrentPower = PowerTypes.None;
            }
            else if (currentPower == PowerTypes.GlueBall)
            {
                code.PaddleCode.AimLine.enabled = true;
            }
            transform.localPosition = initLocPos;
            gluePosition = transform.localPosition;
        }
        ballRigidbody2D.simulated = false;
        ballRigidbody2D.velocity = new Vector2(0, 0);
        onlyOnce = false;
    }

    private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Death"))
		{

            if (currentPower == PowerTypes.BigBall)
			{
				BigBall(false);
			}
			code.Death();
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

    private void OnCollisionExit2D(Collision2D col)
	{
        
		if (Mathf.Abs(ballRigidbody2D.velocity.y) < vForceMin)
		{
			float velX = ballRigidbody2D.velocity.x;
			if (ballRigidbody2D.velocity.y >= 0)
			{
				ballRigidbody2D.velocity = new Vector2(velX, vForceMin * vForceMultiplier);
			}
			else
			{
				ballRigidbody2D.velocity = new Vector2(velX, -vForceMin * vForceMultiplier);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
        transform.Rotate(Vector3.forward * 45);
        if (currentPower == PowerTypes.GlueBall)
        {
            bounceSoundSource.clip = glueBallClip[Random.Range(0, glueBallClip.Length)];
        }
        else if (currentPower == PowerTypes.BigBall)
        {
            bounceSoundSource.clip = bigBallBounceClip;
        }
        else
        {
            bounceSoundSource.clip = normalBounceClip;
        }
        bounceSoundSource.Play();
        if (col.gameObject.CompareTag("Paddle"))
		{
			gluePosition = transform.position - col.transform.position;
			if (gluePosition.y > 0)
			{
				if (code.LevelOver)
				{
					code.LevelOverPaddleActivate();
				}
				else if (currentPower == PowerTypes.GlueBall)
				{
					Init();
				}
				else
				{
					float difX = transform.position.x - col.transform.position.x;
					if (difX < -paddleDirBlindSpot)
					{
						// Left side pladdle hit
						ballRigidbody2D.velocity = new Vector2(0, 0);
						ballRigidbody2D.AddForce(new Vector2(-speed / 2, speed));
						//Debug.Log("LEFT SIDE");
					}
					else if (difX > paddleDirBlindSpot)
					{
						// Right side pladdle hit
						ballRigidbody2D.velocity = new Vector2(0, 0);
						ballRigidbody2D.AddForce(new Vector2(speed / 2, speed));
						//Debug.Log("RIGHT SIDE");
					}
				}
			}
		}
		if (col.gameObject.CompareTag("Floor") && code.LevelOver)
		{ 
			code.LevelOverPaddleActivate();
		}
	}

	public void BigBall(bool turnOnOff)
	{
		if (turnOnOff)
		{
			bigBallParticle.Play();
			currentPower = PowerTypes.BigBall;
			if (ballRigidbody2D.transform.parent != null)
			{
				ballRigidbody2D.transform.position += Vector3.up; 
			}
			ballRigidbody2D.transform.localScale = originalScale * ballScaler;
			trail.startWidth = trailOriginalWitdh * ballScaler;
			trail.time = trailOriginalTime * ballScaler;
			colliderTrigger.enabled = true;
            trail.colorGradient = fireGrad;
            bounceSoundSource.pitch = 1.0f;
        }
		else
		{
			bigBallParticle.Stop();
			currentPower = PowerTypes.None;
			trail.time = trailOriginalTime;
			trail.startWidth = trailOriginalWitdh;
			ballRigidbody2D.transform.localScale = originalScale;
			colliderTrigger.enabled = false;
			code.CurrentPower = PowerTypes.None;
            trail.colorGradient = normalGrad;
            bounceSoundSource.pitch = 0.15f;

        }

	}

    public  void GlueBall(bool turnOnOff)
    {
        if (turnOnOff)
        {
            trail.colorGradient = glueGrad;
            bounceSoundSource.pitch = 1.0f;
        }
        else
        {
            trail.colorGradient = normalGrad;
            bounceSoundSource.pitch = 0.15f;
        }
    }

	public void ShrinkBall(bool turnOnOff)
	{
		if (turnOnOff)
		{
			currentPower = PowerTypes.ShrinkBall;
			ballRigidbody2D.transform.localScale = originalScale / ballScaler;
			trail.startWidth = trailOriginalWitdh / ballScaler;
			trail.time = trailOriginalTime / ballScaler;
            bounceSoundSource.pitch = 1.5f;

        }
		else
		{
			currentPower = PowerTypes.None;
			trail.time = trailOriginalTime;
			trail.startWidth = trailOriginalWitdh;
			ballRigidbody2D.transform.localScale = originalScale;
            bounceSoundSource.pitch = 0.15f;
        }

	}

}
