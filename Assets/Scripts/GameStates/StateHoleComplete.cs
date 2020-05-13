using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHoleComplete : GameState
{
    private GvrControllerInputDevice controller;
    private Rigidbody discRigidbody;
    private float downTime;
    
    public override void OnEnterState(GameController gameController)
    {
        controller = gameController.GetController();
        discRigidbody = gameController.GetDisc().GetComponent<Rigidbody>();
        gameController.SetPopupActive(true);
        Popup popup = gameController.GetPopup();
        int strokes = gameController.GetStrokes();
        int diff= strokes - gameController.GetPar();
        popup.SetExtraLargeText(strokes + "(" + (diff >= 0 ? "+" : "") + diff + ")");
        popup.SetLargeText(null);
        popup.SetBottomText(GetBottomTextString(diff));
        discRigidbody.isKinematic = true;
    }

    private string GetBottomTextString(int diff)
    {
        switch (diff)
        {
            case -3:
                return "Albatross";
            case -2:
                return "Eagle";
            case -1:
                return "Birdie";
            case 0:
                return "Par";
            case 1:
                return "Bogey";
            case 2:
                return "Double Bogey";
            case 3:
                return "Triple Bogey";
            case 4:
                return "Quadruple Bogey";
            default:
                return ((int)Mathf.Abs(diff)) + " " + (diff > 0 ? "over" : "under") + " par";
        }
    }

    public override void OnExitState(GameController gameController)
    {
        gameController.SetPopupActive(false);
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
                gameController.GoToNextHole();
            }
        }
    }
}
