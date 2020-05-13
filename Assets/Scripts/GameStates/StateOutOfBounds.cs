using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOutOfBounds : GameState
{
    private GvrControllerInputDevice controller;
    private float downTime;

    public override void OnEnterState(GameController gameController)
    {
        controller = gameController.GetController();
        gameController.SetPopupActive(true);
        Popup popup = gameController.GetPopup();
        popup.SetLargeText("Out of Bounds");
        popup.SetExtraLargeText(null);
        int strokes = gameController.GetStrokes();
        int diff = strokes - gameController.GetPar();
        popup.SetBottomText("Stroke " + strokes + "(" + (diff > 0 ? "+" : "") + diff + ")");
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
                gameController.SetState(State.PRE_THROW);
            }
        }
    }
}
