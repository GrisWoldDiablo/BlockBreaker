using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public enum BrickTypes { Normal, Hard, Indestructible }

#if (UNITY_EDITOR)
[CustomEditor(typeof(BrickBehavior))]
[CanEditMultipleObjects]
public class ObjectBuilderEditor : Editor
{
	private BrickTypes theBrickType;
	private BrickBehavior myScript;

    private void OnEnable()
    {
        myScript = (BrickBehavior)target;
    }

    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        UpdateObject();
    }
    
	public void UpdateObject()
	{
        theBrickType = myScript.BrickType;
        switch (theBrickType)
        {
            case BrickTypes.Normal:
                myScript.GetComponent<SpriteRenderer>().sprite = myScript.NormalBrick;
                break;
            case BrickTypes.Hard:
                myScript.GetComponent<SpriteRenderer>().sprite = myScript.HardBrick;
                break;
            case BrickTypes.Indestructible:
                myScript.GetComponent<SpriteRenderer>().sprite = myScript.IndesBrick;
                break;
            default:
                break;
        }
    }
}
#endif

public class BrickBehavior : MonoBehaviour {

	private Color[] powerColors = new Color[] { Color.clear, Color.clear, // Blank space
		Color.yellow,   // GrowPaddle                       
		Color.cyan,     // Bullets                          
		Color.red,      // BigBall                          
		Color.gray,     // FloorProtection                  
		Color.green,    // GlueBall                         
		new Color(1.0f,0.65f,0.0f), // SpeedDownBall         
		Color.magenta,  // ExtraBall                        
		Color.black,    // ShrinkPaddle                     
		Color.white,    // ShrinkBall                       
		Color.blue,     // SpeedUpBall   
        new Color(0.5f,0.0f,0.8f) // LifeUp
    };                                                      
	private GameManager code;

    [Header("Brick Options")]
    [Tooltip("Normal Brick = base, Hard Brick = base * 2, Insdestructable Brick = base * 3")]
    [SerializeField] private int basePointValue = 100;
	[SerializeField] private BrickTypes brickType;

    [Header("Power Options")]
	[SerializeField] private GameObject powerUp;
    [SerializeField] private PowerTypes powerType;
	[Tooltip("Percentage Power Up spawn chance")]
    [SerializeField, Range(1, 100)] private float powerUpSpawnPercentage = 20.0f;
    [SerializeField] private int powerPointValue = 10;

    [Header("Brick Sprites")]
    [SerializeField] private Sprite normalBrick;
    public Sprite NormalBrick { get { return normalBrick; } set { normalBrick = value; } }
    [SerializeField] private Sprite hardBrick;
    public Sprite HardBrick { get { return hardBrick; } set { hardBrick = value; } }
    [SerializeField] private Sprite indesBrick;
    public Sprite IndesBrick { get { return indesBrick; } set { indesBrick = value; } }

    [Header("Audio")]
    [SerializeField] private GameObject burnSoundObject;


    private int hitAmount;
    private int pointValue;
    public int PointValue { get { return pointValue; } set { pointValue = value; } }
	private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }
    public BrickTypes BrickType { get { return brickType; } }

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		code = GameObject.Find("GameManager").GetComponent<GameManager>();
		switch (brickType)
		{
			case BrickTypes.Normal:
				pointValue = basePointValue;
				hitAmount = 1;
				code.AddBlock();
				break;
			case BrickTypes.Hard:
				pointValue = basePointValue * 2;
				hitAmount = 2;
				code.AddBlock();
				break;
			case BrickTypes.Indestructible:
                pointValue = basePointValue * 3;
                break;
			default:
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D col)
	{

        if (col.gameObject.CompareTag("Bullet"))
		{
			Destroy(col.gameObject);
			AttackBrick(PowerTypes.Bullets);
		}

		if (col.gameObject.CompareTag("Ball"))
		{
			if (brickType == BrickTypes.Normal || brickType == BrickTypes.Hard)
			{
				PowerTypes colPower = col.gameObject.GetComponent<MoveBall>().CurrentPower;
				if (colPower != PowerTypes.BigBall)
				{
					AttackBrick(colPower); 
				}
			}
		}
		if (col.gameObject.CompareTag("ExtraBall"))
		{
			AttackBrick(PowerTypes.ExtraBall);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Ball"))
		{
			PowerTypes colPower = col.gameObject.GetComponent<MoveBall>().CurrentPower;
			AttackBrick(colPower);
		}
	}

	private void AttackBrick(PowerTypes colPower = PowerTypes.None)
	{
		if (colPower != PowerTypes.ShrinkBall)
		{
			if (--hitAmount == 0 || colPower == PowerTypes.BigBall || colPower == PowerTypes.Bullets || colPower == PowerTypes.ExtraBall)
			{
				// Power Up
				if (powerType != PowerTypes.None)
				{
					if (Random.Range(0, 100) < powerUpSpawnPercentage) // Spawn % chance
					{
						GameObject powerUpObject = Instantiate(powerUp);
						powerUpObject.transform.position = transform.position;
						SpriteRenderer powerUpSprite = powerUpObject.GetComponent<SpriteRenderer>();
						PowerBehavior powerUpScript = powerUpObject.GetComponent<PowerBehavior>();
                        powerUpScript.PowerValue = powerPointValue;

						if (powerType != PowerTypes.Random)
						{
							powerUpScript.PowerType = powerType;
							powerUpSprite.color = powerColors[(int)powerType];
						}
						else
						{
							int randomNumber = Random.Range(2, powerColors.Length);
							powerUpSprite.color = powerColors[randomNumber];
							powerUpScript.PowerType = (PowerTypes)randomNumber;
						}
					}

				}

				if (brickType != BrickTypes.Indestructible )
				{
					code.AddPoints(pointValue);
				}
                GameObject sound = Instantiate(burnSoundObject);
                sound.transform.position = this.transform.position;
				Destroy(gameObject);
            }
			else
			{
                spriteRenderer.sprite = normalBrick;
				//sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a / 2);
			} 
		}
		
	}

}
