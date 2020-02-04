using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerSnapshot inMenuSS;
    public AudioMixerSnapshot InMenuSS { get { return inMenuSS; } }
    [SerializeField] private AudioMixerSnapshot inNormalSS;
    public AudioMixerSnapshot InNormalSS { get { return inNormalSS; } }
    [SerializeField] private string masterVolParam;
    public string MasterVolParam { get { return masterVolParam; } }
    [SerializeField] private string musicVolParam;
    public string MusicVolParam { get { return musicVolParam; } }
    [SerializeField] private string sFXVolParam;
    public string SFXVolParam { get { return sFXVolParam; } }
    [SerializeField] private string sensitivityParam;
    public string SensitivityParam { get { return sensitivityParam; } }
    [SerializeField] private Button applyButton;
    public Button ApplyButton { get { return applyButton; } }

    [Header("Setting sliders")]
    [SerializeField] private SetVolume masterVol;
    [SerializeField] private SetVolume musicVol;
    [SerializeField] private SetVolume sfxVol;
    [SerializeField] private SetGFX qualitySetting;
    [SerializeField] private SetSensitivity sensitivityLevel;

    [Header("Leaderboard")]
    [SerializeField] private Text lbScoreText;
    [SerializeField] private Text lbNameText;
    private List<int> leaderboardScores;
    public List<int> LeaderboardScores { get { return leaderboardScores; } set { leaderboardScores = value; } }
    private List<string> leaderboardNames;
    public List<string> LeaderboardNames { get { return leaderboardNames; } set { leaderboardNames = value; } }

    private float sensitivity;
    public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }


    private void Awake()
    {
        // Load settings
        audioMixer.SetFloat(masterVolParam, PlayerPrefs.GetFloat(masterVolParam, 0));
        audioMixer.SetFloat(musicVolParam, PlayerPrefs.GetFloat(musicVolParam, 0));
        audioMixer.SetFloat(sFXVolParam, PlayerPrefs.GetFloat(sFXVolParam, 0));
        qualitySetting.PrefValue = QualitySettings.GetQualityLevel();
        sensitivity = PlayerPrefs.GetFloat(sensitivityParam, 20);
        GetLeaderboard();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveChanges()
    {
        PlayerPrefs.SetFloat(masterVol.NameParam, masterVol.Slider.value);
        PlayerPrefs.SetFloat(musicVol.NameParam, musicVol.Slider.value);
        PlayerPrefs.SetFloat(sfxVol.NameParam, sfxVol.Slider.value);
        int gfxIndex = (int)Mathf.Floor(qualitySetting.Slider.value);
        QualitySettings.SetQualityLevel(gfxIndex, true);
        PlayerPrefs.SetFloat(sensitivityLevel.NameParam, sensitivityLevel.Slider.value);
        sensitivity = sensitivityLevel.Slider.value;
        PlayerPrefs.Save();
        applyButton.interactable = false;
    }

    public void CancelChanges()
    {
        float masterVolValue = PlayerPrefs.GetFloat(masterVol.NameParam, 0);
        masterVol.SetVol(masterVolValue);

        float musicVolValue = PlayerPrefs.GetFloat(musicVol.NameParam, 0);
        musicVol.SetVol(musicVolValue);

        float sfxVolValue = PlayerPrefs.GetFloat(sfxVol.NameParam, 0);
        sfxVol.SetVol(sfxVolValue);

        qualitySetting.SetQuality(qualitySetting.PrefValue);

        float sensitivityValue = PlayerPrefs.GetFloat(sensitivityLevel.NameParam, 20);
        sensitivityLevel.SetSens(sensitivityValue);

        applyButton.interactable = false;

    }

    public void ResetSettings()
    {
        if (masterVol.Slider.value != 0 || musicVol.Slider.value != 0
            || sfxVol.Slider.value != 0 || qualitySetting.Slider.value != 3
            || sensitivityLevel.Slider.value != 20)
        {
            masterVol.SetVol(0);
            musicVol.SetVol(0);
            sfxVol.SetVol(0);
            qualitySetting.SetQuality(3);
            sensitivityLevel.SetSens(20);
        }
    }

    public void GetLeaderboard()
    {
        lbScoreText.text = string.Empty;
        lbNameText.text = string.Empty;
        leaderboardScores = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            leaderboardScores.Add(PlayerPrefs.GetInt("Score" + i, 0));
        }
        leaderboardNames = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            leaderboardNames.Add(PlayerPrefs.GetString("Name" + i, "AAA").ToUpper());
        }
        for (int i = 0; i < 10; i++)
        {
            if (i != 0)
            {
                lbScoreText.text += "\n" + leaderboardScores[i].ToString("D8");
                lbNameText.text += "\n- " + leaderboardNames[i];
            }
            else
            {
                lbScoreText.text += leaderboardScores[i].ToString("D8");
                lbNameText.text += "- " + leaderboardNames[i]; ;
            }
        }

    }

    
}
