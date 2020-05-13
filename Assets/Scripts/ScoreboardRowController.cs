using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardRowController : MonoBehaviour {

    public Text[] scoreTexts = new Text[9];
    public Text totalText;

    private int[] scores = new int[9];
    public bool alwaysShowSign = false;

	// Use this for initialization
	void Start ()
    {
        ClearAllScores();
        ClearTotal();
    }

    private void ClearAllScores()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scores[i] = 0;
            ClearScoreAtPosition(i);
        }
    }

    public void ClearTotal()
    {
        if (totalText != null) totalText.text = "-";
    }

    public void CalculateAndDisplayTotal()
    {
        if (totalText != null)
        {
            int total = CalculateTotal();
            if (alwaysShowSign)
                totalText.text = (total >= 0 ? "+" : "") + total;
            else
                totalText.text = "" + total;
        }
    }

    private int CalculateTotal()
    {
        int total = 0;
        foreach (int s in scores)
        {
            total += s;
        }

        return total;
    }

    public void ClearScoreAtPosition(int position)
    {
        Text t = scoreTexts[position];
        if (t != null) t.text = "-";
        scores[position] = 0;
    }

    public void SetScoreAtPosition(int position, int score)
    {
        scores[position] = score;
        Text t = scoreTexts[position];
        if (t != null)
        {
            if (alwaysShowSign)
                t.text = (score >= 0 ? "+" : "") + score;
            else
                t.text = "" + score;
        }
    }

    public int GetScoreAtPosition(int position)
    {
        return scores[position];
    }

    public int GetTotal()
    {
        return CalculateTotal();
    }
}
