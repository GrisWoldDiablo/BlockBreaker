using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using UnityEngine.Audio;



public class GameManager : MonoBehaviour
{
    private string[] powerTypesNames = 
    {
        "None",
        "Random",
        "Wide Paddle",
        "Shovel Bullets",
        "Mega Fire Ball",
        "Floor Protection",
        "Glue Ball",
        "Ball Speed Down",
        "Extra Ball",
        "Shrink Paddle",
        "Shrink Ball",
        "Ball Speed Up",
        "Bonus Life"
    };
#if UNITY_EDITOR
    [Header("DEBUG!!")]
    [SerializeField] private PowerTypes debugPower = PowerTypes.None;
    [SerializeField] private bool debugActivatePower = false;
    [SerializeField] private int debugLevelToLoad = 0;
    [SerializeField] private bool debugLoadLevel = false;

#endif
    private int points;
    private int maxPoints = 99999999;
	private int blocks = 0;
	private int currLvl = 1;
    private int playerLvl = 1;
	private int bestScore = 0;
	private GameObject currBoard;
	private int lifes = 3;
    private const int maxLife = 3;
    private int credits = 3;
    private float paddleSensitivity;
    public float PaddleSensitivity { get { return paddleSensitivity; } }
    

    // !!! Power Up !!!
    private bool powerOn = false;
	private bool powerActive = true;
	private int speedChangeQty = 0;
	private float powerTime = 0.0f;
	private int bulletsQty = 5;
    public int BulletsQty { get { return bulletsQty; } }
	private Vector2 paddleSpriteOGSize;
	private PowerTypes currentPower;
	public PowerTypes CurrentPower { get { return currentPower; } set { currentPower = value; } }

	private MovePaddle paddleCode;
	public MovePaddle PaddleCode { get { return paddleCode; } }
    private MoveBall ballCode;
	public MoveBall BallCode { get { return ballCode; } }
	
	private bool levelOver = false;
	public bool LevelOver { get { return levelOver; } }

        
    private int playerRank;
    public int PlayerRank { get { return playerRank; } }

    private Settings settingsCode;

    [Header("Levels")]
    [SerializeField] private GameObject[] levelsPrefab;

	[Header("Game UI")]
	[SerializeField] private Text powerTx;
	[SerializeField] private Text scoreText;
    [SerializeField] private Text LevelText;
	[SerializeField] private GameObject[] lifeCountGUI;
	[SerializeField] private GameObject[] bulletCountGUI;
    [SerializeField] private float dayNightCycleInSecond = 30;
    [SerializeField] private GameObject decorObject;
    [SerializeField] private SpriteRenderer oceanSprite;
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private Color oceanNightColor;
    private Color currentDecorColor;
    private Color currentOceanColor;
    private SpriteRenderer[] decorSprites;

    [Header("Menu")]
    [SerializeField] private MenuInteraction menuCode;
    [SerializeField] private Text menuText;
    [SerializeField] private Text menuScoreText;
    [SerializeField] private Text creditText;
    [SerializeField] private Button continueButton;
	[SerializeField] private GameObject startOverButton;
    [SerializeField] private GameObject returnToGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private int playPanelIndex;
    [SerializeField] private int menuPanelIndex;
    [SerializeField] private int leaderboardPanelIndex;
    [SerializeField] private int newRankPanelIndex;
    [SerializeField] private Text lbScoreText;
    [SerializeField] private Text rankedText;

	[Header("Player")]
	[SerializeField] private GameObject ballObject;
	[SerializeField] private GameObject paddleObject;
    [SerializeField] private float newGameBallSpeed = 300.0f;
    [SerializeField] private float ballSpeedIncreasePerLevel = 20.0f;
    [SerializeField] private float ballSpeedMax = 500.0f;
    private float currentLevelBallSpeed;

    [Header("Powers")]
	[SerializeField, Range(1, 20)] private float secondsPowerLast = 5.0f;
	[SerializeField] private float ballSpeedModifier = 1.3f;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private GameObject floorProtector;
	[SerializeField] private GameObject extraBallPrefab;

    [Header("Paddle Sprites")]
	[SerializeField] private Sprite paddleNormalSpr;
	[SerializeField] private Sprite paddleGlueSpr;
	[SerializeField] private Sprite paddleBulletSpr;
    private SpriteRenderer paddleSprite;

    [Header("Ball Sprites")]
    [SerializeField] private Sprite ballNormalSpr;
    [SerializeField] private Sprite ballFireSpr;
    [SerializeField] private Sprite ballGlueSpr;
    private SpriteRenderer ballSprite;

    private float currentTime;
    private bool dayTime = true;

    // Use this for initialization
    public void Start()
    {
        DayAndNightCycle(true);
        settingsCode = GameObject.FindObjectOfType<Settings>();
        paddleSensitivity = settingsCode.Sensitivity;
        ballCode = ballObject.GetComponent<MoveBall>();
        ballSprite = ballObject.GetComponent<SpriteRenderer>();
        paddleCode = paddleObject.GetComponent<MovePaddle>();
        paddleSprite = paddleObject.GetComponent<SpriteRenderer>();
        paddleSpriteOGSize = paddleSprite.size;

        scoreText.text = "SCORE: " + points.ToString("D8");
        powerTx.enabled = false;
        SetCreditText();
        settingsCode.GetLeaderboard();
        bestScore = settingsCode.LeaderboardScores[0];
        rankedText.text = string.Empty;
        LoadLvl();
        InitBallSpeed();
    }

    private void InitBallSpeed()
    {
        ballCode.Speed = newGameBallSpeed;
        currentLevelBallSpeed = newGameBallSpeed;
    }

    private void DayAndNightCycle(bool init = false)
    {
        if (init)
        {
            decorSprites = decorObject.GetComponentsInChildren<SpriteRenderer>();
            currentTime = Time.time;
            foreach (var item in decorSprites)
            {
                item.color = dayColor;
            }
            oceanSprite.color = dayColor;
            return;
        }

        if (dayTime)
        {
            currentDecorColor = Color.Lerp(dayColor, nightColor, (Time.time - currentTime) / dayNightCycleInSecond);
            currentOceanColor = Color.Lerp(dayColor, oceanNightColor, (Time.time - currentTime) / dayNightCycleInSecond);
            if (currentOceanColor == oceanNightColor)
            {
                dayTime = false;
                currentTime = Time.time;
            }
        }
        else
        {
            currentDecorColor = Color.Lerp(nightColor, dayColor, (Time.time - currentTime) / dayNightCycleInSecond);
            currentOceanColor = Color.Lerp(oceanNightColor, dayColor, (Time.time - currentTime) / dayNightCycleInSecond);
            if (currentDecorColor == dayColor)
            {
                dayTime = true;
                currentTime = Time.time;
            }
        }

        oceanSprite.color = currentOceanColor;
        foreach (var item in decorSprites)
        {
            item.color = currentDecorColor;
        }
        
    }
    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
        if (debugActivatePower)
        {
            ActivatePower();
        }
        if (debugLoadLevel)
        {
            debugLoadLevel = false;
            currLvl = Mathf.Clamp(debugLevelToLoad, 1, levelsPrefab.Length);
            playerLvl = currLvl;
            LoadLvl();
        }
#endif
        DayAndNightCycle();

        if (!menuCode.Panels[0].activeInHierarchy)
        {
            settingsCode.InMenuSS.TransitionTo(0.0f);
        }
        else
        {
            settingsCode.InNormalSS.TransitionTo(0.0f);
            
        }
		powerTx.text = (powerTime - Time.time).ToString("N2");
		if (powerOn)
		{
			if ((int)currentPower > 8)
			{
				powerTx.color = Color.red;
			}
			else
			{
				powerTx.color = Color.green;
			}

			switch (currentPower)
			{
				case PowerTypes.GrowPaddle:
					PowerGrowPaddle(true);
					break;
				case PowerTypes.Bullets:
					PowerBullets(true);
					break;
				case PowerTypes.BigBall:
					PowerBigBall(true);
					break;
				case PowerTypes.ShrinkPaddle:
					PowerShrinkPaddle(true);
					break;
				case PowerTypes.ShrinkBall:
					PowerShrinkBall(true);
					break;
				case PowerTypes.FloorProtection:
					FloorProtection(true);
					break;
				case PowerTypes.GlueBall:
					GlueBall(true);
					break;
				case PowerTypes.SpeedUpBall:
					SpeedUpBall(true);
					break;
				case PowerTypes.SpeedDownBall:
					SpeedDownBall(true);
					break;
				default:
					break;
			}
			powerOn = false;
			powerTx.enabled = true;
			powerActive = true;
		}

		
		if ((powerTime < Time.time || BulletsQty == 0 || currentPower == PowerTypes.None) && powerActive)
		{
			TurnOffPowers();
		}
        if (Input.GetButtonDown("PaddleFire"))
        {
            PaddleFire();
        }
        if (Input.GetButtonDown("Cancel") && Time.timeScale != 0.0f)
        {
            PauseGame();
        }

	}

    public void PaddleFire()
    {
        if (currentPower == PowerTypes.Bullets)
        {

            RemoveBullet();
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = paddleObject.transform.position;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        ToggleMenu(true);
        menuCode.PanelToggle(menuPanelIndex);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        menuCode.PanelToggle(playPanelIndex);
    }

	private void TurnOffPowers()
	{
		PowerGrowPaddle(false);     // Turn Off Grow Paddle
		PowerBullets(false);        // Turn Off Bullets
		PowerBigBall(false);        // Turn Off Big Ball
		PowerShrinkPaddle(false);   // Turn Off Shrink Paddle
		PowerShrinkBall(false);     // Turn Off Shrink Ball
		FloorProtection(false);     // Turn Off Floor Protection
		GlueBall(false);            // Turn Off Glue Ball
		if (ballCode.Speed > currentLevelBallSpeed)
		{
			SpeedUpBall(false);         // Turn Off Speed Up Ball
		}
		else if(ballCode.Speed < currentLevelBallSpeed)
		{
			SpeedDownBall(false);       // Turn Off Speed Down Ball 
		}
		currentPower = PowerTypes.None;
		powerActive = false;
		powerTx.enabled = false;
	}

	public void LoadLvl()
	{

        if (currentLevelBallSpeed < ballSpeedMax)
        {
            currentLevelBallSpeed += ballSpeedIncreasePerLevel;
            ballCode.Speed = currentLevelBallSpeed; 
        }
        Debug.Log("Ball Speed : " + ballCode.Speed);
		levelOver = false;
		ballCode.gameObject.SetActive(true);
		if (currBoard)
		{
			Destroy(currBoard);
			GameObject[] allPowers = GameObject.FindGameObjectsWithTag("Power");
			foreach (var item in allPowers)
			{
				Destroy(item);
			}
			GameObject[] allBullets = GameObject.FindGameObjectsWithTag("Bullet");
			foreach (var item in allBullets)
			{
				Destroy(item);
			}
			GameObject[] allExtraBalls = GameObject.FindGameObjectsWithTag("ExtraBall");
			foreach (var item in allExtraBalls)
			{
				Destroy(item);
			}
		}

		blocks = 0;

		if (levelsPrefab.Length != 0)
		{
            currBoard = Instantiate(levelsPrefab[currLvl - 1]);
		}

        LevelText.text = "Level : " + playerLvl;
        LevelText.gameObject.GetComponent<Animator>().SetTrigger("TurnOn");
	}

	public void AddBlock()
	{
		blocks++;
	}

    
	public void Death()
	{
		
        lifes--;
		if (lifes >= 0)
		{
            lifeCountGUI[lifes].SetActive(false);
            ballCode.Init(true);
		}
		else
		{
            Gameover();
        }

        if (LevelOver)
        {
            LoadLvl();
        }
    }

    private void Gameover()
    {
        PauseGame();
        currentPower = PowerTypes.None;
        scoreText.text = "SCORE: " + points.ToString("D8");
        ballCode.gameObject.SetActive(false);
        //SetLoaderboard(points);
        if (credits > 0)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        ToggleMenu(false);
        playerRank = CheckLeaderboard(points);
        if (playerRank == -1 || playerRank > 9)
        {
            SetLoaderboard(playerRank);
            menuCode.PanelToggle(leaderboardPanelIndex);
        }
        else
        {
            menuCode.PanelToggle(newRankPanelIndex);
        }
    }

    private int CheckLeaderboard(int score)
    {
        return settingsCode.LeaderboardScores.FindIndex(x => score > x);
    }

    public void SetLoaderboard(int rank, string playerName = "NEW")
    {
        if (rank == -1)
        {
            rankedText.text = "You ranked over #100";
        }
        else
        {
            rankedText.text = "You ranked #" + (rank + 1).ToString() + ".";
            settingsCode.LeaderboardScores.Insert(rank, points);
            if (rank < 10)
            {
                settingsCode.LeaderboardNames.Insert(rank, playerName);
            }
            for (int i = 0; i < 100; i++)
            {
                PlayerPrefs.SetInt("Score" + i, settingsCode.LeaderboardScores[i]);
            }
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.SetString("Name" + i, settingsCode.LeaderboardNames[i].ToUpper());
            }
            PlayerPrefs.Save();
            settingsCode.GetLeaderboard();
        }
    }

    private void ToggleMenu(bool pause)
	{
        Navigation navigation = settingButton.navigation;
        menuScoreText.text = scoreText.text;
        if (pause)
        {
            menuText.text = "PAUSE";
            navigation.selectOnUp = returnToGameButton.GetComponent<Button>();

        }
        else
        {
            menuText.text = "GAME OVER";
            navigation.selectOnUp = startOverButton.GetComponent<Button>();
        }
        settingButton.navigation = navigation;
        returnToGameButton.SetActive(pause);
        startOverButton.SetActive(!pause);
        continueButton.gameObject.SetActive(!pause);
	}

	public void AddPoints(int value, bool removeBlock = true)
	{
        
		points += value;
        if (points > maxPoints)
        {
            points = maxPoints;
        }
		string preTxt = string.Empty;
		if (points > bestScore)
		{
			preTxt = "BEST\n";
		}
		scoreText.text = preTxt + "SCORE: " + points.ToString("D8");
        if (removeBlock)
        {
            blocks--;
        }
        else
        {
            return;
        }
		if (blocks <= 0)
		{
			if (playerLvl >= levelsPrefab.Length)
			{
				currLvl = Random.Range(1,levelsPrefab.Length + 1);
                Debug.Log("Max Level, Now Random level");
                playerLvl++;
                
            }
            else
            {
                playerLvl++;
                currLvl++;
            }
            Debug.Log("currLvl: " + currLvl);
            Debug.Log("playerLvl: " + playerLvl);
            levelOver = true;
			if (ballCode.transform.parent != null)
			{
				LoadLvl();
			}
		}
	}

	public void LevelOverPaddleActivate()
	{
		ballCode.Init();
		LoadLvl();
	}

	public void DoContinue()
    {
        UnpauseGame();
        credits--;
        SetCreditText();
        ResetLives();
        ResetPoints();
        InitBallSpeed();
        ballCode.Init(true);
        ballCode.gameObject.SetActive(true);
    }

    private void SetCreditText()
    {
        creditText.text = "CREDIT : " + credits;
    }

    public void DoStartOver()
	{
        UnpauseGame();
        playerLvl = 1;
        credits = 3;
        SetCreditText();
        ResetPlay(1);
        InitBallSpeed();
        DayAndNightCycle(true);
    }

	private void ResetPlay(int level)
	{

		currLvl = level;
		ballCode.Init(true);
		ResetLives();
		ResetPoints();
		LoadLvl();
	}

	public void DoQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void ResetLives()
	{
		lifes = maxLife;
		foreach (var item in lifeCountGUI)
		{
			item.SetActive(true);
		}
	}

	private void ResetPoints()
	{
		points = 0;
		scoreText.text = "SCORE: " + points.ToString("D8");
	}

	// Power Up Methods

	private void RemoveBullet()
	{
		bulletCountGUI[--bulletsQty].SetActive(false);
	}

	private void PowerGrowPaddle(bool turnOnOff)
	{
		if (turnOnOff)
		{
			paddleSprite.size = new Vector2(paddleSprite.size.x * 2, paddleSprite.size.y);
		}
		else
		{
			paddleSprite.size = paddleSpriteOGSize;
		}
	}

	private void PowerBullets(bool turnOnOff)
	{
		foreach (var item in bulletCountGUI)
		{
			item.SetActive(turnOnOff);
		}
		if (turnOnOff)
		{
			paddleSprite.sprite = paddleBulletSpr;
		}
		else
		{
			paddleSprite.sprite = paddleNormalSpr;
		}
		bulletsQty = 5;
	}

	private void PowerBigBall(bool turnOnOff)
	{
        if (turnOnOff)
        {
            ballSprite.sprite = ballFireSpr;
        }
        else
        {
            ballSprite.sprite = ballNormalSpr;
        }
		ballCode.BigBall(turnOnOff);
	}

	private void PowerShrinkPaddle(bool turnOnOff)
	{
		if (turnOnOff)
		{
			paddleSprite.size = new Vector2(paddleSprite.size.x / 3, paddleSprite.size.y);
		}
		else
		{
			paddleSprite.size = paddleSpriteOGSize;
		}
	}

	private void PowerShrinkBall(bool turnOnOff)
	{
		ballCode.ShrinkBall(turnOnOff);
	}

	private void FloorProtection(bool turnOnOff)
	{
		floorProtector.SetActive(turnOnOff);
	}

    private void GlueBall(bool turnOnOff)
    {
        ballCode.GlueBall(turnOnOff);
        if (turnOnOff)
        {
            if (ballCode.transform.parent != null)
            {
                paddleCode.AimLine.enabled = true;
            }
            ballCode.CurrentPower = PowerTypes.GlueBall;
            paddleSprite.sprite = paddleGlueSpr;
            ballSprite.sprite = ballGlueSpr;
        }
        else
        {
            paddleCode.AimLine.enabled = false;
            ballCode.CurrentPower = PowerTypes.None;
            paddleSprite.sprite = paddleNormalSpr;
            ballSprite.sprite = ballNormalSpr;
        }
    }

    public void SetPower(PowerTypes incomingPower)
	{
        
		if (incomingPower == PowerTypes.ExtraBall)
		{
			ExtraBall();
        }
        else if (incomingPower == PowerTypes.LifeUp && lifes < maxLife)
        {
            LifeUp();
            LevelText.text = powerTypesNames[(int)incomingPower];
            LevelText.gameObject.GetComponent<Animator>().SetTrigger("TurnOn");
        }
        else if (currentPower == PowerTypes.None && incomingPower != PowerTypes.LifeUp)
		{
            currentPower = incomingPower;
			Debug.Log(currentPower);
			powerOn = true;
			powerTime = Time.time + secondsPowerLast;
            LevelText.text = "Power : " + powerTypesNames[(int)incomingPower];
            LevelText.gameObject.GetComponent<Animator>().SetTrigger("TurnOn");
        }
		else if (incomingPower == PowerTypes.SpeedUpBall)
		{
			SpeedUpBall(true);
		}
		else if (incomingPower == PowerTypes.SpeedDownBall)
		{
			SpeedDownBall(true);
		}
       
    }

	private void ExtraBall()
	{
		Instantiate(extraBallPrefab, new Vector3(paddleCode.transform.position.x, extraBallPrefab.transform.position.y), Quaternion.identity);
	}

	private void SpeedUpBall(bool turnOnOff)
	{
		Debug.Log(ballCode.BallRigidbody2D.velocity);
		if (turnOnOff)
		{
			ballCode.BallRigidbody2D.velocity *= ballSpeedModifier;
			ballCode.Speed *= ballSpeedModifier;
			if (currentPower == PowerTypes.SpeedDownBall)
			{
				speedChangeQty--;
			}
			else
			{
				speedChangeQty++;
			}
			//Debug.Log("Speed UP");
		}
		else
		{
			for (int i = 0; i < speedChangeQty; i++)
			{
				ballCode.BallRigidbody2D.velocity /= ballSpeedModifier;
			}
			
			ballCode.Speed = currentLevelBallSpeed;
			speedChangeQty = 0;
			//Debug.Log("Speed Normal");
		}

	}

	private void SpeedDownBall(bool turnOnOff)
	{
		if (turnOnOff)
		{
			ballCode.BallRigidbody2D.velocity /= ballSpeedModifier;
			ballCode.Speed /= ballSpeedModifier;
            if (ballCode.Speed < newGameBallSpeed / 2)
            {
                ballCode.Speed = newGameBallSpeed / 2;
            }
			if (currentPower == PowerTypes.SpeedUpBall)
			{
				speedChangeQty--;
			}
			else
			{
				speedChangeQty++;
			}
			
			//Debug.Log("Speed Down");
		}
		else
		{
			for (int i = 0; i < speedChangeQty; i++)
			{
				ballCode.BallRigidbody2D.velocity *= ballSpeedModifier;
			}
			ballCode.Speed = currentLevelBallSpeed;
			speedChangeQty = 0;
			//Debug.Log("Speed Normal");
		}
	}

    public void LifeUp()
    {
        lifeCountGUI[lifes].SetActive(true);
        lifes++;
    }

#if UNITY_EDITOR
    private void ActivatePower()
    {
        debugActivatePower = false;
        SetPower(debugPower);
    }
#endif
}
