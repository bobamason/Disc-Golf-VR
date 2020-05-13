using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStrokeComplete : GameState
{
    private GvrControllerInputDevice controller;
    private float downTime;

    public override void OnEnterState(GameController gameController)
    {
        controller = gameController.GetController();
        gameController.SetPopupActive(true);
        Popup popup = gameController.GetPopup();
        popup.SetBottomText(null);
        popup.SetExtraLargeText(null);
        int strokes = gameController.GetStrokes();
        int diff = strokes - gameController.GetPar();
        popup.SetLargeText("Stroke " + strokes + "(" + (diff > 0 ? "+" : "") + diff + ")");
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
            if((Time.time - downTime) < 1.0f)
            {
                Vector3 position = gameController.GetDisc().transform.position;
                gameController.MovePlayerToDiscPosition(position);
                gameController.SetState(State.PRE_THROW);
            }
        }
    }
}
