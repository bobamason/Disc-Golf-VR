using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour {

    private bool isLoading;
    public GameObject loadingUI;
    public GameObject levelSelectUI;
    
	void Start () {
        isLoading = false;
        setLoadingUiActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine("LoadSceneAsync", sceneName);
            setLoadingUiActive(true);
            isLoading = true;
        }
    }

    private void setLoadingUiActive(bool active)
    {
        levelSelectUI.SetActive(!active);
        loadingUI.SetActive(active);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
            yield return null;
    }
}
