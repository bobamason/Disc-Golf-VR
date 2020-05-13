using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateCourseComplete : GameState
{
    private GvrControllerInputDevice controller;
    private Rigidbody discRigidbody;
    private float downTime;

    public override void OnEnterState(GameController gameController)
    {
        controller = gameController.GetController();
        discRigidbody = gameController.GetDisc().GetComponent<Rigidbody>();
        gameController.SetScoreboardActive(true);
        ScoreboardController scoreboard =  gameController.GetScoreboardController();
        int finalScore = scoreboard.GetFinalScore();
        int finalDiff = scoreboard.GetFinalDifference();
        discRigidbody.isKinematic = true;

        string sceneName = SceneManager.GetActiveScene().name;
        string keyScore = "bestScore_" + sceneName;
        string keyDiff = "bestDiff_" + sceneName;
        int bestScore = PlayerPrefs.GetInt(keyScore, -1);
        if(bestScore == -1 || finalScore < bestScore)
        {
            PlayerPrefs.SetInt(keyScore, finalScore);
            PlayerPrefs.SetInt(keyDiff, finalDiff);
        }
    }

    public override void OnExitState(GameController gameController)
    {
        gameController.SetScoreboardActive(false);
    }

    public override void OnUpdate(GameController gameController)
    {
        if (controller.GetButtonDown(GvrControllerButton.TouchPadButton))
        {
            downTime = Time.time;
        }
        if (controller.GetButtonUp(GvrControllerButton.TouchPadButton))
        {
            if ((Time.time - downTime) < 1.0f)
            {
                gameController.QuitGame();
            }
        }
    }
}
