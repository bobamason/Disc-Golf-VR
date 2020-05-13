using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreDislpay : MonoBehaviour {

    public string[] sceneNames;
    public Text[] texts;

	void Start () {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            SetScoreText(sceneNames[i], texts[i]);
        }
	}

    private void SetScoreText(string sceneName, Text text)
    {
        string keyScore = "bestScore_" + sceneName;
        string keyDiff = "bestDiff_" + sceneName;
        int bestScore = PlayerPrefs.GetInt(keyScore, -1);
        if (bestScore != -1)
        {
            int diff = PlayerPrefs.GetInt(keyDiff, 0);
            text.text = "Best Score: " + bestScore + "(" + (diff > 0 ? "+" : "") + diff  + ")";
        } else
        {
            text.text = "Best Score: --";
        }
    }
}
