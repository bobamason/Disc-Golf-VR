using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour {

    public ScoreboardRowController parRow;
    public ScoreboardRowController scoreRow;
    public ScoreboardRowController diffRow;

    public AnimationCurve curve;
    public Vector3 animationOffset;

    private Vector3 positionDefault;
    private Vector3 positionStartAnimation;
    private bool initialized = false;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!initialized)
        {
            positionDefault = transform.localPosition;
            positionStartAnimation = positionDefault + animationOffset;
            initialized = true;
        }
    }

    public void SetParForHole(int hole, int par)
    {
        parRow.SetScoreAtPosition(hole, par);
        parRow.CalculateAndDisplayTotal();
    }

    public void SetScoreForHole(int hole, int score)
    {
        scoreRow.SetScoreAtPosition(hole, score);
        diffRow.SetScoreAtPosition(hole, score - parRow.GetScoreAtPosition(hole));
        scoreRow.CalculateAndDisplayTotal();
        diffRow.ClearTotal();
    }

    public void ClearScoreForHole(int hole)
    {
        scoreRow.ClearScoreAtPosition(hole);
        diffRow.ClearScoreAtPosition(hole);
        scoreRow.CalculateAndDisplayTotal();
        diffRow.ClearTotal();
    }

    public int GetScoreForHole(int hole)
    {
        return scoreRow.GetScoreAtPosition(hole);
    }

    public int GetFinalScore()
    {
        return scoreRow.GetTotal();
    }

    public int GetFinalDifference()
    {
        return diffRow.GetTotal();
    }

    public void CalculateAndDisplayDifferenceTotal()
    {
        scoreRow.CalculateAndDisplayTotal();
        diffRow.CalculateAndDisplayTotal();
    }

    public void ResetAnimation()
    {
        Init();
        transform.localPosition = positionStartAnimation;
    }

    public void Animate()
    {
        Init();
        transform.localPosition = positionStartAnimation;
        StopCoroutine("AnimateCoroutine");
        StartCoroutine("AnimateCoroutine");
    }

    private IEnumerator AnimateCoroutine()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 20; i++)
        {
            transform.localPosition = Vector3.Lerp(positionStartAnimation, positionDefault, curve.Evaluate(i / 20f));
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition = positionDefault;
        yield return null;
    }

    internal void ClearScore()
    {
        for (int i = 0; i < 9; i++)
        {
            scoreRow.ClearScoreAtPosition(i);
            diffRow.ClearScoreAtPosition(i);
        }
        scoreRow.CalculateAndDisplayTotal();
        diffRow.ClearTotal();
    }
}
