using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    private AsyncOperation async;

    public void Load()
    {
        if (async == null)
        {
            Scene curScene = SceneManager.GetActiveScene();
            async = SceneManager.LoadSceneAsync(curScene.buildIndex + 1);
            if (async != null)
            {
                async.allowSceneActivation = true;
            }
            else
            {
                Debug.Log("No Scene found");
            }
        }
    }

    public void Load(int sceneIndex)
    {
        if (async == null)
        {
            async = SceneManager.LoadSceneAsync(sceneIndex);
            if (async != null)
            {
                async.allowSceneActivation = true;
            }
            else
            {
                Debug.Log("No Scene found");
            }
        }
    }

    public void Load(string sceneName)
    {
        if (async == null)
        {
            async = SceneManager.LoadSceneAsync(sceneName);
            if (async != null)
            {
                async.allowSceneActivation = true;
            }
            else
            {
                Debug.Log("No Scene found");
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
