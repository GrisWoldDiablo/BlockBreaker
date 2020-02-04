using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    [Header("Graphics")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Text percentText;
    [Header("Options")]
    [SerializeField] private bool waitForUserInput = false;
    [SerializeField] private float waitForTimeDelay = 0.0f;
    [Header("Scene to load (-1 = next in build index)")]
    [SerializeField] private int sceneIndex = -1;

    private AsyncOperation asyncLoader;



    // Use this for initialization
    void Start () {
        Time.timeScale = 1.0f;
        Input.ResetInputAxes();
        System.GC.Collect();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex != -1 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            asyncLoader = SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            if (currentScene == SceneManager.sceneCountInBuildSettings - 1)
            {
                asyncLoader = SceneManager.LoadSceneAsync(0); 
            }
            else
            {
                asyncLoader = SceneManager.LoadSceneAsync(currentScene + 1);
            }
        }
        if (waitForUserInput)
        {
            asyncLoader.allowSceneActivation = false;
        }
        else if (waitForTimeDelay > 0)
        {
            asyncLoader.allowSceneActivation = false;
            Invoke("TimeDelay", waitForTimeDelay);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey && waitForUserInput)
        {
            asyncLoader.allowSceneActivation = true;
        }

        fillImage.fillAmount = asyncLoader.progress + 0.1f;
        percentText.text = "Loading " + fillImage.fillAmount.ToString("P2");
    }

    private void TimeDelay()
    {
        asyncLoader.allowSceneActivation = true;
    }
}
