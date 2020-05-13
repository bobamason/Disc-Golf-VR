using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public string sceneName;

    void Start () {
        StartCoroutine(LoadMainScene());
	}

    private IEnumerator LoadMainScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while(!async.isDone)
            yield return null;
    }
}
